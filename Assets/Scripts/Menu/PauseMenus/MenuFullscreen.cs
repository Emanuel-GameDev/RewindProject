using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFullscreen : Menu
{

    private void OnDisable()
    {
        PlayerController.instance.inputs.Menu.MenuInteractionOne.Enable();
    }

}
