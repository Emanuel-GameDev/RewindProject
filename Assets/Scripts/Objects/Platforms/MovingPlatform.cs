using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovingPlatform : MonoBehaviour
{
    [Header("GENERAL")]
    [Tooltip("Add reference to this if you want the platform to move following a path")]
    [SerializeField] Transform waypointPath;
    [SerializeField] private float speed;

    [Header("WAIT FOR STAND DATA")]
    [Tooltip("set to true if platform needs to move only when something is standing on it")]
    [SerializeField] private bool waitForStand = false;
    [Tooltip("Time needed for the platform to trigger the movement")]
    [SerializeField] private float triggerOffset;
    [SerializeField] private LayerMask platformTrigger;

    private List<Transform> waypoints = new List<Transform>();

    private int targetWaypointId;
    private Transform targetWaypoint;
    private Transform prevWaypoint;
    private bool canMove = false;
    private IEnumerator activatorCoroutine;

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
        int nextId = currId + 1;
        
        if (nextId == waypoints.Count)
            nextId = 0;

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
