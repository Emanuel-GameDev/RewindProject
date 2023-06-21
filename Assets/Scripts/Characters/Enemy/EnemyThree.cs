using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : BaseEnemy
{
    [Header("Specific Tree Data")]
    [Tooltip("Imposta la velocità di movimento del nemico")]
    [SerializeField] float speed = 2.5f;
    [Tooltip("Imposta la distanza massima entro cui vede il bersaglio")]
    [SerializeField] float viewDistance = 15;

    [Header("Other Data")]
    [SerializeField] GameObject core;
    private SpriteLineHidener hidener;

    //Nomi delle variabili nel behaviour tree
    private const string CORE = "Core";
    private const string VIEW_DISTANCE = "View Distance";
    private const string SPEED = "Speed";
    private const string MOVE = "Move";

    //Nomi delle variabili nell'animator
    private const string SPAWN = "Spawn";



    protected override void InitialSetup()
    {
        base.InitialSetup();
        tree.SetVariableValue(VIEW_DISTANCE, viewDistance);
        tree.SetVariableValue(SPEED, speed);
        if(core != null) tree.SetVariableValue(CORE, core);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, Spawn);
        hidener = GetComponentInChildren<SpriteLineHidener>();
    }

    private void Spawn(object obj)
    {
        if(Vector2.Distance(target.transform.position, startPosition) < viewDistance) 
        {
            core.SetActive(true);
            animator.SetTrigger(SPAWN);
            StartHiddenBody();
        }
    }

    private void StartHiddenBody()
    {
        hidener.Hide();
    }

    public void StartChase()
    {
        tree.SetVariableValue(MOVE, true);
        hidener.Show();

    }

    //Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Spawn(this);
        }
    }


}
