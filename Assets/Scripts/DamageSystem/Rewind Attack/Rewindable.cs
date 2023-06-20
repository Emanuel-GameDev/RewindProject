using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewindable : MonoBehaviour
{
    [Range(0.1f, 30f)]
    [SerializeField] float maxRecordedTime = 10f;
    [Range(0.1f, 30f)]
    [SerializeField] float rewindTimeWhenHitted = 5f;
    [Range(0.1f, 30f)]
    [SerializeField] float immunityTime = 5f;

    private float rewindElapsedTime = 0;
    private float immuneElapsedTime = 0;
    private bool isRewinding = false;
    private bool isImmune = false;
    private List<RewindData> rewindData = new List<RewindData>();
    private int maxDataCount => (int)(maxRecordedTime * (1f / Time.fixedDeltaTime));
    private Rigidbody2D rb;
    public PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (rewindTimeWhenHitted > maxRecordedTime)
            rewindTimeWhenHitted = maxRecordedTime;

        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();  
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            ExecuteRewind();
        }
        else
        {
            RecordData();
        }

        if (isImmune)
        {
            immuneElapsedTime += Time.deltaTime;

            if(immuneElapsedTime > immunityTime)
            {
                DisableImmunity();
            }
        }
        
    }

    private void RecordData()
    {
        RewindData data = new RewindData(transform.position, transform.rotation, transform.localScale, spriteRenderer.sprite, spriteRenderer.flipX);

        rewindData.Insert(0, data);

        if(rewindData.Count > maxDataCount)
        {
            rewindData.RemoveAt(rewindData.Count - 1);
        }
    }

    private void ExecuteRewind()
    {
        if (rewindData.Count > 0 && rewindElapsedTime < rewindTimeWhenHitted)
        {
            transform.position = rewindData[0].position;
            transform.rotation = rewindData[0].rotation;
            transform.localScale = rewindData[0].scale;
            spriteRenderer.sprite = rewindData[0].sprite;
            spriteRenderer.flipX = rewindData[0].spriteFlipX;
            rewindData.RemoveAt(0);
            rewindElapsedTime += Time.deltaTime;
        }
        else
        {
            StopRewind();
        }
        
    }

    internal void StartRewind()
    {
        if (!isRewinding && !isImmune)
        {
            isRewinding = true;
            rewindElapsedTime = 0;
            rb.isKinematic = true;
            if(playerController != null )
            {
                playerController.inputs.Disable();
            }
        }
    }

    private void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        EnableImmunity();
        if (playerController != null)
        {
            playerController.inputs.Enable();
        }
    }

    private void EnableImmunity()
    {
        isImmune = true;
        immuneElapsedTime = 0;
    }

    private void DisableImmunity()
    {
        isImmune = false;
    }

    public bool GetIsRewinding()
    {
        bool value = isRewinding;

        return value;
    }

}
