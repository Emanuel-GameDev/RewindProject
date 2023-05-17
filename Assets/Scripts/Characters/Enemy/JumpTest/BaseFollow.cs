using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseFollow : MonoBehaviour
{
    public Transform target;  // L'oggetto target che il NavMeshAgent deve seguire
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;


        // Assicurati che il target sia valido
        if (target == null)
        {
            Debug.LogError("Il target non è stato assegnato!");
        }
    }

    private void Update()
    {
        agent.SetDestination(target.position);
    }

}
