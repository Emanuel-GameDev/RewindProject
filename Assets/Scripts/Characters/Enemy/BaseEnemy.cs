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

    protected BehaviorTree tree;
    protected Damageable damageable;

    public virtual void Awake()
    {
        tree = GetComponentInChildren<BehaviorTree>();
        damageable = GetComponentInChildren<Damageable>();
        if (damageable != null)
            damageable.maxHealth = healtPoint;
    }
    
    public BehaviorTree GetTree()
    {
        return tree;
    }

}
