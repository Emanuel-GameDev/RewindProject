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
    [Header("General Data")]
    [SerializeField] BossPosition startPosition;
    [SerializeField] GameObject bossBody;
    [SerializeField] BossGroundManager groundManager;
    [SerializeField] GameObject targetPlayer;

    [Header("Movement")]
    [Tooltip("Imposta quanto tempo ci mette a muoversi da un punto ad un altro orizzontalmente")]
    [SerializeField] float horizontalMoveDuration = 5f;
    [Tooltip("Imposta quanto tempo ci mette a muoversi da un punto ad un altro verticalmente")]
    [SerializeField] float verticalMoveDuration = 5f;

    [Header("Change Ground Settings")]
    [Tooltip("Imposta dopo quanti colpi il boss inzia a cambiare il terreno/soffitto")]
    [SerializeField] int necessaryHitForChangeGround = 2;
    [Tooltip("Imposta ogni quanto tempo deve ripetersi il cambio terreno/soffitto")]
    [SerializeField] float changeGroundTime = 30f;

    [Header("Sphere Attack Settings")]
    [SerializeField] GameObject projectilePrefab;
    [Tooltip("Imposta il numero di proiettili che vengono sparati")]
    [SerializeField] int numberOfProjectile = 5;
    [Tooltip("Imposta la distanza tra un proiettile e l'altro")]
    [SerializeField] float distanceBetweenProjectile = 5f;
    [Tooltip("Imposta quanto tempo deve passare prima che i proiettili vengano creati")]
    [SerializeField] float waitBeforeSpawn = 2f;
    [Tooltip("Imposta quanto tempo deve passare prima che i proiettili vengano sparati dopo essere stati creati")]
    [SerializeField] float waitBeforeShot = 2f;
    [Tooltip("Imposta la distanza verticale in cui compaiono i proiettili rispetto ai punti di sosta del boss")]
    [SerializeField] float projectileVerticalOffset = 2f;
    [Tooltip("Imposta la distanza orizzontale in cui compaiono i proiettili rispetto ai punti di sosta del boss")]
    [SerializeField] float projectileHorizontalOffset = 5f;
    [Tooltip("Imposta la velocità di movimento dei proiettili")]
    [SerializeField] float projectileSpeed = 750f;
    [Tooltip("Imposta la durata dei proiettili e quanto tempo passa prima che il boss cambi stato dopo aver sparato")]
    [SerializeField] float projectileLifeTime = 5f;

    [Header("Uroboro Attack Settings")]
    [SerializeField] GameObject uroboro1;
    [SerializeField] GameObject uroboro2;
    [SerializeField] BossUroboroEndPoint uroboroEndPoint1;
    [SerializeField] BossUroboroEndPoint uroboroEndPoint2;
    [SerializeField] float uroboroHorizontalSpawnOffset = 2;
    [SerializeField] float uroboroVerticalSpawnOffset = 2;
    [SerializeField] float uroboroSpeed = 10f;
    [SerializeField] float uroboroTimeChange = 5f;

    [Header("Rewindable Attack Settings")]
    [SerializeField] GameObject rewindableProjectilePrefab;
    [SerializeField] float rewindableSpeed = 1000f;
    [SerializeField] float rewindableVerticalOffset = 2;
    [Tooltip("Imposta quanto tempo deve passare prima che il proiettile venga creato")]
    [SerializeField] float rewindableWaitBeforeSpawn = 2f;
    [Tooltip("Imposta quanto tempo deve passare prima che il proiettile venga sparato dopo essere stato creato")]
    [SerializeField] float rewindableWaitBeforeShoot = 2f;
    [Tooltip("Imposta la durata del proiettile e quanto tempo passa prima che il boss cambi stato dopo aver sparato")]
    [SerializeField] float rewindableLifeTime = 5f;

    [Header("Other Settings")]
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
            HitCounterUpdater(1);
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

    public void HitCounterUpdater(int n)
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

    public BossProjectile GenerateProjectile(Vector2 point)
    {
        BossProjectile projectile = Instantiate(projectilePrefab, point, Quaternion.identity).GetComponent<BossProjectile>();
        projectile.Inizialize(Vector2.zero, point, 0);
        projectile.lifeTime = projectileLifeTime;

        return projectile;
    }

    public BossProjectile GenerateRewindable(Vector2 point)
    {
        BossProjectile rewindable = Instantiate(rewindableProjectilePrefab, point, Quaternion.identity).GetComponent<BossProjectile>();
        rewindable.Inizialize(Vector2.zero, point, 0);
        rewindable.lifeTime = rewindableLifeTime;

        return rewindable;
    }

    //FUNZIONI GET
    //====================================================================================================================================
    #region Funzioni Get

    #region Generiche

    public GameObject GetTargetPlayer()
    {
        return targetPlayer;
    }

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

    public BossPosition GetOppositePosition()
    {
        foreach(BossPosition position in positions)
        {
            if(position.GetVerticalPosition() != currentPosition.GetVerticalPosition() 
                && position.GetHorizontalPosition() == currentPosition.GetHorizontalPosition())
                return position;
        }

        return null;
    }
    #endregion

    #region Proiettili

    public GameObject GetProjectilePrefab()
    {
        return projectilePrefab;
    }

    public int GetNumberOfProjectile()
    {
        return numberOfProjectile;
    }

    public float GetDistanceBetweenProjectile()
    {
        return distanceBetweenProjectile;
    }
    public float GetWaitBeforeSpawn()
    {
        return waitBeforeSpawn;
    }

    public float GetWaitBeforeShot()
    {
        return waitBeforeShot;
    }

    public float GetProjectileVerticalOffset()
    {
        return projectileVerticalOffset;
    }

    public float GetProjectileHorizontalOffset()
    {
        return projectileHorizontalOffset;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }
    public float GetProjectileLifeTime()
    {
        return projectileLifeTime;
    }
    #endregion

    #region Uroboro
    public GameObject GetUroboro1()
    {
        return uroboro1;
    }
    public GameObject GetUroboro2()
    {
        return uroboro2;
    }

    public BossUroboroEndPoint GetUroboroEndPoint1()
    {
        return uroboroEndPoint1;
    }

    public BossUroboroEndPoint GetUroboroEndPoint2()
    {
        return uroboroEndPoint2;
    }

    public float GetUroboroHorizontalSpawnOffset()
    {
        return uroboroHorizontalSpawnOffset;
    }
    public float GetUroboroVerticalSpawnOffset()
    {
        return uroboroVerticalSpawnOffset;
    }
    public float GetUroboroSpeed()
    {
        return uroboroSpeed;
    }
    public float GetUroboroTimeChange()
    {
        return uroboroTimeChange;
    }

    #endregion

    #region Poiettile Rewindable

    public float GetRewindableVerticalOffset()
    {
        return rewindableVerticalOffset;
    }

    public float GetRewindableWaitBeforeSpawn()
    {
        return rewindableWaitBeforeSpawn;
    }

    public float GetRewindableWaitBeforeShoot()
    {
        return rewindableWaitBeforeShoot;
    }

    public float GetRewindableLifeTime()
    {
        return rewindableLifeTime;
    }

    public float GetRewindableSpeed()
    {
        return rewindableSpeed;
    }

    #endregion

    #endregion

    #endregion
}


