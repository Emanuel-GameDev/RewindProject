using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using NavMeshPlus.Components;

public class FindNearestPlatform : Action
{
    public SharedFloat searchRadius;
    public SharedLayerMask layerMask;
    public SharedGameObject target;
    public SharedFloat maxDistance;

    [SharedRequired]
    public SharedVector3 storeJumpPoint;

    private List<Collider2D> nearColliders = new List<Collider2D>();
    private Vector2 centerOfCollider;
    private Collider2D nearestCollider;


    public override TaskStatus OnUpdate()
	{
        centerOfCollider = FindCenterOfColldier();
        FindNearColliders();
        nearestCollider = FindClosestCollider(nearColliders, transform.position, maxDistance.Value, target.Value.transform.position);
        storeJumpPoint.Value = FindPointWhereToJump(nearestCollider);

        return TaskStatus.Success;
	}


    public override void OnReset()
    {
		nearColliders = new List<Collider2D>();
	}

    private void FindNearColliders()
    {
        nearColliders.Clear();
        // Trova tutti gli oggetti all'interno del raggio specificato
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius.Value, layerMask.Value);

        // Ottenere il collider dell'oggetto che chiama la funzione
        Collider2D objectCollider = GetComponent<Collider2D>();

        // Cicla attraverso tutti gli oggetti trovati e controlla se hanno il componente NavMeshModifier 
        foreach (Collider2D collider in colliders)
        {
            if (collider.IsTouching(objectCollider))
            {
                continue; // Se sono in contatto, passa al prossimo collider
            }

            NavMeshModifier modifier = collider.GetComponent<NavMeshModifier>();
            if (modifier != null)
            {
                nearColliders.Add(collider);
            }
        }
    }

    private Collider2D FindClosestCollider(List<Collider2D> colliders, Vector3 position, float maxYDifference, Vector3 targetPosition)
    {
        Collider2D closestCollider = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            float yDifference = Mathf.Abs(position.y - collider.transform.position.y);
            if (yDifference <= maxYDifference)
            {
                Vector3 directionToCollider = collider.transform.position - position;
                float dotProduct = Vector3.Dot(directionToCollider, targetPosition - position);
                if (dotProduct > 0)
                {
                    float distance = Vector3.Distance(position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestCollider = collider;
                    }
                }
            }
        }

        return closestCollider;
    }

    private Vector3 FindPointWhereToJump(Collider2D nearestPlatform)
    {
        Bounds bounds = nearestPlatform.bounds;

        Vector3 topLeft = new Vector3(bounds.min.x, bounds.max.y, 0f);
        Vector3 topRight = bounds.max;

        topLeft.x += centerOfCollider.x;
        topLeft.y += centerOfCollider.y;
        topRight.x -= centerOfCollider.x;
        topRight.y += centerOfCollider.y;

        Vector3 closestPoint = (transform.position - topLeft).sqrMagnitude < (transform.position - topRight).sqrMagnitude ? topLeft : topRight;

        return closestPoint;
    }

    private Vector2 FindCenterOfColldier()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Vector2 size = collider.bounds.size;
        float offsetX = size.x / 2f;
        float offsetY = size.y / 2f;
        Vector2 distances = new Vector2(offsetX, offsetY);
        return distances;
    }

}