using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossRewindableProjectile : BossProjectile
{
    float maxRecordedTime = 30f;
    GameObject target;

    private bool isRewinding = false;
    private bool canBeRewinded = false;
    private List<RewindData> rewindData = new List<RewindData>();
    private int maxDataCount => (int)(maxRecordedTime * (1f / Time.fixedDeltaTime));

    protected override void Update()
    {
        if (!isRewinding)
        {
            base.Update();
            SetDirection();
        }
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

    }
    private void SetDirection()
    {
        if (target != null)
            direction = (target.transform.position - transform.position).normalized;
    }

    public void Inizialize(Vector2 direction, Vector2 position, float speed, GameObject target)
    {
        Inizialize(direction, position, speed);
        this.target = target;
    }

    private void RecordData()
    {
        RewindData data = new RewindData(transform.position, transform.rotation, transform.localScale, spriteRenderer.sprite, spriteRenderer.flipX);

        rewindData.Insert(0, data);

        if (rewindData.Count > maxDataCount)
        {
            rewindData.RemoveAt(rewindData.Count - 1);
        }
    }
    private void ExecuteRewind()
    {
        if (rewindData.Count > 0)
        {
            transform.position = rewindData[0].position;
            transform.rotation = rewindData[0].rotation;
            transform.localScale = rewindData[0].scale;
            spriteRenderer.sprite = rewindData[0].sprite;
            spriteRenderer.flipX = rewindData[0].spriteFlipX;
            rewindData.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }

    }
    private void StopRewind()
    {
        isRewinding = false;
    }
    public void StartRewind()
    {
        if (!isRewinding && canBeRewinded)
        {
            isRewinding = true;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RewindableTriggerAura>())
        {
            canBeRewinded = true;
            if (collision.GetComponent<RewindableTriggerAura>().GetParryIsActive())
            {
                StartRewind();
            }
        }
        if (IsInLayerMask(collision.gameObject.layer, targetLayers))
        {
            if (!collision.isTrigger)
            {
                StopAndExplode();
            }
            else
            {
                if (collision.gameObject.GetComponentInParent<BossBheaviour>() && isRewinding)
                {
                    collision.gameObject.GetComponentInParent<BossBheaviour>().RewindHit(1);
                    StopAndExplode();
                }
            }
        }

    }

    public override void Dismiss()
    {
        PubSub.Instance.Notify(EMessageType.TimeRewindStop, this);
        base.Dismiss();
    }
}
