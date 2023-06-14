using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public bool taken = false;
    public GameObject menu; 
    public SceneAsset Hub;


    private void Start()
    {
        if (menu != null)
            menu.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
            collision.gameObject.GetComponent<PlayerController>().inputs.Player.Interaction.performed += Interact;
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (menu != null)
            menu.SetActive(false);

        if (collision.gameObject.GetComponent<PlayerController>())
            collision.gameObject.GetComponent<PlayerController>().inputs.Player.Interaction.performed -= Interact;
    }

    private void OnDisable()
    {
        PlayerController.instance.inputs.Player.Interaction.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext obj)
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
            menu.SetActive(true);
        else
            menu.SetActive(false);
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene(Hub.name);
    }

    public void SetCheckpoint()
    {
        DataSerializer.Save("SPAWNPOINT", transform.position);
        PlayerController.instance.GetComponent<Damageable>().SetMaxHealth();
        PubSub.Instance.Notify(EMessageType.CheckpointVisited, this);
    }
}
