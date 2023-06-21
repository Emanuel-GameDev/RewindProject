using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
    }

    public override void Pick(Character picker)
    {
        Debug.Log("hfiuhaf");
        PlayerController controller = picker.GetComponent<PlayerController>();

        if (controller == null) return;

        controller.canDoubleJump = true;
        gameObject.SetActive(false);
    }

}
