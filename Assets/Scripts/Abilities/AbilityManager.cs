using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    List<ScriptableAbility> _abilities = new List<ScriptableAbility>();

    private void Awake()
    {
        _abilities = Resources.LoadAll<ScriptableAbility>("Abilities").ToList();
    }

    public List<Ability> GetUnlockedAbilities()
    {
        return _abilities.Where(ability => ability.abilityPrefab.unlocked == true)
                                .Select(ability => ability.abilityPrefab)
                                .ToList(); 
    }
}
