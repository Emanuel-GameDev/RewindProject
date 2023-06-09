using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/DoubleJump")]
public class DoubleJump : Ability
{
    public override void Activate1(GameObject parent)
    {
        base.Activate1(parent);
    }

    public override void Pick(Character picker)
    {
        PlayerController controller = picker.GetComponent<PlayerController>();

        if (controller == null) return;

        controller.canDoubleJump = true;
        DataSerializer.Save("CANDOUBLEJUMP", true);
    }

    
}
