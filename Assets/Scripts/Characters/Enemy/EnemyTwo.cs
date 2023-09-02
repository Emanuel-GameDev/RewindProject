using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class EnemyTwo : BaseEnemy
{
    [Header("Specific Tree Data")]
    [Tooltip("Imposta la distanza del campo visivo del nemico")]
    [SerializeField] float viewDistance = 20;
    [Tooltip("Imposta la distanza a cui il nemico attaccherà in corpo a corpo")]
    [SerializeField] float meleeDistance = 2;
    [Tooltip("Imposta il tempo che intercorre tra uno sparo e il successivo nella stessa serie")]
    [SerializeField] float timeBetweenShots = 1f;
    [Tooltip("Imposta il tempo che intercorre tra una serie di spari e la successiva")]
    [SerializeField] float timeBetweenShotsSeries = 5f;
    [Tooltip("Imposta il numero di spari da effettuare in ogni serie")]
    [SerializeField] int numberOfShotsPerSerie = 5;
    [Tooltip("Imposta il tempo che intercorre tra uno attacco e il successivo")]
    [SerializeField] float timeBetweenAttacks = 2f;

    [Header("Shoot Data")]
    [Tooltip("Imposta il punto da cui parte il proiettile")]
    [SerializeField] Transform shootPoint;
    [Tooltip("Imposta la velocita di movimento del proiettile")]
    [SerializeField] float projectileSpeed;

    private bool active;


    //Nomi delle variabili nel behaviour tree
    private const string VIEW_DISTANCE = "View Distance";
    private const string MELEE_DISTANCE = "Melee Distance";
    private const string TIME_BETWEEN_SHOTS = "Time Between Shots";
    private const string TIME_BETWEEN_SHOTS_SERIES = "Time Between Shots Series";
    private const string NUMBER_OF_SHOTS_PER_SERIE = "Number Of Shots Per Serie";
    private const string TIME_BETWEEN_ATTACKS = "Time Between Attacks";
    private const string SELF_GAME_OBJECT = "Self Game Object";

    //Nomi delle variabili nell'animator
    private const string ACTIVATION = "Activation";
    private const string DEACTIVATION = "Deactivation";
    private const string ATTACK = "Attack";

    private void Update()
    {
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        if (PlayerIsOnTheRight())
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }

    private bool PlayerIsOnTheRight()
    {
        return target.transform.position.x > transform.position.x;
    }

    public void Activate()
    {
        if (!active)
        {
            active = true;
            animator.SetTrigger(ACTIVATION);
        }
    }

    public void Deactivate()
    {
        if (active)
        {
            active = false;
            animator.SetTrigger(DEACTIVATION);
        }
    }

    public void Shoot()
    {
        Vector2 direction = PlayerIsOnTheRight() ? Vector2.right : Vector2.left;
        ProjectilePool.Instance.GetProjectile().Inizialize(direction, shootPoint.position, projectileSpeed);
        animator.SetTrigger(ATTACK);
    }

    public void MeleeAttack()
    {
        Debug.Log("Slash");
    }

    protected override void InitialSetup()
    {
        base.InitialSetup();
        tree.SetVariableValue(VIEW_DISTANCE, viewDistance);
        tree.SetVariableValue(MELEE_DISTANCE, meleeDistance);
        tree.SetVariableValue(TIME_BETWEEN_SHOTS, timeBetweenShots);
        tree.SetVariableValue(TIME_BETWEEN_SHOTS_SERIES, timeBetweenShotsSeries);
        tree.SetVariableValue(NUMBER_OF_SHOTS_PER_SERIE, numberOfShotsPerSerie);
        tree.SetVariableValue(TIME_BETWEEN_ATTACKS, timeBetweenAttacks);
        tree.SetVariableValue(SELF_GAME_OBJECT, this.gameObject);
    }

}
