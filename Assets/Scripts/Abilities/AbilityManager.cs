using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private List<Ability> _abilities = new List<Ability>();
    
    [SerializeField] private List<AbilityHolder> _holders = new List<AbilityHolder>();

    private void Awake()
    {
        _abilities = Resources.LoadAll<Ability>("Abilities").ToList();
        PubSub.Instance.RegisterFunction(EMessageType.ActiveAbilityChanged, GiveAbility);
    }

    public List<Ability> GetUnlockedAbilities()
    {
        return _abilities.Where(ability => ability.unlocked == true)
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
