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
}
