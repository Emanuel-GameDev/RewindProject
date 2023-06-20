using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 25f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Transform target;
    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion toRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        Vector2 currentPos = target.position;
        transform.position = Vector2.MoveTowards(transform.position, currentPos, moveSpeed * Time.deltaTime);
    }
}
