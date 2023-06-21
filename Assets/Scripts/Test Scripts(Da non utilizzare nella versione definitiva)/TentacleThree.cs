using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleThree : MonoBehaviour
{
    [Header("General Data")]
    [SerializeField] int length;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Vector3[] segmentPoses;
    private Vector3[] segmentV;

    [Header("Tentacle Data")]
    [SerializeField] Transform targetDir;
    [SerializeField] float targetDist;
    [SerializeField] float smoothSpeed;

    [Header("Wiggle Data")]
    [SerializeField] Transform wiggleDir;
    [SerializeField] float wiggleSpeed;
    [SerializeField] float wiggleMagnitude;

    [Header("Body Data")]
    [SerializeField] Transform tailEnd;
    [SerializeField] Transform[] bodyParts;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        ResetPosition();
    }

    private void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;
            Vector3 targetPosition = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            segmentPoses[i] = new Vector3(targetPosition.x, targetPosition.y, segmentPoses[i].z);
            if (bodyParts.Count<Transform>() >= i )
                bodyParts[i - 1].transform.position = segmentPoses[i];
        }
        lineRenderer.SetPositions(segmentPoses);

        if (tailEnd != null)
            tailEnd.position = segmentPoses[segmentPoses.Length - 1];
    }

    private void ResetPosition()
    {
        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < length; i++)
        {
            segmentPoses[i] = segmentPoses[i - 1] + targetDir.right * targetDist;
        }
        lineRenderer.SetPositions(segmentPoses);
    }

}
