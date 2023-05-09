using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>())
        {
            DealDamage(collision.gameObject.GetComponent<Damageable>());
        }
    }

    public void DealDamage(Damageable damageable)
    {
        damageable.ChangeHealth(-damage);
    }
    
}
