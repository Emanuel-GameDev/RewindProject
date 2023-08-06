using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileCause : Cause
{
    [SerializeField] private LayerMask targetProjectileLayer;
    [Tooltip("Use this field in case you want to use a Type insted of layer (Type is not serializable, Reflection will be used)")]
    [SerializeField] private string targetTypeName;

    protected override void ActivateEffect()
    {
        effect?.Invoke();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    private bool CheckCondition(GameObject collided)
    {
        if (targetProjectileLayer.value == 0 && targetTypeName == "")
        {
            Debug.LogError("Error: you need to assign at laest one of the Cause fields");
            return false;
        }

        Type targetType = Type.GetType(targetTypeName);

        if (collided.layer == Mathf.RoundToInt(Mathf.Log(targetProjectileLayer.value, 2f)))
            return true;
        else if (targetType != null)
        {
            Component targetComponent = collided.gameObject.GetComponent(targetType);

            if (targetComponent != null)
                return true;
            else
                return false;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckCondition(collision.gameObject))
        {
            ActivateEffect();
        }
    }
}
