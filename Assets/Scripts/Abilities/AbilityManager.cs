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
    [SerializeField] private AbilityWheel _wheel;

    private void Awake()
    {
        abilityBin = new GameObject("AbilityBin");
        DontDestroyOnLoad(abilityBin);

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
        if (obj is not Ability) return;
        Ability newAbility = (Ability)obj;

        _abilities.Add(newAbility);

        _wheel.AddToWheel(newAbility);

        if(!abilityNameToSave.Contains(newAbility.name))
            abilityNameToSave.Add(newAbility.name);

        DataSerializer.Save<List<string>>("ABILITIES", abilityNameToSave);
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
