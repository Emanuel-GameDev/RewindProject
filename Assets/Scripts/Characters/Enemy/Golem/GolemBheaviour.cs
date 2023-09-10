using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;


public class GolemBheaviour : MonoBehaviour
{
    [Header("Golem Data")]
    [Tooltip("Imposta il bersaglio del nemico")]
    [SerializeField] protected GameObject target;
    [Tooltip("Imposta i punti vita del nemico")]
    [SerializeField] int healtPoint = 2;
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

    private Damageable damageable;
    private Animator animator;
    private Collider2D coll;
    private Vector2 startPosition;

    private bool active;
    private bool transformed;
    private bool isDead;
    private bool isAttacking;

    private int shootCount;
    private float elapsedTime;

    //Nomi delle variabili nell'animator
    private const string ACTIVATION = "Activation";
    private const string DEACTIVATION = "Deactivation";
    private const string ATTACK = "Attack";
    private const string TRANSFORMED = "Transformed";
    private const string HITTED = "Hitted";
    private const string DEAD = "Dead";

    private void Awake()
    {
        InitialSetup();
    }
    private void Update()
    {
        if (!isDead)
        {
            CheckTargetDistance();

            if (active)
            {
                RotateToTarget();
                Attack();
            }
        }
    }

    private void InitialSetup()
    {
        coll = GetComponent<Collider2D>();
        startPosition = transform.position;

        damageable = GetComponentInChildren<Damageable>();
        if (damageable != null)
            damageable.maxHealth = healtPoint;

        animator = GetComponent<Animator>();
        if (GetComponent<Animator>() == null)
            animator = GetComponentInChildren<Animator>();

        ResetEnemy();
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


    private void CheckTargetDistance()
    {
        float distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);
        if (distanceFromTarget < viewDistance)
        {
            Activate();
            if(distanceFromTarget < meleeDistance)
            {
                MeleeAsset();
            }
            else
            {
                RangeAsset();
            }
        }
        else if (!isAttacking && shootCount <= 0)
            Deactivate();


    }

    public void Shoot()
    {
        Vector2 direction = PlayerIsOnTheRight() ? Vector2.right : Vector2.left;
        ProjectilePool.Instance.GetProjectile().Inizialize(direction, shootPoint.position, projectileSpeed);
    }

    public void OnDie()
    {
        isDead = true;
        animator.SetTrigger(DEAD);
        coll.enabled = false;
    }
    public void OnHit()
    {
        if (!isAttacking) animator.SetTrigger(HITTED);
    }

    public void ResetEnemy()
    {
        transform.position = startPosition;
        isDead = false;
        coll.enabled = true;
        active = false;
        transformed = false;
        isAttacking = false;
        ResetShootValue();
    }

    private void ResetShootValue()
    {
        shootCount = 0;
        elapsedTime = 0;
    }

    public void Activate()
    {
        if (!active)
        {
            active = true;
            animator.SetTrigger(ACTIVATION);
            ResetShootValue();
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

    private void RangeAsset()
    {
        if (transformed)
        {
            transformed = false;
            animator.SetBool(TRANSFORMED, false);
            ResetShootValue();
        }
    }

    private void MeleeAsset()
    {
        if (!transformed)
        {
            transformed = true;
            animator.SetBool(TRANSFORMED, true);
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            elapsedTime += Time.deltaTime;
            if (transformed)
            {
                if (elapsedTime > timeBetweenAttacks)
                {
                    AnimationAttack();
                }

            }
            else
            {
                if (shootCount >= numberOfShotsPerSerie)
                {
                    if (elapsedTime > timeBetweenShotsSeries)
                        ResetShootValue();
                }
                else if (elapsedTime > timeBetweenShots)
                {
                    AnimationAttack();
                    shootCount++;
                }
            }
        }
    }

    private void AnimationAttack()
    {
        animator.SetTrigger(ATTACK);
        isAttacking = true;
        elapsedTime = 0;
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleeDistance);
    }


}
