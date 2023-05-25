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
    public bool unlocked = false;

    public virtual void Activate(GameObject parent)
    {
        
    }

    public virtual void Start()
    {
        icon = GetComponent<SpriteRenderer>().sprite;  
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            PubSub.Instance.Notify(EMessageType.AbilityPicked, this);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
