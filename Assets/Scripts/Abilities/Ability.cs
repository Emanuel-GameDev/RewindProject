using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public new string name;
    public string description;  
    public Sprite icon;

    public virtual void Activate1(GameObject parent) { }
    public virtual void Activate2(GameObject parent) { }
    public virtual void Activate3(GameObject parent) { }

    public virtual void Disactivate1(GameObject gameObject) { }
    public virtual void Disactivate2(GameObject gameObject) { }
    public virtual void Disactivate3(GameObject gameObject) { }

    public virtual void Start() { }

    public virtual void CopyValuesTo(Ability newAbility)
    {
        if (newAbility.GetType() != GetType())
        {
            Debug.LogError("Error: types of abilityPicked and abilityCopy doesn't match");
            return;
        }

        newAbility.name = name;
        newAbility.description = description;
        newAbility.icon = icon;
    }

    public virtual void Pick(Character picker)
    {
        Ability ab = GetComponent<Ability>();

        PubSub.Instance.Notify(EMessageType.AbilityPicked, ab);
        gameObject.SetActive(false);
    }
}