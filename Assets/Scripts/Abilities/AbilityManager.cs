using System.Collections.Generic;
using System.Linq;
using ToolBox.Serialization;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] List<Ability> _gameAbilities;
    [SerializeField] private List<Ability> _abilities;

    [SerializeField] List<string> abilityNameToSave = new List<string>();

    [SerializeField] private List<AbilityHolder> _holders = new List<AbilityHolder>();

    public AbilityWheel wheel;

    #region Debug Variables
    [HideInInspector]
    public List<Ability> DebugAbilities = new List<Ability>();

    #endregion

    private void Awake()
    {
        PubSub.Instance.RegisterFunction(EMessageType.AbilityPicked, AddToAbilities);
        PubSub.Instance.RegisterFunction(EMessageType.ActiveAbilityChanged, GiveAbility);
    }

    private void Start()
    {

        if (!GameManager.Instance.debug)
        {


            if (DataSerializer.TryLoad<List<string>>("ABILITIES", out abilityNameToSave))
            {
                foreach (string abilityName in abilityNameToSave)
                {
                    Ability abilityToGive = _gameAbilities.Find(a => a.name == abilityName);
                    abilityToGive.Pick(PlayerController.instance);
                }

            }
            else
            {
                abilityNameToSave = new List<string>();
                _abilities = new List<Ability>();
            }

        }

    }

    private void AddToAbilities(object obj)
    {
        // Filter
        if (obj is not Ability) return;
        Ability newAbility = obj as Ability;

        // Add to unlocked abilities
        if (GameManager.Instance.debug)
            DebugAbilities.Add(newAbility);
        else
            _abilities.Add(newAbility);

        if (!newAbility.passive)
        {
            // Add to wheel
            wheel.AddToWheel(newAbility);
        }

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

    public Ability GetAbilityFrom(Sprite abilityIcon)
    {
        if (GameManager.Instance.debug)
            return DebugAbilities.Where(ability => ability.icon == abilityIcon)?.First();

        return _abilities.Where(ability => ability.icon == abilityIcon)?.First();
    }

    public List<Ability> GetUnlockedAbilities()
    {
        if (GameManager.Instance.debug)
            return DebugAbilities;
        else
            return _abilities;
    }
}
