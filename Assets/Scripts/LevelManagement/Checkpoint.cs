using System;
using System.Collections;
using System.Collections.Generic;
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
        menu.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovementInput>())
            collision.gameObject.GetComponent<PlayerMovementInput>().inputs.Player.Interaction.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        taken = true;
        SetCheckpoint();
        OpenMenu();
    }

    private void OpenMenu()
    {
        if (!menu.activeSelf)
            menu.SetActive(true);
        else
            menu.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        menu.SetActive(false);

        if (collision.gameObject.GetComponent<PlayerMovementInput>())
            collision.gameObject.GetComponent<PlayerMovementInput>().inputs.Player.Interaction.performed -= Interact;
    }

    private void OnDisable()
    {
        PlayerMovementInput.instance.inputs.Player.Interaction.performed -= Interact;
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene(Hub.name);
    }

    public void SetCheckpoint()
    {
        LevelManager.spawnPoint = transform;
        PubSub.Instance.SendMessages(eMessageType.CheckpointVisited, this);
    }
}
