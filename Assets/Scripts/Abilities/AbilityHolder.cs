using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : Character
{
    public Ability activeAbility;

    private EAbilityState state = EAbilityState.ready; 

    [SerializeField] KeyCode interactKey;

    private void Update()
    {
        switch (state)
        {
            case EAbilityState.ready:
                if (Input.GetKeyDown(interactKey))
                {
                    activeAbility.Activate(gameObject);
                    state = EAbilityState.active;
                }
                break;
            case EAbilityState.active:
                if ( activeAbility.activeTime > 0 )
                {
                    activeAbility.activeTime -= Time.deltaTime;
                }
                else
                {
                    state = EAbilityState.cooldown;
                }
                break;
            case EAbilityState.cooldown:
                if (activeAbility.cooldownTime > 0)
                {
                    activeAbility.cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = EAbilityState.ready;
                }
                break;
        }
    }
}
