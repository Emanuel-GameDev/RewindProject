using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;   

public class AbilityManager : MonoBehaviour
{
    private List<Ability> _abilities = new List<Ability>();
    
    [SerializeField] private List<AbilityHolder> _holders = new List<AbilityHolder>();
    [SerializeField] private AbilityWheel _wheel;

    private void Awake()
    {
        PubSub.Instance.RegisterFunction(EMessageType.AbilityPicked, AddToAbilities);
        PubSub.Instance.RegisterFunction(EMessageType.ActiveAbilityChanged, GiveAbility);
    }

    private void AddToAbilities(object obj)
    {
        if (obj is not Ability) return;
        Ability newAbility = (Ability)obj;

        _abilities.Add(newAbility);

        _wheel.AddToWheel(newAbility);
    }

    public void GiveAbility(object obj)
    {
        if (obj is not Image) return;

        Image abilityIcon = (Image)obj;
        Ability newActiveAbility = GetAbilityFrom(abilityIcon);

        foreach (AbilityHolder holder in _holders)
        {
            holder.activeAbility = newActiveAbility;
        }
    }

    private Ability GetAbilityFrom(Image abilityIcon)
    {
        return _abilities.Where(ability => ability.icon == abilityIcon.sprite)?.First();
    }
}
