using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform waypointPath;
    [SerializeField] private float speed;

    private List<Transform> waypoints = new List<Transform>();

    private int targetWaypointId;
    private Transform targetWaypoint;
    private Transform prevWaypoint;

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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            Character character = collision.gameObject.GetComponent<Character>();

            character.gameObject.transform.parent = null;
        }
    }
}
