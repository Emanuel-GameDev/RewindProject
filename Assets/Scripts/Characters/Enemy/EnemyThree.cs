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
    [SerializeField] eZone movingArea;
    [SerializeField] float despawnDistance = 50f;
    [SerializeField] Transform rotationTarget;
    [SerializeField] BodyRotate bodyRotate;

    private SpriteLineHidener hidener;
    private float elapsedTime;
    private bool isMoving;

    [Header("Suoni")]
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] float timeBetweenWingSound = 1.2f;

    //Nomi delle variabili nel behaviour tree
    private const string CORE = "Core";
    private const string VIEW_DISTANCE = "View Distance";
    private const string SPEED = "Speed";
    private const string MOVE = "Move";

    //Nomi delle variabili nell'animator
    private const string SPAWN = "Spawn";
    private const string DESPAWN = "Despawn";
    private const string DEAD = "Dead";




    protected override void InitialSetup()
    {
        base.InitialSetup();
        tree.SetVariableValue(VIEW_DISTANCE, viewDistance);
        tree.SetVariableValue(SPEED, speed);
        if(core != null) tree.SetVariableValue(CORE, core);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, SpawnCheck);
        hidener = GetComponentInChildren<SpriteLineHidener>();
        hidener.Hide();
        elapsedTime = timeBetweenWingSound;
    }

    public override void OnDie()
    {
        GetComponent<AudioSourceGenerator>().PlaySound(deathSound);
        isDead = true;
        animator.SetBool(DEAD, true);
        tree.SetVariableValue(IS_DEAD, isDead);
        coll.enabled = false;
        StopChase();
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        tree.RestartWhenComplete = true;
    }

    private void SpawnCheck(object obj)
    {
        if(obj is TimelineManager)
        {
            TimelineManager timelineManager = (TimelineManager)obj;
            if (Enum.Equals(timelineManager.actualZone, movingArea) && !isDead)
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        GetComponent<AudioSourceGenerator>().PlaySound(spawnSound);
        core.SetActive(true);
        animator.SetTrigger(SPAWN);
        hidener.Hide();
        bodyRotate.SetTarget(rotationTarget);
    }
    public void StartChase()
    {
        tree.SetVariableValue(MOVE, true);
        isMoving = true;
        hidener.Show();
        bodyRotate.SetTarget(target.transform);
    }

    //Test
    private void Update()
    {
        if(Vector2.Distance(startPosition,transform.position) > despawnDistance)
        {
            Despawn(movingArea);
        }
        if (isMoving)
        {
            if(elapsedTime > timeBetweenWingSound)
            {
                GetComponent<MainCharacter_SoundsGenerator>().PlayFootStepSound();
                elapsedTime = 0;
            }
            else
            {
                elapsedTime += Time.deltaTime;
            }
        }
    }

    public void SetMoovingZone(eZone zone)
    {
        if (!Enum.Equals(zone, movingArea))
            movingArea = zone;
    }

    public void Despawn(eZone zone)
    {
        if (Enum.Equals(zone, movingArea))
        {
            StopChase();
        }
    }

    private void StopChase()
    {
        GetComponent<AudioSourceGenerator>().PlaySound(deathSound);
        tree.SetVariableValue(MOVE, false);
        isMoving = false;
        bodyRotate.SetTarget(rotationTarget);
        animator.SetTrigger(DESPAWN);
    }

    public void HideBody()
    {
        hidener.Hide();
    }

    public void CompleteDespawn()
    {
        core.SetActive(false);
        transform.position = startPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, despawnDistance);
    }

    public GameObject GetTarget()
    {
        return target;
    }

}
