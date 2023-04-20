using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;

    Rigidbody2D rBody;

    bool grounded = false;
    public float horizontalMovement = 0;


    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rBody.velocity = new Vector2(horizontalMovement * speed, rBody.velocity.y);
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    internal void Jump()
    {
        if (grounded)
            rBody.AddForce(Vector2.up * jumpForce * rBody.gravityScale);
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundMask))
            grounded = true;
        else
            grounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.3f);
    }
}
