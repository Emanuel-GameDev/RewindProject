using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] internal  float walkSpeed;

    [Header("JUMP")]
    [SerializeField] internal float jumpForce;

    [Header("GROUND")]
    [SerializeField] internal LayerMask groundMask;
    [SerializeField] internal Transform groundCheck;
    [SerializeField] float groundCheckRadius = 1;

    internal Rigidbody2D rBody;

    internal bool grounded = false;
    internal float horizontalMovement = 0;


    public virtual void FixedUpdate()
    {
        GroundCheck();
    }

    public virtual void CheckJump()
    {
        if (grounded)
            rBody.AddForce(Vector2.up * jumpForce * rBody.gravityScale);
    }

    public virtual void GroundCheck()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundMask))
            grounded = true;
        else
            grounded = false;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.3f);
    }
}
