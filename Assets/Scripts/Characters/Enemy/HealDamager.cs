using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Damageable))]
public class HealDamager : MonoBehaviour
{
    [SerializeField] int healValue = 1;
    Damageable damageable;
    private void Start()
    {
         damageable = GetComponent<Damageable>();
    }


    public void HealTarget()
    {
        Damageable targetDamageable = damageable.GetLastDamager().GetComponent<Damageable>();
        
        if(targetDamageable == null)
        {
            targetDamageable = damageable.GetLastDamager().GetComponentInParent<Damageable>();
        }
        
        if (targetDamageable == null)
        {
            targetDamageable = damageable.GetLastDamager().GetComponentInChildren<Damageable>();
        }
        
        if (targetDamageable != null)
        {
            targetDamageable.Heal(healValue);
        }

    }
}
