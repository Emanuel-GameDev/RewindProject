using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private NavMeshAgent navMeshAgent;
    private BehaviorTree tree;
    [SerializeField] float viewRotation = 90;
    [SerializeField] string viewRotationVariableName = "View Rotation";

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        tree = GetComponentInChildren<BehaviorTree>();
    }

    private void Update()
    {
        FlipCharacter();
        FlipView();
    }

    private void FlipView()
    {
        float value = viewRotation;
        if(spriteRenderer.flipX)
            value = -value;

        tree.SetVariableValue(viewRotationVariableName, value);
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
                spriteRenderer.flipX = true;
            }
            else if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
