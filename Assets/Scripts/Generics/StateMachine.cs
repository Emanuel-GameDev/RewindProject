using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State
{
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}


public class StateMachine<T> where T : Enum
{
    Dictionary<T, State> _states = new();
    State currentState;

    public void RegisterState(T type, State state)
    {
        if (_states.ContainsKey(type))
            return;

        _states.Add(type, state);
    }

    public void StateUpdate()
    {
        currentState?.Update();
    }

    public void SetState(T type)
    {
        currentState?.Exit();

        currentState = _states[type];

        currentState.Enter();
    }
}
