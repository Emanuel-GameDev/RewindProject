using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Add reference to this if you want the platform to move following a path")]
    [SerializeField] Transform waypointPath;
    [SerializeField] private float speed;

    [Tooltip("set to true if platform needs to move only when something is standing on it")]
    [SerializeField] bool waitForStand = false;
    [Tooltip("Time needed for the platform to trigger the movement")]
    [SerializeField] private float triggerOffset;
    [SerializeField] private LayerMask platformTrigger;
    [SerializeField] private bool stopAtEnd = false;
    [SerializeField] private bool stopAtBothEnds = false;

    [Tooltip("Check this bool if you want the platform to go through every waypoint in both directions")]
    [SerializeField] private bool loopPath = false;

    private List<Transform> waypoints = new List<Transform>();

    private int targetWaypointId;
    private Transform targetWaypoint;
    private Transform prevWaypoint;
    private bool canMove = false;
    private bool platformLeft = false;
    private IEnumerator activatorCoroutine;
    private int prevId = -1;

    private float timeToWaypoint;
    private float elapsedTime;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            Character character = collision.gameObject.GetComponent<Character>();

            character.gameObject.transform.parent = transform;
        }
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(platformTrigger.value, 2f)))
        {
            if (activatorCoroutine != null)
                StopCoroutine(activatorCoroutine);

            activatorCoroutine = ActivateMovement(true);
            StartCoroutine(activatorCoroutine);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            Character character = collision.gameObject.GetComponent<Character>();

            character.gameObject.transform.parent = null;
        }
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(platformTrigger.value, 2f)))
        {
            if (activatorCoroutine != null)
                StopCoroutine(activatorCoroutine);

            platformLeft = true;

        }
    }

    private void OnDrawGizmos()
    {
        if (waypointPath == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);
        }


        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        if (loopPath)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
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

        if (waitForStand && !canMove) return false;

        if (stopAtEnd && platformLeft)
        {
            if (stopAtBothEnds)
            {
                if (transform.position == waypoints[waypoints.Count - 1].position || transform.position == waypoints[0].position)
                    return false;

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
                // Inverte la direzione quando raggiunge i bordi della lista
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

        platformLeft = false;
        canMove = mode;
        activatorCoroutine = null;
    }
}
