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
    private StateMachine<eBossState> stateMachine;
    private List<BossPosition> positions;
    private BossPosition currentPosition;
    [SerializeField] BossPosition startPosition;
    [SerializeField] float moveDuration = 5f;
    [SerializeField] GameObject bossBody;

    
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
        stateMachine.SetState(eBossState.Start);
    }

    private void InitialSetup()
    {
        positions = GetComponentsInChildren<BossPosition>().ToList();
        currentPosition = startPosition;
        transform.position = startPosition.transform.position;
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

    public float GetMoveDuration()
    {
        return moveDuration;
    }

    
    #endregion

    #endregion
}


