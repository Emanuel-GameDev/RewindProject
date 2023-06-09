using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using System;
using UnityEngine.AI;

public class EnemyFollowAndJump : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float maxJumpDistance = 7f;
    [SerializeField] float parabolaHeight = 1f;
    [SerializeField] float jumpSpeed = 0.1f; // velocit� di movimento lungo la parabola
    [SerializeField] float searchRadius = 15f; // raggio di ricerca
    [SerializeField] float arriveDistance = 0.1f;
    [SerializeField] float distanceFromPlatformToJump = 2.5f;
    [SerializeField] float heightDifferenceTollerance = 1f;
    [SerializeField] LayerMask layerMask; // layer degli oggetti da cercare
    [SerializeField] Vector2 raycastOrigins;

    private NavMeshAgent navMeshAgent;
    private List<Collider2D> nearColliders = new();
    private Collider2D nearestPlatform;
    private float lerp = 0f; // il valore di interpolazione per muovere l'oggetto lungo la parabola
    private Vector3 pointToJump = Vector3.zero;
    private Vector3 startJumpPoint = Vector3.zero;
    private Vector3 actualTargetPoint;
    private Vector2 centerOfCollider;
    private Vector2 raycastOrigin => FlipRaycast();
    private float raycastDistance => maxJumpDistance - raycastOrigins.x*2f;
    private float overlapSize = 0.25f;

    private bool isSameHeight = true;
    private bool thereIsFloor = true;
    private bool isJumping = false;
   
          
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
        BoolsUpdate();
        MoveUpdate();
    }


    //Gestione Follow
    //=============================================================================================================================
    #region Gestione Follow


    private void MoveUpdate()
    {
        if (isJumping)
        {
            JumpToPoint(pointToJump, parabolaHeight);
        }
        else
        {
            actualTargetPoint = target.transform.position;
            
            if (!isSameHeight)
            {
                if (Mathf.Abs(transform.position.x - startJumpPoint.x) < arriveDistance)
                {
                    SetIsJumping(true);
                }
                else
                {
                    FindPoint();
                    actualTargetPoint = startJumpPoint;
                }
            }

            if (!thereIsFloor)
            {
                FindPoint();
                if (Vector2.Distance(pointToJump, transform.position) < maxJumpDistance)
                {
                    SetIsJumping(true);
                }
                else
                {
                    actualTargetPoint = transform.position;
                }
            }

            navMeshAgent.SetDestination(actualTargetPoint);
        }

    }

    private void SetIsJumping(bool value)
    {
        isJumping = value;
        if (value)
        {
            actualTargetPoint = pointToJump;
            lerp = 0;
            startJumpPoint = transform.position;
        }
    }

    private void BoolsUpdate()
    {
        //Controllo se sta saltando e se � arrivatoa  destinazione
        if (isJumping)
        {
            if (Vector2.Distance(pointToJump, transform.position) < arriveDistance)
            {
                SetIsJumping(false);
            }
        }

        //Controllo dell'altezza del Target
        isSameHeight = CheckTargetHeight();

        //Controllo del Pavimento
        thereIsFloor = FloorPresence();
    }

    private bool CheckTargetHeight()
    {
        if (MathF.Abs(target.transform.position.y - transform.position.y) > heightDifferenceTollerance)
        {
            return false;
        }
        else
        {
            return true;
        }
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

            if(nearestPlatform != null)
            {
                if(collider == FrontOverlap())
                {
                    continue;
                }
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
            float heightDifference = Mathf.Abs(position.y - collider.transform.position.y);
            if (heightDifference <= maxJumpDistance)
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

        //Vector2 position = transform.position;
        //Vector2 targetPosition = target.transform.position;
        //Collider2D closestCollider = null;
        //float closestDistance = float.MaxValue;

        //foreach (Collider2D collider in nearColliders)
        //{
        //    Vector2 directionToCollider = new Vector2(collider.transform.position.x - position.x, collider.transform.position.y - position.y);
        //    float dotProduct = Vector2.Dot(directionToCollider, targetPosition - position);
        //    if (dotProduct > 0)
        //    {
        //        float distance = position.x - collider.transform.position.x;
        //        if (distance <= maxJumpDistance && distance < closestDistance)
        //        {
        //           closestDistance = distance;
        //           closestCollider = collider;
        //        }
        //    }

        //}

        //return closestCollider;
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
    }

    private Vector3 FindPointsForTheJump()
    {

        Vector3 closestPoint = target.transform.position; //new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

        if (nearestPlatform != null)
        {
            Bounds bounds = nearestPlatform.bounds;

            Vector3 topLeft = new Vector3(bounds.min.x, bounds.max.y, 0f);
            Vector3 topRight = bounds.max;

            topLeft.x += centerOfCollider.x;
            topRight.x -= centerOfCollider.x;

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

        RaycastHit2D hit = Physics2D.Raycast(closestPoint, Vector2.down, layerMask);
        closestPoint = hit.point + new Vector2( 0, centerOfCollider.y);


        return closestPoint;
    }

    #endregion


    //Raicast
    //=============================================================================================================================
    #region Raycast
    private bool FloorPresence()
    {
        Collider2D circleHit = FrontOverlap();
        if (circleHit != null)
        {
            return true;
        }

        if (target.transform.position.x < transform.position.x)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, layerMask);

            // Controlla se il raycast ha colpito un oggetto
            if (hit.collider != null)
            {
                return true;
            }

        }

        return false;
    }

    private Collider2D FrontOverlap()
    {
        return Physics2D.OverlapCircle(raycastOrigin, overlapSize, layerMask);
    }

    private Vector2 FlipRaycast()
    {
        Vector2 raycastDirection;
        // Otteniamo la direzione del movimento
        Vector3 moveDirection = Vector3.right;
        if (navMeshAgent != null)
            moveDirection = navMeshAgent.desiredVelocity;

        if (moveDirection.x > 0)
        {
            raycastDirection = new Vector2(transform.position.x + raycastOrigins.x, transform.position.y + raycastOrigins.y);
        }
        else
        {
            raycastDirection = new Vector2(transform.position.x - raycastOrigins.x, transform.position.y + raycastOrigins.y);
        }
        

        return raycastDirection;
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
        Gizmos.DrawWireSphere(transform.position, maxJumpDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(raycastOrigin, overlapSize);
        Gizmos.DrawLine(raycastOrigin, (new Vector2(raycastOrigin.x, raycastOrigin.y - raycastDistance)));
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(startJumpPoint, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointToJump, 0.6f);


    }

    private void FindPoint()
    {
        FindNearColliders();
        nearestPlatform = FindClosestCollider();
        pointToJump = FindPointsForTheJump();
    }



    #endregion

}
