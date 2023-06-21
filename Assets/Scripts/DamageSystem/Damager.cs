using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    [SerializeField] LayerMask targetLayers;

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
        damageable.TakeDamage(damage);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        // Converte la LayerMask in un intero bit a bit
        int layerMaskValue = layerMask.value;

        // Controlla se il bit corrispondente alla layer dell'oggetto è attivo
        return (layerMaskValue & (1 << layer)) != 0;
    }

}
