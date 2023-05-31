using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BehaviorTree))]
public class BaseEnemy : MonoBehaviour
{
    [Header("Enemy Data")]
    [Tooltip("Imposta i punti vita del nemico")]
    [SerializeField] int healtPoint = 2;

    [Header("General Tree Data")]
    [Tooltip("Imposta il bersaglio del nemico")]
    [SerializeField] protected GameObject target;

    protected BehaviorTree tree;
    protected Damageable damageable;

    private const string TARGET = "Target";

    protected virtual void Awake()
    {
        InitialSetup();
    }

    protected virtual void InitialSetup()
    {
        tree = GetComponentInChildren<BehaviorTree>();
        damageable = GetComponentInChildren<Damageable>();
        if (damageable != null) damageable.maxHealth = healtPoint;

        if (target != null) tree.SetVariableValue(TARGET, target);
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
}
