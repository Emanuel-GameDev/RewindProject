using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestPickable : MonoBehaviour
{
    public UnityEvent Do;

    public Ability ab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Do.Invoke();
        //PubSub.Instance.Notify(EMessageType.AbilityPicked, ab);
    }
}
