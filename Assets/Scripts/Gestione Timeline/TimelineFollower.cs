using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineFollower : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float positionDamping = 10.0f;
    [SerializeField] float rotationDamping = 10.0f;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        InitialSetup();
    }

    private void InitialSetup()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        target = transform.parent;
        this.transform.parent = null;
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = target.position;
        float targetRotation = target.eulerAngles.z;

        // Interpolate position to target
        Vector2 newPosition = Vector2.Lerp(_rigidbody2D.position, targetPosition, Time.deltaTime * positionDamping);

        // Interpolate rotation to target
        float newRotation = Mathf.LerpAngle(_rigidbody2D.rotation, targetRotation, Time.deltaTime * rotationDamping);

        // Set new position and rotation
        _rigidbody2D.MovePosition(newPosition);
        _rigidbody2D.MoveRotation(newRotation);
    }
}
