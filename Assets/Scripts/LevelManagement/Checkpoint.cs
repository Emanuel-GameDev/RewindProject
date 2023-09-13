using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public bool taken = false;
    public GameObject menu; 
    public string Hub;
    [SerializeField] public MenuButton eventSystemDefaultButton;

    private void Start()
    {
        if (menu != null)
            menu.SetActive(false);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (menu != null)
            menu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        PlayerController.instance.inputs.Menu.CloseMenu.performed -= CloseMenu_performed;
        PlayerController.instance.inputs.Menu.Disable();
        PlayerController.instance.inputs.Player.Enable();
        PlayerController.instance.inputs.AbilityController.Enable();
    }


    public void Interact()
    {
        taken = true;
        SetCheckpoint();


        HandleMenu();
    }

    private void HandleMenu()
    {
        if (menu == null)
            return;

        if (!menu.activeSelf)
        {
            menu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(eventSystemDefaultButton.gameObject);

            PlayerController.instance.inputs.Menu.CloseMenu.performed += CloseMenu_performed;
            PlayerController.instance.inputs.Menu.Enable();
            PlayerController.instance.inputs.Player.Disable();
            PlayerController.instance.inputs.AbilityController.Disable();
        }
        else
        {
            menu.SetActive(false);
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

    public void ReturnToHub()
    {
        LevelManager.instance.LoadLevel(Hub);
    }

    private void OnDisable()
    {
        PlayerController.instance.inputs.Menu.CloseMenu.performed -= CloseMenu_performed;
        PlayerController.instance.inputs.Menu.Disable();
        PlayerController.instance.inputs.Player.Enable();
        PlayerController.instance.inputs.AbilityController.Enable();
    }

    public void SetCheckpoint()
    {
        DataSerializer.Save("CHECKPOINTIDTOLOAD", LevelManager.instance.checkpoints.FindIndex(0,5,c=>c==this));
        DataSerializer.Save("SPAWNPOINT", transform.position);
        PlayerController.instance.GetComponent<Damageable>().SetMaxHealth();
        PubSub.Instance.Notify(EMessageType.CheckpointVisited, this);
    }
}
