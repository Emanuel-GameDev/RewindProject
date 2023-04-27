using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [SerializeField] internal  float speed;
    [SerializeField] internal float jumpForce;

    [SerializeField] internal LayerMask groundMask;
    [SerializeField] internal Transform groundCheck;

    internal Rigidbody2D rBody;

    internal bool grounded = false;
    internal float horizontalMovement = 0;


    public virtual void FixedUpdate()
    {
        GroundCheck();
    }

    public virtual void Jump()
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
