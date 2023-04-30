using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public new string name;
    public Sprite icon;
    public bool unlocked = false;
}
