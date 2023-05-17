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
    [SerializeField] float distanceFromPlatformToJump = 2.5f;
    [SerializeField] float yTollerance = 1f;
    [SerializeField] LayerMask layerMask; // layer degli oggetti da cercare
    [SerializeField] Vector2 raycastOrigins;
    private List<Collider2D> nearColliders = new();
    private bool isJumping = false;
    private Vector3 pointToJump = Vector3.zero;
    private float lerp = 0f; // il valore di interpolazione per muovere l'oggetto lungo la parabola
    private Vector3 startJumpPoint = Vector3.zero;
    private Vector2 centerOfCollider;
    private Vector3 actualTargetPoint;
    private bool canReachTarget = true;
    private Collider2D nearestPlatform;
    private Vector2 raycastOrigin => new Vector2(transform.position.x + raycastOrigins.x, transform.position.y + raycastOrigins.y);

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
            FindPoint();
            isJumping = true;
            startJumpPoint = transform.position;
            lerp = 0f;
        }

        if (isJumping)
        {
            JumpToPoint(pointToJump, paraboleMaxHeight);
        }

        if (target != null && !isJumping)
        {
           FollowTarget();
        } 
    }

    //Gestione Follow
    //=============================================================================================================================
    #region Gestione Follow

    private void FollowTarget()
    {
        if (CanReachTarget())
        {
            actualTargetPoint = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            if (Mathf.Abs(transform.position.x - actualTargetPoint.x) < arriveDistance)
            {
                isJumping = true;
            }
            else
            {
                FindPoint();
                actualTargetPoint = startJumpPoint;
            }

        }

        navMeshAgent.SetDestination(actualTargetPoint);
    }

    private bool CanReachTarget()
    {
        if(MathF.Abs(target.transform.position.y - transform.position.y) > yTollerance)
        {
            canReachTarget = false;
        }
        else
        {
            canReachTarget = true;
        }

        return canReachTarget;
    }

    #endregion


    //Gestione Salto
    //=============================================================================================================================
    #region Gestione Salto
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

    private Collider2D FindClosestCollider()
    {
        Vector3 position = transform.position;
        Vector3 targetPosition = target.transform.position;
        Collider2D closestCollider = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in nearColliders)
        {
            float yDifference = Mathf.Abs(position.y - collider.transform.position.y);
            if (yDifference <= maxJumpHeight)
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

        if(Vector2.Distance(pointToJump,transform.position) < arriveDistance)
        {
            isJumping = false;
        }
    }

    private Vector3 FindPointsForTheJump()
    {

        Vector3 closestPoint = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

        if (nearestPlatform != null)
        {
            Bounds bounds = nearestPlatform.bounds;

            Vector3 topLeft = new Vector3(bounds.min.x, bounds.max.y, 0f);
            Vector3 topRight = bounds.max;

            topLeft.x += centerOfCollider.x;
            topLeft.y += centerOfCollider.y;
            topRight.x -= centerOfCollider.x;
            topRight.y += centerOfCollider.y;

            if ((transform.position - topLeft).sqrMagnitude < (transform.position - topRight).sqrMagnitude)
            {
                closestPoint = topLeft;
                startJumpPoint = new Vector3(topLeft.x - distanceFromPlatformToJump, transform.position.y, transform.position.z);
            }
            else
            {
                closestPoint = topRight;
                startJumpPoint = new Vector3(topRight.x + distanceFromPlatformToJump, transform.position.y, transform.position.z);

            }
        }
           
        return closestPoint;
    }

    #endregion


    //Raicast
    //=============================================================================================================================
    #region Raycast
    private bool FloorPrsence()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, maxJumpHeight, layerMask);

        // Controlla se il raycast ha colpito un oggetto
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    #endregion

    //Varie
    //=============================================================================================================================
    #region Varie
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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastOrigin, (new Vector2(raycastOrigin.x, raycastOrigin.y - maxJumpHeight)));

    }

    private void FindPoint()
    {
        FindNearColliders();
        nearestPlatform = FindClosestCollider();
        pointToJump = FindPointsForTheJump();
    }


    #endregion

}
