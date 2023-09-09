using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : BaseEnemy
{
    [Header("Specific Tree Data")]
    [Tooltip("Imposta l'angolo di rotazione del campo visivo del nemico")]
    [SerializeField] float viewRotation = 90;
    [Tooltip("Imposta l'ampiezza del campo visivo del nemico")]
    [SerializeField] float fieldOfView = 90;
    [Tooltip("Imposta la distanza del campo visivo del nemico")]
    [SerializeField] float viewDistance = 15;
    [Tooltip("Imposta la velocità di movimento normale del nemico")]
    [SerializeField] float walkSpeed = 2.5f;
    [Tooltip("Imposta la velocità di movimento di corsa del nemico")]
    [SerializeField] float runSpeed = 5;
    [Tooltip("Imposta la distanza a cui si deve fermare dal Target durante l'inseguimento")]
    [SerializeField] float stopDistance = 3;
    [Tooltip("Imposta il tempo che intercorre tra un attacco e il successivo")]
    [SerializeField] float timeBetweenAttacks = 0.5f;
    [Tooltip("Imposta il tempo che rimane fermo dopo aver colpito con un attacco")]
    [SerializeField] float pauseDurationAfterHit = 5f;
    [Tooltip("Imposta i punti della ronda del nemico")]
    [SerializeField] GameObject[] pathPoints;

    [Header("Sounds")]
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip roarSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip attackSound;

    GameObject attack;
    protected NavMeshAgent navMeshAgent;
    private bool hitPause = false;
    private float elapsedTime = 0;
    private MainCharacter_SoundsGenerator audioGenerator;

    //Nomi delle variabili nel behaviour tree
    private const string VIEW_ROTATION = "View Rotation";
    private const string FIELD_OF_VIEW = "Field Of View Angle";
    private const string VIEW_DISTANCE = "View Distance";
    private const string WALK_SPEED = "Walk Speed";
    private const string RUN_SPEED = "Run Speed";
    private const string STOP_DISTANCE = "Stop Distance";
    private const string TIME_BETWEEN_ATTACKS = "Time Between Attacks";
    private const string PATH = "Path";

    //Nomi delle variabili nel Animator
    private const string SPEED = "Speed";
    private const string ATTACK = "Attacking";


    void Start()
    {
        NavmeshSetup();
    }
    private void Update()
    {
        FlipCharacter();
        ManageAnimation();
        HitPauseManager();
    }

    private void HitPauseManager()
    {
        if (hitPause)
        {
            if(elapsedTime > pauseDurationAfterHit)
            {
                hitPause = false;
                elapsedTime = 0;
                tree.SetVariableValue(IS_DEAD, false);
            }
            else
            {
                elapsedTime += Time.deltaTime;
                tree.SetVariableValue(IS_DEAD, true);
            }
        }  
    }

    private void ManageAnimation()
    {
        animator.SetFloat(SPEED, navMeshAgent.velocity.magnitude);
        if (navMeshAgent.velocity.magnitude > 2.6)
        {
            audioGenerator.PlaySound(roarSound);
        }
    }

    private void FlipCharacter()
    {
        // Otteniamo la direzione del movimento
        Vector3 moveDirection = navMeshAgent.desiredVelocity;

        // Se la direzione non è nulla, effettuiamo il flip
        if (moveDirection != Vector3.zero)
        {
            if (moveDirection.x < 0)
            {
                //spriteRenderer.flipX = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (moveDirection.x > 0)
            {
                //spriteRenderer.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    public void Attack()
    {
        attack.SetActive(true);
        animator.SetBool(ATTACK, true);
        tree.SetVariableValue(IS_DEAD, true);
        audioGenerator.PlaySound(hitSound);
    }

    public void EndAttack()
    {
        attack.SetActive(false);
        animator.SetBool(ATTACK, false);
    }

    protected override void InitialSetup()
    {
        base.InitialSetup();
        tree.SetVariableValue(VIEW_ROTATION, viewRotation);
        tree.SetVariableValue(FIELD_OF_VIEW, fieldOfView);
        tree.SetVariableValue(VIEW_DISTANCE, viewDistance);
        tree.SetVariableValue(WALK_SPEED, walkSpeed);
        tree.SetVariableValue(RUN_SPEED, runSpeed);
        tree.SetVariableValue(STOP_DISTANCE, stopDistance);
        tree.SetVariableValue(TIME_BETWEEN_ATTACKS, timeBetweenAttacks);
        tree.SetVariableValue(PATH, pathPoints.ToList<GameObject>());

        attack = GetComponentInChildren<Damager>().gameObject;
        audioGenerator = GetComponentInChildren<MainCharacter_SoundsGenerator>();
        hitPause = false;
        EndAttack();
    }

    private void NavmeshSetup()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    
    public void EndAnimationAttack()
    {
        tree.SetVariableValue(IS_DEAD, false);
    }


    public void SetPauseAfterHit()
    {
        hitPause = true;
        EndAttack();
    }

    public override void OnDie()
    {
        base.OnDie();
        EndAttack();
        hitPause = false;
        audioGenerator.PlaySound(deathSound);
    }

    public override void OnHit()
    {
        base.OnHit();
        audioGenerator.PlaySound(hitSound);
    }

}
