using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum eBossState
{
    Start,
    Moving,
    Falling,
    Stunned,
    UroboroAttack,
    RewindableAttack,
    HorizontalSphereAttack,
    VerticalSphereAttack,
    ChangeGroundAttack
}

public class BossBheaviour : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] BossPosition startPosition;
    [SerializeField] GameObject bossBody;
    [SerializeField] GameObject topGround;
    [SerializeField] GameObject bottomGround;

    [Header("Value")]
    [Tooltip("Imposta quanto tempo ci mette a muoversi da un punto ad un altro orizzontalmente")]
    [SerializeField] float horizontalMoveDuration = 5f;
    [Tooltip("Imposta quanto tempo ci mette a muoversi da un punto ad un altro verticalmente")]
    [SerializeField] float verticalMoveDuration = 5f;
    [Tooltip("Imposta in quanto tempo compare il terreno/soffitto durante il ribaltamento")]
    [SerializeField] float groundFadeInDuration = 5f;
    [Tooltip("Imposta in quanto tempo scompare il terreno/soffitto durante il ribaltamento")]
    [SerializeField] float groundFadeOutDuration = 5f;
    [Tooltip("Imposta per quanto tempo vene scosso il terreno/soffitto prima di iniziare a scomparire")]
    [SerializeField] float groundShakeDuration = 5f;
    [Tooltip("Imposta dopo quanti colpi il boss inzia a cambiare il terreno/soffitto")]
    [SerializeField] int necessaryHitForChangeGround = 2;

    private StateMachine<eBossState> stateMachine;
    private List<BossPosition> positions;
    private BossPosition currentPosition;
    private int hitCounter;
    private bool changeGroundStarted;
    private eBossState nextState;
    private float changeGroundCountdown;


    // Start is called before the first frame update
    void Start()
    {
        StateMachineSetup();
        InitialSetup();
    }


    // Update is called once per frame
    void Update()
    {
        stateMachine.StateUpdate();
    }


    private void StateMachineSetup()
    {
        stateMachine = new StateMachine<eBossState>();
        stateMachine.RegisterState(eBossState.Start, new Start(this));
        stateMachine.RegisterState(eBossState.Moving, new Moving(this));
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

    //FUNZIONI PUBBLICHE
    //====================================================================================================================================
    #region Funzioni Pubbliche
    public void ChangeState(eBossState state)
    {
        stateMachine.SetState(state);
    }

    public void SetCurrentPosition(BossPosition newPosition)
    {
        currentPosition = newPosition;
    }

    public eBossState NextStateChooser()
    {
        eBossState nextState = eBossState.Start;

        if (changeGroundStarted && changeGroundCountdown <= 0)
            nextState = eBossState.ChangeGroundAttack;
        else
        {

        }


        return nextState;
    }

    public void HitCounteurUpdater(int n)
    {
        hitCounter += n;
        if(hitCounter >= necessaryHitForChangeGround && !changeGroundStarted)
        {
            changeGroundStarted = true;
        }
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

    public float GetGroundFadeInDuration()
    {
        return groundFadeInDuration;
    }

    public float GetGroundFadeOutDuration()
    {
        return groundFadeOutDuration;
    }

    public float GetGroundShakeDuration()
    {
        return groundShakeDuration;
    }

    
    #endregion

    #endregion
}


