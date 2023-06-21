using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public new string name;
    public string description;
    public Sprite icon;
    public float activeTime;
    public float cooldownTime;

    public virtual void Activate(GameObject parent)
    {
        
    }

    public virtual void Pick(Character picker)
    {
        PubSub.Instance.Notify(EMessageType.AbilityPicked, this);
        //gameObject.SetActive(false);
    }
}
