using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuosZRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
