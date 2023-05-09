using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private List<ScriptableAbility> _abilities = new List<ScriptableAbility>();
    
    [SerializeField] private List<AbilityHolder> _holders = new List<AbilityHolder>();

    private void Awake()
    {
        _abilities = Resources.LoadAll<ScriptableAbility>("Abilities").ToList();
        PubSub.Instance.RegisterFunction(EMessageType.ActiveAbilityChanged, GiveAbility);
    }

    public List<Ability> GetUnlockedAbilities()
    {
        return _abilities.Where(ability => ability.abilityPrefab.unlocked == true)
                                .Select(ability => ability.abilityPrefab)
                                .ToList(); 
    }

    public void GiveAbility(object obj)
    {
        if (obj is not Ability) return;
        Ability ability = (Ability)obj;

        foreach (AbilityHolder holder in _holders)
        {
            holder.activeAbility = ability;
        }
    }
}
