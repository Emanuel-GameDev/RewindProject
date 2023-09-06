using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] SceneAsset levelToLoad;
    [SerializeField] GameObject levelSelectionMenu;
    [SerializeField] public MenuButton eventSystemDefaultButton;

    List<DoorMenuSelectionButton> buttons;
    List<LevelLight> lights;
    public List<bool> checkpointTaken;

    private void OnEnable()
    {
        if (DataSerializer.HasKey(levelToLoad.name + "TAKENCHECKPOINTS"))
            checkpointTaken = DataSerializer.Load<List<bool>>(levelToLoad.name + "TAKENCHECKPOINTS");

        PlayerController.instance.inputs.Menu.Enable();


    }

    private void Start()
    {
        buttons = new List<DoorMenuSelectionButton>(GetComponentsInChildren<DoorMenuSelectionButton>());
        lights = new List<LevelLight>(GetComponentsInChildren<LevelLight>(true));

        KindleLights();

        levelSelectionMenu.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        levelSelectionMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        PlayerController.instance.inputs.Player.Enable();
        PlayerController.instance.inputs.AbilityController.Enable();
        PlayerController.instance.inputs.Menu.CloseMenu.performed -= CloseMenu_performed;
    }
    private void OnDisable()
    {
        PlayerController.instance.inputs.Menu.CloseMenu.performed -= CloseMenu_performed;
        PlayerController.instance.inputs.Player.Enable();
        PlayerController.instance.inputs.AbilityController.Enable();
        PlayerController.instance.inputs.Menu.Disable();
    }


    private void KindleLights()
    {  
        if (checkpointTaken.Count < 1)
            return;

        for (int i = 0; i < lights.Count; i++)
        {
            
            if (checkpointTaken[i+1] == true)
            {
                lights[i].Light();
            }
        }
    }

    public void Interact()
    {
        
        if (checkpointTaken.FindAll(taken => taken == true).Count <= 1)
        {
            DataSerializer.Save("CHECKPOINTIDTOLOAD", 0);
            EnterDoor();
        }
        else
            HandleMenu();
    }

    private void HandleMenu()
    {
        
        if (!levelSelectionMenu.activeSelf)
        {
            
            levelSelectionMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(eventSystemDefaultButton.gameObject);
            PlayerController.instance.inputs.Menu.Enable();
            PlayerController.instance.inputs.Menu.CloseMenu.performed += CloseMenu_performed;

            PlayerController.instance.inputs.Player.Disable();
            PlayerController.instance.inputs.Player.Interaction.Enable();
            PlayerController.instance.inputs.AbilityController.Disable();

            for (int i = 0; i < checkpointTaken.Count; i++)
            {
                buttons[i].checkpointToLoadIndex = i;


                if (checkpointTaken[i])
                {
                    buttons[i].gameObject.SetActive(true);
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            levelSelectionMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            PlayerController.instance.inputs.Menu.CloseMenu.performed -= CloseMenu_performed;
            PlayerController.instance.inputs.Menu.Disable();
            PlayerController.instance.inputs.Player.Enable();
            PlayerController.instance.inputs.AbilityController.Enable();
        }
    }

    private void CloseMenu_performed(InputAction.CallbackContext obj)
    {
        HandleMenu();
    }

    public void EnterDoor()
    {
        //animazioni varie

        LoadLevel();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelToLoad.name);
    }

}
