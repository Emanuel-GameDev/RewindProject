using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubSub : MonoBehaviour
{
    private static PubSub _instance;

    public static PubSub Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject pubsubObject = new GameObject("# PubSub");
                _instance = pubsubObject.AddComponent<PubSub>();
            }
            return _instance;
        }
    }

    private Dictionary<eMessageType, List<Action<object>>> _registeredFunction = new();

    public void SendMessages(eMessageType messageType, object messageContent)
    {
        foreach (Action<object> function in _registeredFunction[messageType])
        {
            function.Invoke(messageContent);
        }
    }


    public void RegisterFuncion(eMessageType messageType, Action<object> fuction)
    {
        if (_registeredFunction.ContainsKey(messageType))
        {
            _registeredFunction[messageType].Add(fuction);
        }
        else
        {
            List<Action<object>> newList = new();
            newList.Add(fuction);

            _registeredFunction.Add(messageType, newList);
        }
    }

}
