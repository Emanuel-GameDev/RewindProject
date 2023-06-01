using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] Line[] lines;
    [SerializeField] float characterPerSecond;

    [Header ("Boxes")]
    [SerializeField] TextMeshProUGUI speakerTextBox;
    [SerializeField] TextMeshProUGUI dialogueTextBox;

    [System.Serializable]
    struct Line
    {
        public string speaker;
        public string textLine;
        
        public Line(string speaker,string line)
        {
            this.speaker = speaker;
            this.textLine = line;
        }
    }


    public PlayerInputs inputs { get; private set; }

    int index;

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Dialogue.Enable();
        inputs.Dialogue.NextLyne.performed += NextLyne_performed;
        PlayerController.instance.inputs.Player.Disable();
    }

    private void NextLyne_performed(InputAction.CallbackContext obj)
    {
        if(dialogueTextBox.text ==lines[index].textLine)
            NextLine();
        else
        {
            StopAllCoroutines();
            dialogueTextBox.text = lines[index].textLine;
        }
    }

    private void OnDisable()
    {
        inputs.Dialogue.Disable();
        inputs.Dialogue.NextLyne.performed -= NextLyne_performed;
        PlayerController.instance.inputs.Player.Enable();
    }

    private void Start()
    {
        dialogueTextBox.text = string.Empty;
        StartDialogue();
    }



    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        speakerTextBox.text = lines[index].speaker;
        foreach (char c in lines[index].textLine.ToCharArray())
        {
            dialogueTextBox.text += c;
            yield return new WaitForSeconds(1/characterPerSecond);
                 
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueTextBox.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
