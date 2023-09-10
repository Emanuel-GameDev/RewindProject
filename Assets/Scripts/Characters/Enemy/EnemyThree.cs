using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyThree : BaseEnemy
{
    [Header("Specific Tree Data")]
    [UnityEngine.Tooltip("Imposta la velocità di movimento del nemico")]
    [SerializeField] float speed = 2.5f;
    [UnityEngine.Tooltip("Imposta la distanza massima entro cui vede il bersaglio")]
    [SerializeField] float viewDistance = 15;

    [Header("Other Data")]
    [SerializeField] GameObject core;
    [SerializeField] eZone movingArea;
    [SerializeField] float despawnDistance = 50f;
    [SerializeField] Transform rotationTarget;
    [SerializeField] BodyRotate bodyRotate;
    [SerializeField] bool faiRoar = false;

    private SpriteLineHidener hidener;
    private float elapsedTime;
    private bool isMoving;
    private bool isActive;
    private List<Collider2D> colliders;

    [Header("Suoni")]
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] float timeBetweenWingSound = 1.2f;
    private MainCharacter_SoundsGenerator sourceGenerator;

    //Nomi delle variabili nel behaviour tree
    private const string CORE = "Core";
    private const string VIEW_DISTANCE = "View Distance";
    private const string SPEED = "Speed";
    private const string MOVE = "Move";

    //Nomi delle variabili nell'animator
    private const string SPAWN = "Spawn";
    private const string DESPAWN = "Despawn";
    private const string DEAD = "Dead";
    private const string ROAR = "Roar";




    protected override void InitialSetup()
    {
        base.InitialSetup();
        tree.SetVariableValue(VIEW_DISTANCE, viewDistance);
        tree.SetVariableValue(SPEED, speed);
        if(core != null) tree.SetVariableValue(CORE, core);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, SpawnCheck);
        hidener = GetComponentInChildren<SpriteLineHidener>();
        sourceGenerator = GetComponent<MainCharacter_SoundsGenerator>();
        hidener.Hide();
        elapsedTime = 0f;
        colliders = GetComponentsInChildren<Collider2D>().ToList();
        DisactivateColliders();
        animator.SetBool(ROAR, faiRoar);
    }

    public override void OnDie()
    {
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

    public void Spawn()
    {
        if (!isActive)
        {
            isActive = true;
            transform.position = startPosition;
            sourceGenerator.PlaySound(spawnSound);
            core.SetActive(true);
            animator.SetTrigger(SPAWN);
            hidener.Hide();
            bodyRotate.SetTarget(rotationTarget);
        }
    }
    public void StartChase()
    {
        if (!faiRoar)
        {
            tree.SetVariableValue(MOVE, true);
            isMoving = true;
        }
        hidener.Show();
        bodyRotate.SetTarget(target.transform);
        ActivateColliders();
    }

    public void AferRoarStart()
    {
        tree.SetVariableValue(MOVE, true);
        isMoving = true;
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
                sourceGenerator.PlayFootStepSound();
                elapsedTime = 0;
            }
            else
            {
                elapsedTime += Time.deltaTime;
            }
        }

        //test
        //if(Input.GetKeyDown(KeyCode.L)) Spawn(); 

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

    public void StopChase()
    {
        sourceGenerator.PlaySound(deathSound);
        tree.SetVariableValue(MOVE, false);
        isMoving = false;
        bodyRotate.SetTarget(rotationTarget);
        animator.SetTrigger(DESPAWN);
        DisactivateColliders();
        isActive = false;
    }

    public void HideBody()
    {
        hidener.Hide();
    }

    public void CompleteDespawn()
    {
        
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

    private void ActivateColliders()
    {
        foreach (Collider2D coll in colliders)
        {
            coll.enabled = true;
        }
    }
    private void DisactivateColliders()
    {
        foreach (Collider2D coll in colliders)
        {
            coll.enabled = false;
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        tree.SetVariableValue(SPEED, speed);
    }

    public void SetStartPosition(Vector2 start)
    {
        startPosition = start;
    }

}
