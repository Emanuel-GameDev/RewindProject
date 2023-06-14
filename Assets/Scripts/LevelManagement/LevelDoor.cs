using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] SceneAsset levelToLoad;
    [SerializeField] GameObject levelSelectionMenu;

    List<DoorMenuSelectionButton> buttons;
    List<LevelLight> lights;
    public List<bool> checkpointTaken;

    private void OnEnable()
    {
        if (DataSerializer.HasKey(levelToLoad.name + "TAKENCHECKPOINTS"))
            checkpointTaken = DataSerializer.Load<List<bool>>(levelToLoad.name + "TAKENCHECKPOINTS");
            
    }

    private void Start()
    {
        buttons = new List<DoorMenuSelectionButton>(GetComponentsInChildren<DoorMenuSelectionButton>());
        lights = new List<LevelLight>(GetComponentsInChildren<LevelLight>(true));

        KindleLights();

        levelSelectionMenu.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
            collision.gameObject.GetComponent<PlayerController>().inputs.Player.Interaction.performed += Interact;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
            collision.gameObject.GetComponent<PlayerController>().inputs.Player.Interaction.performed -= Interact;

        levelSelectionMenu.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerController.instance.inputs.Player.Interaction.performed -= Interact;
    }

    private void KindleLights()
    {  
        if (checkpointTaken.Count < 1)
            return;

        for (int i = 0; i < lights.Count; i++)
        {
            
            if (checkpointTaken[i] == true)
            {
                lights[i].Light();
            }
        }
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (checkpointTaken.FindAll(taken => taken == true).Count <= 1)
            EnterDoor();
        else
            HandleMenu();
    }

    private void HandleMenu()
    {
        if (!levelSelectionMenu.activeSelf)
        {
            levelSelectionMenu.SetActive(true);

            for (int i = 0; i < checkpointTaken.Count; i++)
            {
                buttons[i].checkpointToLoadIndex = i;


                if (checkpointTaken[i])
                {
                    buttons[i].locked = false;
                    buttons[i].buttonText.color = buttons[i].baseColor;
                }
                else
                {
                    buttons[i].locked = true;
                    buttons[i].buttonText.color = Color.gray;
                }
            }
        }
        else
            levelSelectionMenu.SetActive(false);
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
