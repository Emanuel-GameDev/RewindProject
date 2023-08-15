using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum eBossState
{
    Start,
    Moving,
    Falling,
    Stunned,
    UroboroAttack,
    RewindableAttack,
    SphereAttack,
    ChangeGroundAttack
}

public class BossBheaviour : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] BossPosition startPosition;
    [SerializeField] GameObject bossBody;
    [SerializeField] BossGroundManager groundManager;

    [Header("Value")]
    [Tooltip("Imposta quanto tempo ci mette a muoversi da un punto ad un altro orizzontalmente")]
    [SerializeField] float horizontalMoveDuration = 5f;
    [Tooltip("Imposta quanto tempo ci mette a muoversi da un punto ad un altro verticalmente")]
    [SerializeField] float verticalMoveDuration = 5f;
    [Tooltip("Imposta dopo quanti colpi il boss inzia a cambiare il terreno/soffitto")]
    [SerializeField] int necessaryHitForChangeGround = 2;
    [Tooltip("Imposta ogni quanto tempo deve ripetersi il cambio terreno/soffitto")]
    [SerializeField] float changeGroundTime = 30f;
    [Tooltip("Imposta quanto è probabile che esegua nuovamente la stessa mossa di seguito in relazione alle altre (1 stessa probabilità delle altre, 0 nessuna probabilità)")]
    [Range(0f, 1f)]
    [SerializeField] float repeatPercentage = 0.5f;

    private StateMachine<eBossState> stateMachine;
    private List<BossPosition> positions;
    private BossPosition currentPosition;
    private int hitCounter;
    private bool changeGroundStarted;
    private eBossState currentState;
    private eBossState nextState;
    private float changeGroundCountdown;


    void Start()
    {
        StateMachineSetup();
        InitialSetup();
    }

    void Update()
    {
        stateMachine.StateUpdate();

        if(changeGroundCountdown > 0) changeGroundCountdown -= Time.deltaTime;

        //Temporaneo per test
        if (Input.GetKeyDown(KeyCode.H))
        {
            HitCounteurUpdater(1);
        }
    }

    private void StateMachineSetup()
    {
        stateMachine = new StateMachine<eBossState>();
        stateMachine.RegisterState(eBossState.Start, new Start(this));
        stateMachine.RegisterState(eBossState.Moving, new Moving(this));
        stateMachine.RegisterState(eBossState.UroboroAttack, new UroboroAttack(this));
        stateMachine.RegisterState(eBossState.SphereAttack, new SphereAttack(this));
        stateMachine.RegisterState(eBossState.RewindableAttack, new RewindableAttack(this));
        stateMachine.RegisterState(eBossState.ChangeGroundAttack, new ChangeGroundAttack(this));
        stateMachine.SetState(eBossState.Start);
    }

    private void InitialSetup()
    {
        positions = GetComponentsInChildren<BossPosition>().ToList();
        currentPosition = startPosition;
        transform.position = startPosition.transform.position;
        hitCounter = 0;
        changeGroundStarted = false;
        changeGroundCountdown = 0;
    }

    //FUNZIONI CAMBIO STATO
    //====================================================================================================================================
    #region ChangeStateFunction
    private eBossState NextStateChooser()
    {
        eBossState nextState = eBossState.Start;

        if (changeGroundStarted && changeGroundCountdown <= 0)
            nextState = eBossState.ChangeGroundAttack;
        else if (Enum.Equals(currentState, eBossState.Falling))
        {
            nextState = eBossState.Stunned;
        }
        else if (Enum.Equals(currentState, eBossState.Stunned))
        {
            nextState = eBossState.Moving;
        }
        else
        {
            List<eBossState> possibleStates = new List<eBossState>();

            if (Enum.Equals(currentState, eBossState.Start) || Enum.Equals(currentState, eBossState.Moving))
            {
                possibleStates.Add(eBossState.UroboroAttack);
                possibleStates.Add(eBossState.SphereAttack);
            }
            else
            {
                possibleStates = GeneratePossibleStateList(new List<eBossState>() { eBossState.SphereAttack, eBossState.UroboroAttack, eBossState.RewindableAttack, eBossState.Moving });
            }
            nextState = possibleStates[Random.Range(0, possibleStates.Count)];
        }

        return nextState;
    }

    private List<eBossState> GeneratePossibleStateList(List<eBossState> cosideredStates)
    {
        List<eBossState> possibleStates = new List<eBossState>();
        int totalAmount = 100;
        int amountForSingleState = totalAmount / cosideredStates.Count();
        if (cosideredStates.Contains(currentState))
        {
            int reducedChance = Mathf.RoundToInt(amountForSingleState * repeatPercentage);
            for(int i = 0; i < reducedChance; i++)
            {
                possibleStates.Add(currentState);
            }
            amountForSingleState = (totalAmount - reducedChance) / (cosideredStates.Count() - 1);
        }

        foreach(eBossState state in cosideredStates)
        {
            if(state != currentState)
            {
                for (int i = 0; i < amountForSingleState; i++)
                {
                    possibleStates.Add(state);
                }
            }
        }

        return possibleStates;
    }
    #endregion

    //FUNZIONI PUBBLICHE
    //====================================================================================================================================
    #region Funzioni Pubbliche
    public void ChangeState(eBossState state)
    {
        stateMachine.SetState(state);
        currentState = state;
    }

    public void ChangeState()
    {
        if(nextState == currentState)
        {
            nextState = NextStateChooser();
        }
        ChangeState(nextState);
    }

    public void ResetChangeGroundCountdown()
    {
        changeGroundCountdown = changeGroundTime;
    }

    public void SetCurrentPosition(BossPosition newPosition)
    {
        currentPosition = newPosition;
    }

    public void HitCounteurUpdater(int n)
    {
        hitCounter += n;
        if(hitCounter >= necessaryHitForChangeGround && !changeGroundStarted)
        {
            changeGroundStarted = true;
        }
    }

    public void SetNextState(eBossState nextState)
    {
        this.nextState = nextState;
    }


    //FUNZIONI GET
    //====================================================================================================================================
    #region Funzioni Get
    public List<BossPosition> GetPositions()
    {
        return positions;
    }
    
    public BossPosition GetCurrentPosition()
    {
        return currentPosition;
    }

    public GameObject GetBossBody()
    {
        return bossBody;
    }

    public float GetHorizontalMoveDuration()
    {
        return horizontalMoveDuration;
    }

    public float GetVerticalMoveDuration()
    {
        return verticalMoveDuration;
    }

    public eBossState GetNextState()
    {
        return nextState;
    }

    public BossGroundManager GetBossGroundManager()
    {
        return groundManager;
    }


    #endregion

    #endregion
}


