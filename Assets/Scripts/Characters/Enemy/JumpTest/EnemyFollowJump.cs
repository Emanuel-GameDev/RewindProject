using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

public class EnemyFollowJump : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float maxJumpHeight = 7f;
    [SerializeField] float searchRadius = 15f; // raggio di ricerca
    [SerializeField] LayerMask layerMask; // layer degli oggetti da cercare
    private List<Collider2D> nearColliders = new List<Collider2D>(); // lista dei NavMeshModifier  trovati



    private void FindNearColliders()
    {
        nearColliders.Clear();
        // Trova tutti gli oggetti all'interno del raggio specificato
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, layerMask);

        // Cicla attraverso tutti gli oggetti trovati e controlla se hanno il componente NavMeshModifier 
        foreach (Collider2D collider in colliders)
        {
            NavMeshModifier modifier = collider.GetComponent<NavMeshModifier>();
            if (modifier != null)
            {
                nearColliders.Add(collider);
            }
        }
    }

    private Collider2D FindClosestCollider(List<Collider2D> colliders, Vector3 position, float maxYDifference)
    {
        Collider2D closestCollider = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            float yDifference = Mathf.Abs(position.y - collider.transform.position.y);
            if (yDifference <= maxYDifference)
            {
                float distance = Vector3.Distance(position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = collider;
                }
            }
        }

        return closestCollider;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FindNearColliders();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxJumpHeight);
    }
}
