using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] SceneAsset levelToLoad;
    [SerializeField] GameObject levelSelectionMenu;

    List<LevelSelectionButton> buttons;
    List<LevelLight> lights;
    public List<bool> checkpointTaken;


    private void Start()
    {
        buttons = new List<LevelSelectionButton>(GetComponentsInChildren<LevelSelectionButton>());
        lights = new List<LevelLight>(GetComponentsInChildren<LevelLight>());

        GetTakenCheckpoints();
        KindleLights();

        levelSelectionMenu.SetActive(false);
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

        levelSelectionMenu.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerMovementInput.instance.inputs.Player.Interaction.performed -= Interact;
    }

    private void GetTakenCheckpoints()
    {
        if (!LevelMaster.instance.levels.ContainsKey(levelToLoad.name))
            return;

        if (LevelMaster.instance.levels[levelToLoad.name].Count > 0)
            checkpointTaken = LevelMaster.instance.levels[levelToLoad.name];
    }

    private void KindleLights()
    {
        for (int i = 0; i < checkpointTaken.Count; i++)
        {
            if (checkpointTaken[i] == true)
            {
                lights[i].Light();
            }
        }
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (checkpointTaken.FindAll(taken => taken == true).Count < 1)
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
                    buttons[i].unlocked = true;
                else
                    buttons[i].unlocked = false;
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
