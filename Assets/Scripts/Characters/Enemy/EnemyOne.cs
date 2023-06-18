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

    
    GameObject attack;
    protected NavMeshAgent navMeshAgent;

    //Nomi delle variabili nel behaviour tree
    private const string VIEW_ROTATION = "View Rotation";
    private const string FIELD_OF_VIEW = "Field Of View Angle";
    private const string VIEW_DISTANCE = "View Distance";
    private const string WALK_SPEED = "Walk Speed";
    private const string RUN_SPEED = "Run Speed";
    private const string STOP_DISTANCE = "Stop Distance";
    private const string TIME_BETWEEN_ATTACKS = "Time Between Attacks";

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
    }

    private void ManageAnimation()
    {
        animator.SetFloat(SPEED, navMeshAgent.velocity.magnitude);
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

        attack = GetComponentInChildren<Damager>().gameObject;
        EndAttack();
    }

    private void NavmeshSetup()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    
}
