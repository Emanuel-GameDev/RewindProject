using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public static class CompleteTextWithButtonPromptSprite
{
    
    public static string ReadAndReplaceBindings(string textToDisplay,InputBinding actionNeeded, InputAction inputAction)
    {
        string stringButtonName = actionNeeded.ToString();
        stringButtonName = RenameInput(stringButtonName, inputAction);
        
        textToDisplay = textToDisplay.Replace("BUTTONPROMPT", $"<sprite name=\"{stringButtonName}\">");

        return textToDisplay;

    }



    private static string RenameInput(string stringButtonName, InputAction actionNeeded)
    {
        stringButtonName = stringButtonName.Replace($"{actionNeeded.name}:", string.Empty);
        stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");
        stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");

        return stringButtonName; 
    }

    

}
