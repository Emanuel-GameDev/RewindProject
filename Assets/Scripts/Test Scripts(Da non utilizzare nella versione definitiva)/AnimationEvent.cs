using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    EnemyOne enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyOne>();
    }

    public void EndAnimationAttack()
    {
        enemy.EndAnimationAttack();
    }

}
