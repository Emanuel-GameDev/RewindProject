using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerInstance : MonoBehaviour
{
    [SerializeField] int healValue = 1;
    
    public void HealPlayer()
    {
        PlayerController.instance.gameObject.GetComponentInChildren<Damageable>().Heal(healValue);
    }
}
