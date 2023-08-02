using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.UI;   

public class AbilityManager : MonoBehaviour
{
    [SerializeField] List<Ability> _gameAbilities;
    [SerializeField] private List<Ability> _abilities;

    [SerializeField] List<string> abilityNameToSave = new List<string>();
    
    [SerializeField] private List<AbilityHolder> _holders = new List<AbilityHolder>();

    public AbilityWheel wheel;

    private void Awake()
    {
        PubSub.Instance.RegisterFunction(EMessageType.AbilityPicked, AddToAbilities);
        PubSub.Instance.RegisterFunction(EMessageType.ActiveAbilityChanged, GiveAbility);
        
        if (DataSerializer.TryLoad<List<string>>("ABILITIES", out abilityNameToSave))
        {
            foreach(string abilityName in abilityNameToSave)
            {
                Ability abilityToGive = _gameAbilities.Find(a => a.name == abilityName);
                AddToAbilities(abilityToGive);
            }

        }
        else
        {
            abilityNameToSave = new List<string>();
            _abilities = new List<Ability>();
        }

    }

    private void AddToAbilities(object obj)
    {
        // Filter
        if (obj is not Ability) return;
        Ability newAbility = obj as Ability;

        // Add to unlocked abilities
        _abilities.Add(newAbility);

        // Add to wheel
        wheel.AddToWheel(newAbility);

        if (!abilityNameToSave.Contains(newAbility.name))
            abilityNameToSave.Add(newAbility.name);

        DataSerializer.Save<List<string>>("ABILITIES", abilityNameToSave);
    }

    public void GiveAbility(object obj)
    {
        if (obj is not Sprite) return;

        Sprite abilityIcon = (Sprite)obj;
        Ability newActiveAbility = GetAbilityFrom(abilityIcon);

        foreach (AbilityHolder holder in _holders)
        {
            holder.activeAbility = newActiveAbility;
        }
    }

    private Ability GetAbilityFrom(Sprite abilityIcon)
    {
        return _abilities.Where(ability => ability.icon == abilityIcon)?.First();
    }
}
