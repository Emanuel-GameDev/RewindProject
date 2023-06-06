using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidFollower : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speed = 5f;
    [SerializeField] float arriveDistance = 0.1f;
    [SerializeField] float jumpSpeed = 0.1f;
    [SerializeField] float parabolaHeight = 1f;
    [SerializeField] float heightDifferenceTollerance = 1f;


    private Vector3 pointToJump = Vector3.zero;
    private Vector3 startJumpPoint = Vector3.zero;
    private Vector3 actualTargetPoint;

    private Rigidbody2D rb;

    private bool targetIsAbove = false;
    private bool targetIsBelow = false;
    private bool isAtEndPoint = false;
    private bool isJumping = false;

    private float lerp = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        BoolsUpdate();
        TargetUpdate();
        MoveUpdate();
    }

    private void TargetUpdate()
    {
        actualTargetPoint = target.transform.position;

        if (targetIsAbove)
        {

        }
        if(targetIsBelow)
        {

        }
        if (isAtEndPoint)
        {

        }
    }

    private void MoveUpdate()
    {
        if (isJumping)
        {
            JumpToPoint(pointToJump, parabolaHeight);
        }
        else
        {
            MoveTo();
        }
    }

    private void MoveTo()
    {
        Vector2 velocity = new Vector2(0f, rb.velocity.y);

        if (MathF.Abs(actualTargetPoint.x - transform.position.x) > arriveDistance)
        {
            if (actualTargetPoint.x > transform.position.x)
            {
                velocity = new Vector2(speed * Time.deltaTime, 0);
            }
            else
            {
                velocity = new Vector2(-speed * Time.deltaTime, 0);
            }
        }

        rb.velocity = velocity;
    }

    private void BoolsUpdate()
    {
        //Controllo se sta saltando e se è arrivatoa  destinazione
        if (isJumping)
        {
            if (Vector2.Distance(pointToJump, transform.position) < arriveDistance)
            {
                SetIsJumping(false);
            }
        }
        CheckTargetHeight();
    }

    

    private void CheckTargetHeight()
    {
        if (MathF.Abs(target.transform.position.y - transform.position.y) > heightDifferenceTollerance)
        {
            if(target.transform.position.y > transform.position.y)
            {
                targetIsAbove = true;
                targetIsBelow = false;
            }
            else
            {
                targetIsBelow = true;
                targetIsAbove = false;
            }
        }
        else
        {
            targetIsAbove = false;
            targetIsBelow = false;
        }
    }


    //Salto
    //=============================================================================================================================
    #region Jump
    private void SetIsJumping(bool value)
    {
        isJumping = value;
        if (value)
        {
            actualTargetPoint = pointToJump;
            lerp = 0;
            startJumpPoint = transform.position;
        }
    }

    private void JumpToPoint(Vector3 pointToJump, float maxYJump)
    {
        lerp += jumpSpeed * Time.deltaTime;

        if (lerp > 1f)
        {
            lerp = 1f;
        }

        float x = Mathf.Lerp(startJumpPoint.x, pointToJump.x, lerp);
        float y = Mathf.Lerp(startJumpPoint.y, pointToJump.y, lerp) + maxYJump * Mathf.Sin(Mathf.Lerp(0f, Mathf.PI, lerp));

        transform.position = new Vector3(x, y, 0);
    }
    #endregion


}
