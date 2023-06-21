using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    EnemyOne enemy;
    [SerializeField] UnityEvent events;
    private void Start()
    {
        enemy = GetComponentInParent<EnemyOne>();
    }

    public void EndAnimationAttack()
    {
        enemy.EndAnimationAttack();
    }

    public void CallEvent()
    {
        events?.Invoke();
    }

}
