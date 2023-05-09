using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using System;
using UnityEngine.AI;

public class EnemyFollowAndJump : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float maxJumpHeight = 7f;
    [SerializeField] float paraboleMaxHeight = 1f;
    [SerializeField] float jumpSpeed = 0.1f; // velocità di movimento lungo la parabola
    [SerializeField] float searchRadius = 15f; // raggio di ricerca
    [SerializeField] float arriveDistance = 0.1f;
    [SerializeField] LayerMask layerMask; // layer degli oggetti da cercare
    private List<Collider2D> nearColliders = new List<Collider2D>();
    private bool isJumping = false;
    private Vector3 pointToJump = Vector3.zero;
    private float lerp = 0f; // il valore di interpolazione per muovere l'oggetto lungo la parabola
    private Vector3 startJumpPoint = Vector3.zero;
    private Vector2 centerOfCollider;
    private Vector3 actualTargetPoint;

    private NavMeshAgent navMeshAgent;

    //Start&Update
    //=============================================================================================================================
    void Start()
    {
        NavmeshSetup();
        centerOfCollider = FindCenterOfColldier();
        actualTargetPoint = target.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isJumping)
        {
            FindNearColliders();
            Collider2D nearestPlatform = FindClosestCollider(nearColliders, transform.position, maxJumpHeight, target.transform.position);
            pointToJump = FindPointWhereToJump(nearestPlatform);
            isJumping = true;
            startJumpPoint = transform.position;
            lerp = 0f;
        }

        if (isJumping)
        {
            JumpToPoint(pointToJump, paraboleMaxHeight);
        }

        if (target != null)
        {
           // FollowTarget();
        } 
    }

    //Gestione Follow
    //=============================================================================================================================
    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, transform.position.x);
        navMeshAgent.SetDestination(targetPosition);
    }



    //Gestione Salto
    //=============================================================================================================================

    private Vector2 FindCenterOfColldier()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Vector2 size = collider.bounds.size;
        float offsetX = size.x / 2f;
        float offsetY = size.y / 2f;
        Vector2 distances = new Vector2(offsetX,offsetY);
        return distances;
    }

    private void FindNearColliders()
    {
        nearColliders.Clear();
        // Trova tutti gli oggetti all'interno del raggio specificato
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, layerMask);

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

    private void JumpToPoint(Vector3 pointToJump, float maxYJump)
    {
        lerp += jumpSpeed * Time.deltaTime;

        if (lerp > 1f)
        {
            lerp = 1f;
        }

        float x = Mathf.Lerp(startJumpPoint.x, pointToJump.x, lerp);
        float y = Mathf.Lerp(startJumpPoint.y, pointToJump.y, lerp) + maxYJump * Mathf.Sin(Mathf.Lerp(0f, Mathf.PI, lerp));

        transform.position = new Vector3(x, y, 0);

        if(Vector2.Distance(pointToJump,transform.position)< arriveDistance)
        {
            isJumping = false;
        }
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


    //Varie
    //=============================================================================================================================
    private void NavmeshSetup()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxJumpHeight);
    }
}
