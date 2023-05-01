using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : BaseEnemy
{
    Animator animator;
    GameObject attack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attack = GetComponentInChildren<Damager>().gameObject;
        EndAttack();
    }

    public void Attack()
    {
        attack.SetActive(true);
        animator.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        attack.SetActive(false);
    }

    private void Update()
    {
       
    }
}
