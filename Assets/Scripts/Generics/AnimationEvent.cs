using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] UnityEvent event1;
    [SerializeField] UnityEvent event2;
    [SerializeField] UnityEvent event3;
    public void CallEvent1()
    {
        event1?.Invoke();
    }

    public void CallEvent2()
    {
        event2?.Invoke();
    }

    public void CallEvent3()
    {
        event3?.Invoke();
    }
}
