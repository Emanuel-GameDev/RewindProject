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

    private GameObject abilityBin;

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
        // Filter
        if (obj is not Ability) return;
        Ability abilityToCopy = (Ability)obj;

        // Creation of copy GO, otherwise ability won't be activated
        GameObject abilityGO = new GameObject();
        abilityGO.AddComponent(abilityToCopy.GetType());

        // Parenting the new GO to keep organized
        abilityGO.transform.parent = abilityBin.transform;

        // Copying variables between scripts
        Ability newAbility = abilityGO.GetComponent<Ability>();
        abilityToCopy.CopyValuesTo(newAbility);
        abilityGO.name = newAbility.name;

        // Add to unlocked abilities
        _abilities.Add(newAbility);

        // Add to wheel
        _wheel.AddToWheel(newAbility);

        if (!abilityNameToSave.Contains(newAbility.name))
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
