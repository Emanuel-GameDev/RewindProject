using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public new string name;
    public Sprite icon;
    public float activeTime;
    public float cooldownTime;

    public virtual void Activate(GameObject parent)
    {
        
    }

    public virtual void Start()
    {
        // Per testing, alla fine bisognerà assegnare lo sprite della carta direttamente da editor
        // poiché ci sarà la luce come oggetto raccoglibile
        //icon = GetComponent<SpriteRenderer>().sprite;  
    }

    public virtual void Pick(Character picker)
    {
        PubSub.Instance.Notify(EMessageType.AbilityPicked, this);
        gameObject.SetActive(false);
    }
}
