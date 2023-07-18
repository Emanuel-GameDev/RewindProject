using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;   

public class AbilityManager : MonoBehaviour
{
    private List<Ability> _abilities = new List<Ability>();
    private GameObject abilityBin;
    
    [SerializeField] private List<AbilityHolder> _holders = new List<AbilityHolder>();
    [SerializeField] private AbilityWheel _wheel;

    private void Awake()
    {
        PubSub.Instance.RegisterFunction(EMessageType.AbilityPicked, AddToAbilities);
        PubSub.Instance.RegisterFunction(EMessageType.ActiveAbilityChanged, GiveAbility);
    }

    private void Start()
    {
        abilityBin = GameObject.Find("AbilityBin");
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
