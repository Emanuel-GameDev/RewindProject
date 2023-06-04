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

    //private Rigidbody2D rb;
    private CharacterController controller;


    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveTo();
    }

    private void MoveTo()
    {
        //Vector2 velocity = new Vector2(0f, rb.velocity.y);
        Vector3 direction = Vector3.zero;

        if (MathF.Abs(target.transform.position.x - transform.position.x) > 0.5)
        {
            if (target.transform.position.x > transform.position.x)
            {
                //velocity = new Vector2(speed * Time.deltaTime, velocity.y);
                direction = new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else
            {
                //velocity = new Vector2(-speed * Time.deltaTime, velocity.y);
                direction = new Vector3(-speed * Time.deltaTime, 0, 0);
            }
        }

        //rb.velocity = velocity;
        controller.Move(direction);
    }
}
