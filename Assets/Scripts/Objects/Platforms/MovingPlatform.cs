using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    [Tooltip("Check this bool if you want the platform to go through every waypoint in both directions")]
    [SerializeField] private bool loopPath = false;

    private List<Transform> waypoints = new List<Transform>();

    private int targetWaypointId;
    private Transform targetWaypoint;
    private Transform prevWaypoint;
    private bool canMove = false;
    private IEnumerator activatorCoroutine;
    private int prevId = -1;

    private float timeToWaypoint;
    private float elapsedTime;

    private void Start()
    {
        if (waypointPath != null)
        {
            InitializeWaypoints();
            TargetNextWaypoint();
        }
    }

    private void InitializeWaypoints()
    {
        foreach (Transform waypointChild in waypointPath)
        {
            waypoints.Add(waypointChild);
        }
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

        Debug.Log(prevId + "  " + currId + "  " + nextId);
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

    private void OnDrawGizmos()
    {
        if (waypointPath == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);
        }


        for (int i = 0;i < waypoints.Count - 1;i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position);
        }
        if (loopPath)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }


    }

    private void FixedUpdate()
    {
        if (waypointPath == null) return;

        if (waitForStand && !canMove) return;

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

    private IEnumerator ActivateMovement(bool mode)
    {
        yield return new WaitForSeconds(triggerOffset);

        canMove = mode;
        activatorCoroutine = null;
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

            activatorCoroutine = ActivateMovement(false);
            StartCoroutine(activatorCoroutine);
        }
    }
}
