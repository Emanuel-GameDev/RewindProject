using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionsList : Dictionary<EMessageType, List<Action<object>>> { }

public class PubSub : MonoBehaviour
{
    private static PubSub _instance;
    public static PubSub Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            GameObject pubsubObject = new GameObject("# PubSub");

            _instance = pubsubObject.AddComponent<PubSub>();

            return _instance;
        }
    }

    private FunctionsList _registeredFunctions = new();

    public void RegisterFunction(EMessageType messageType, Action<object> function)
    {
        if (_registeredFunctions.ContainsKey(messageType))
        {
            _registeredFunctions[messageType].Add(function);
        }
        else
        {
            List<Action<object>> newList = new();
            newList.Add(function);

            _registeredFunctions.Add(messageType, newList);
        }
    }

    public void Notify(EMessageType messageType, object messageContent)
    {
        foreach (Action<object> function in _registeredFunctions[messageType])
        {
            function.Invoke(messageContent);
        }
    }

}
