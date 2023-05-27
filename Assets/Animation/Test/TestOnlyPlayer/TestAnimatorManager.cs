using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorManager : MonoBehaviour
{
    Animator animator;
    PlayerController playerController;
    Rewindable rewindable;
    Rigidbody2D rb;
    SpriteRenderer spriteRederer;

    bool isJumping = false;
    bool isFalling = false;
    bool isMooving = false;
    bool isRunning = false;
    bool isRewinding = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rewindable = GetComponent<Rewindable>();
        rb = GetComponent<Rigidbody2D>();
        spriteRederer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBools();
    }

    private void UpdateBools()
    {
        isJumping = playerController.isJumping;
        isFalling = playerController.isFalling;
        isMooving = playerController.isMooving;
        isRunning = playerController.isRunning;
        isRewinding = rewindable.GetIsRewinding();

        animator.SetBool("Jumping", isJumping);
        animator.SetBool("Falling", isFalling);
        animator.SetBool("Walking", isRunning || isMooving);
        if (isRewinding)
        {
            animator.enabled = false;
        }
        else
        {
            animator.enabled = true;
            if (rb.velocity.x > 0)
            {
                spriteRederer.flipX = false;
            }
            else
            {
                spriteRederer.flipX = true;
            }
        }

    }
}
