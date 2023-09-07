using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float knockbackForce = 1;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
       
        if(IsInLayerMask(collision.gameObject.layer, targetLayers))
        {
            if (collision.gameObject.GetComponent<Damageable>())
            {
                DealDamage(collision.gameObject.GetComponent<Damageable>());
            }
        }
    }

    public void DealDamage(Damageable damageable)
    {
        if (damageable.invincible)
            return;

        damageable.TakeDamage(damage);

        if (damageable.knockable && knockbackForce>0)
            KnockBack(damageable);
    }

    private void KnockBack(Damageable damageable)
    {
        damageable.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300);
        Vector2 vec = new Vector2((damageable.GetComponentInParent<Transform>().position.x - transform.position.x), 0);
        damageable.gameObject.GetComponent<Rigidbody2D>().AddForce(vec.normalized * knockbackForce * 1000);

    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        // Converte la LayerMask in un intero bit a bit
        int layerMaskValue = layerMask.value;

        // Controlla se il bit corrispondente alla layer dell'oggetto è attivo
        return (layerMaskValue & (1 << layer)) != 0;
    }

}
