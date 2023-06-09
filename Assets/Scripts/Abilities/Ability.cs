using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class Ability : ScriptableObject
{
    public new string name;
    public string description;  
    public Sprite icon;
    public float cooldownTime;

    public virtual void Activate1(GameObject parent) { }
    public virtual void Activate2(GameObject parent) { }
    public virtual void Activate3(GameObject parent) { }

    public virtual void Disactivate1(GameObject gameObject) { }
    public virtual void Disactivate2(GameObject gameObject) { }
    public virtual void Disactivate3(GameObject gameObject) { }

    public virtual void Start() { }


    public virtual void Pick(Character picker)
    {
        PubSub.Instance.Notify(EMessageType.AbilityPicked, this);
    }

}
