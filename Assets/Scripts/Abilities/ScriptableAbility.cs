using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Scriptable Ability")]
public class ScriptableAbility : ScriptableObject
{
    public Ability abilityPrefab;
}
