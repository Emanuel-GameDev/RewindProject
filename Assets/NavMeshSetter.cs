using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshSetter : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        NavmeshSetup();
    }

    private void NavmeshSetup()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
}
