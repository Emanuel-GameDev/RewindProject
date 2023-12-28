using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CollisionModifier : MonoBehaviour
{
    [SerializeField] private LayerMask[] ignoreCollision;

    [Tooltip("When enabled, the targets are going to set this object's rigidbody to static OnCollisionEnter2D")]
    [SerializeField] private bool useStaticCollision = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useStaticCollision)
        {
            foreach (LayerMask mask in ignoreCollision)
            {
                // Mathf.RoundToInt per arrotondare i numeri float
                // Mathf.Log(x, 2f) logaritmo base 2 
                if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(mask.value, 2f)))
                {
                    Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
                    if (rigidbody2D != null)
                    {
                        rigidbody2D.bodyType = RigidbodyType2D.Static;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (useStaticCollision)
        {
            foreach (LayerMask mask in ignoreCollision)
            {
                // Mathf.RoundToInt per arrotondare i numeri float
                // Mathf.Log(x, 2f) logaritmo base 2 
                if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(mask.value, 2f)))
                {
                    Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
                    if (rigidbody2D != null)
                    {
                        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    }
                }
            }
        }
    }
}
