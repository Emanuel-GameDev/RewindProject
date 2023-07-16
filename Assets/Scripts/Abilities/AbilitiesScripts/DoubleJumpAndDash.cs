using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleJumpAndDash : Ability
{   

    private PlayerController character;

    public override void Activate1(GameObject parent)
    {
        base.Activate1(parent);
    }

    public override void Pick(Character picker)
    {
        character = picker.GetComponent<PlayerController>();

        if (character == null) return;

        character.canDoubleJump = true;
      
        gameObject.SetActive(false);
    }

    public override void Start()
    {
        base.Start();
    }


}
