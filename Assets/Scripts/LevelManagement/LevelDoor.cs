using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public SceneAsset levelToLoad;
    [SerializeField] GameObject levelSelectionMenu;

    List<LevelSelectionButton> buttons;
    public List<LevelLight> lights;
    public List<bool> checkpointTaken;

    public Checkpoint spawn;

    private void Start()
    {
        buttons = new List<LevelSelectionButton>(GetComponentsInChildren<LevelSelectionButton>());
        

        levelSelectionMenu.SetActive(false);
    }

    private void OnEnable()
    {
        lights = new List<LevelLight>(GetComponentsInChildren<LevelLight>());
        if (PlayerPrefsExtra.GetList<bool>("a").Count > 0)
        {
            checkpointTaken = PlayerPrefsExtra.GetList<bool>("a", checkpointTaken);
            Debug.Log(levelToLoad.name);

        }

        for (int i = 0; i < checkpointTaken.Count; i++)
        {
            if (checkpointTaken[i] == true)
                lights[i].Light();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovementInput>())
            collision.gameObject.GetComponent<PlayerMovementInput>().inputs.Player.Interaction.performed += Interact; 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovementInput>())
            collision.gameObject.GetComponent<PlayerMovementInput>().inputs.Player.Interaction.performed -= Interact;
    }

    private void OnDisable()
    {
        PlayerMovementInput.instance.inputs.Player.Interaction.performed -= Interact;
    }


    private void Interact(InputAction.CallbackContext obj)
    {
        //int checkpointTaken = 0;

        //foreach (bool taken in level.checkpointsTaken)
        //{
        //    if (taken == true)
        //        checkpointTaken++;
        //}

        if (checkpointTaken.FindAll(taken => taken == true).Count > 1)
        {
            if (!levelSelectionMenu.activeSelf)
            {
                levelSelectionMenu.SetActive(true);
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].checkpointToLoadIndex = i;

                    if (i == 0)
                        buttons[i].unlocked = true;
                    else
                        buttons[i].unlocked = false;
                }
            }
            else
                levelSelectionMenu.SetActive(false);

        }
        else
        {
            //level.lastCheckpointVisitedIndex = 0;
            EnterDoor();
        }

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
