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
    [SerializeField] eZone movingArea;

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
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, SpawnCheck);
        hidener = GetComponentInChildren<SpriteLineHidener>();
        hidener.Hide();
    }

    private void SpawnCheck(object obj)
    {
        if(obj is TimelineManager)
        {
            TimelineManager timelineManager = (TimelineManager)obj;
            if (Enum.Equals(timelineManager.actualZone, movingArea))
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        core.SetActive(true);
        animator.SetTrigger(SPAWN);
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
            Spawn();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Despawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TimeZone zone = collision.GetComponent<TimeZone>();
        movingArea = zone.zone;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TimeZone zone = collision.GetComponent<TimeZone>();
        if (zone != null)
        {
            if (Enum.Equals(zone.zone, movingArea))
                Despawn();
        }
    }

    private void Despawn()
    {
        StopChase();
        hidener.Hide();
    }

    private void StopChase()
    {
        tree.SetVariableValue(MOVE, false);
    }
}
