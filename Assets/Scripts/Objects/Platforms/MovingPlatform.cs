using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Variables

    [Tooltip("Add reference to this if you want the platform to move following a path")]
    [SerializeField] Transform waypointPath;
    [SerializeField] private float speed;
    [Tooltip("Layers that will be affected by the platform movement\n" +
    "(P.N. this is about parenting with the platform not about triggering movement)")]
    [SerializeField] private LayerMask[] affectedLayers;

    [Tooltip("set to true if platform needs to move only when something is standing on it")]
    [SerializeField] bool waitForStand = false;
    [Tooltip("Time needed for the platform to trigger the movement")]
    [SerializeField] private float triggerOffset;
    [SerializeField] private LayerMask platformTrigger;
    [Tooltip("Platform will stop at path's end when target not standing")]
    [SerializeField] private bool stopAtEnd = false;
    [Tooltip("Platform will stop at the first path's end when target not standing")]
    [SerializeField] private bool stopAtBothEnds = false;

    [Tooltip("Check this bool if you want the platform to go through every waypoint in both directions")]
    [SerializeField] private bool loopPath = false;

    private List<Transform> waypoints = new List<Transform>();

    private int targetWaypointId;
    private Transform targetWaypoint;
    private Transform prevWaypoint;
    private bool movementTriggered = false;
    private bool platformNowJoined = false;
    private IEnumerator activatorCoroutine;
    private int prevId = -1;
    private GameObject objStanding;

    private float timeToWaypoint;
    private float elapsedTime;

    #endregion

    #region UnityFunctions

    private void Start()
    {
        if (waypointPath != null)
        {
            InitializeWaypoints();
            TargetNextWaypoint();
        }
    }

    private void OnValidate()
    {
        if (!waitForStand)
            stopAtEnd = false;

        if (!stopAtEnd)
            stopAtBothEnds = false;
    }

    private void FixedUpdate()
    {
        if (!CheckCondition()) return;

        elapsedTime += Time.deltaTime;

        // Percentage of journey already completed
        float elapsedPercentage = elapsedTime / timeToWaypoint;

        // Update position
        transform.position = Vector2.Lerp(prevWaypoint.position, targetWaypoint.position, elapsedPercentage);

        // Update target waypoint when platform reaches destination
        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }

        // Move obj without parenting
        //if (objStanding != null)
        //{
        //    objStanding.transform.position = transform.position;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check collision for parenting
        foreach (LayerMask mask in affectedLayers)
        {
            if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(mask.value, 2f)))
            {
                RaycastHit2D[] hit;

                // Inverted Gravity
                if (collision.gameObject.GetComponent<Rigidbody2D>().gravityScale < 0)
                    hit = Physics2D.RaycastAll(collision.gameObject.transform.position, Vector2.up);
                // Normal Gravity
                else
                    hit = Physics2D.RaycastAll(collision.gameObject.transform.position, Vector2.down);

                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider != null && hit[i].collider.gameObject.GetComponent<MovingPlatform>())
                    {
                        collision.gameObject.transform.parent.parent = transform;
                        //objStanding = collision.gameObject;
                    }
                }

            }
        }

        // Check collsion for triggering movement
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(platformTrigger.value, 2f)))
        {
            if (activatorCoroutine != null)
                StopCoroutine(activatorCoroutine);

            platformNowJoined = true;

            activatorCoroutine = ActivateMovement(true);
            StartCoroutine(activatorCoroutine);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check collision for parenting
        foreach (LayerMask mask in affectedLayers)
        {
            if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(mask.value, 2f)))
                collision.gameObject.transform.parent.parent = null;
            //objStanding = null;
        }

        // Check collsion for triggering movement
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(platformTrigger.value, 2f)))
        {

        }
    }

    private void OnDrawGizmos()
    {
        if (waypointPath == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypointPath.childCount; i++)
        {
            Gizmos.DrawWireSphere(waypointPath.GetChild(i).position, 0.5f);
        }


        for (int i = 0; i < waypointPath.childCount - 1; i++)
        {
            Gizmos.DrawLine(waypointPath.GetChild(i).position, waypointPath.GetChild(i + 1).position);
        }
        if (loopPath)
        {
            Gizmos.DrawLine(waypointPath.GetChild(waypointPath.transform.childCount - 1).position, waypointPath.GetChild(0).position);
        }
    }

    #endregion

    private void InitializeWaypoints()
    {
        foreach (Transform waypointChild in waypointPath)
        {
            waypoints.Add(waypointChild);
        }
    }

    private bool CheckCondition()
    {
        if (waypointPath == null) return false;

        if (waitForStand && !movementTriggered) return false;

        if (stopAtEnd)
        {
            if (stopAtBothEnds)
            {
                if (transform.position == waypoints[waypoints.Count - 1].position && !platformNowJoined ||
                    transform.position == waypoints[0].position && !platformNowJoined)
                {
                    return false;
                }

            }
            else
            {
                if (transform.position == waypoints[waypoints.Count - 1].position)
                    return false;
            }
        }


        return true;
    }

    private int GetNextWaypointId(int currId)
    {
        int nextId;

        if (loopPath)
        {
            nextId = currId + 1;

            if (nextId == waypoints.Count)
                nextId = 0;
        }
        else
        {
            int direction = currId - prevId;
            nextId = currId + direction;

            if (nextId < 0 || nextId >= waypoints.Count)
            {
                // Invert direction when list borders are reached
                direction *= -1;
                nextId = currId + direction;
            }

        }

        prevId = currId;

        return nextId;
    }

    private void TargetNextWaypoint()
    {
        // Ref to prev waypoint
        prevWaypoint = waypoints[targetWaypointId];
        // Update waypoint id
        targetWaypointId = GetNextWaypointId(targetWaypointId);
        //Ref to next waypoint
        targetWaypoint = waypoints[targetWaypointId];

        elapsedTime = 0;

        float distToWaypoint = Vector2.Distance(prevWaypoint.position, targetWaypoint.position);
        // Calculate time needed to get to the next waypoint
        timeToWaypoint = distToWaypoint / speed;

    }

    private IEnumerator ActivateMovement(bool mode)
    {
        yield return new WaitForSeconds(triggerOffset);

        platformNowJoined = false;
        movementTriggered = mode;
        activatorCoroutine = null;
    }
}
