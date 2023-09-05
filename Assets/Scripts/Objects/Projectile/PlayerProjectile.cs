using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] LayerMask hittable;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, hittable))
        {
            Dismiss();
        }
    }

    

    public override void Dismiss()
    {
        Destroy(gameObject);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        // Converte la LayerMask in un intero bit a bit
        int layerMaskValue = layerMask.value;

        // Controlla se il bit corrispondente alla layer dell'oggetto è attivo
        return (layerMaskValue & (1 << layer)) != 0;
    }
}
