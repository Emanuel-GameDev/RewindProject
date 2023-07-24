using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] LayerMask hittable;


    public override void OnTriggerEnter(Collider other)
    {
            Debug.Log("E");
        if(IsInLayerMask(other.gameObject.layer, hittable))
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

        // Controlla se il bit corrispondente alla layer dell'oggetto � attivo
        return (layerMaskValue & (1 << layer)) != 0;
    }
}
