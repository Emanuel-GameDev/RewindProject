using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Cause : MonoBehaviour
{
    [SerializeField] protected UnityEvent onStart;
    [SerializeField] protected UnityEvent effect;

    protected abstract void ActivateEffect();

    protected virtual void Start()
    {
        onStart?.Invoke();
    }

    protected virtual void OnValidate() { }

}
