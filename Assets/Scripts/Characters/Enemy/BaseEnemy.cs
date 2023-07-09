using BehaviorDesigner.Runtime;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]
public class BaseEnemy : Character
{
    [Header("Enemy Data")]
    [Tooltip("Imposta i punti vita del nemico")]
    [SerializeField] int healtPoint = 2;

    [Header("General Tree Data")]
    [Tooltip("Imposta il bersaglio del nemico")]
    [SerializeField] protected GameObject target;

    protected BehaviorTree tree;
    protected Damageable damageable;
    protected Animator animator;
    protected Collider2D coll;
    protected Vector2 startPosition;

    //Nomi delle variabili nel behaviour tree
    private const string TARGET = "Target";
    protected const string IS_DEAD = "Is Dead";

    //Nomi delle variabili nel Animator
    private const string DEAD = "Dead";
    private const string HITTED = "Hitted";

    protected bool isDead = false;

    protected virtual void Awake()
    {
        InitialSetup();
    }

    protected virtual void InitialSetup()
    {
        tree = GetComponentInChildren<BehaviorTree>();
        damageable = GetComponentInChildren<Damageable>();
        coll = GetComponent<Collider2D>();
        startPosition = transform.position;
        if (damageable != null) damageable.maxHealth = healtPoint;

        if (target != null) tree.SetVariableValue(TARGET, target);

        tree.SetVariableValue(IS_DEAD, isDead);

        
        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();
        else 
            animator = GetComponentInChildren<Animator>();
    }

    public BehaviorTree GetTree()
    {
        return tree;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        tree.SetVariableValue(TARGET, target);
    }

    public virtual void OnDie() 
    {
        isDead = true;
        animator.SetTrigger(DEAD);
        tree.SetVariableValue(IS_DEAD, isDead);
        coll.enabled = false;
    }

    public virtual void ResetEnemy() 
    { 
        transform.position = startPosition;
        isDead = false;
        coll.enabled = true;
    }

    public virtual void OnHit()
    {
        animator.SetTrigger(HITTED);
    }

}
