using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeWatcher : MonoBehaviour
{
    [SerializeField] Transform movingTarget;
    [SerializeField] Transform watchedTarget;
    [SerializeField] float maxDistanceFromCenter = 1;
    [SerializeField] float referenceDistance = 10;
    [SerializeField] Color moveDistanceColor = Color.white;
    [SerializeField] Color watchDistanceColor = Color.white;
    private void Update()
    {
        MoveTarget();
    }

    private void MoveTarget()
    {
        float targetDistanceFromCenter = Comparison() * maxDistanceFromCenter;
        Vector3 directionToWatchedTarget = watchedTarget.position - transform.position;
        Vector3 newPosition = transform.position + directionToWatchedTarget.normalized * targetDistanceFromCenter;
        movingTarget.position = newPosition;
    }

    private float Comparison()
    {
        float distance = Vector2.Distance(watchedTarget.position, transform.position);
        return Mathf.InverseLerp(0, referenceDistance, distance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = moveDistanceColor;
        Gizmos.DrawWireSphere(transform.position, maxDistanceFromCenter);
        Gizmos.color = watchDistanceColor;
        Gizmos.DrawWireSphere(transform.position, referenceDistance);

    }

}
