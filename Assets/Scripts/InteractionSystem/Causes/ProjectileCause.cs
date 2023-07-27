using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCause : Cause
{
    [SerializeField] private LayerMask targetProjectileLayer;

    protected override void ActivateEffect()
    {
        effect?.Invoke();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetProjectileLayer.value, 2f)))
        {
            ActivateEffect();
        }
    }
}
