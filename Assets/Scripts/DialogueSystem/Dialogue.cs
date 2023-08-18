using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Drawing;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField] Line[] lines;
    [SerializeField] float characterPerSecond;

    [Header ("Boxes")]
    [SerializeField] TextMeshProUGUI speakerTextBox;
    [SerializeField] TextMeshProUGUI dialogueTextBox;

    public bool repeatable = false;
    public UnityEvent OnDialogueEnd;

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
        inputs = PlayerController.instance.inputs;
        inputs.Dialogue.Enable();
        inputs.Player.Disable();
        inputs.AbilityController.Disable();

        inputs.Dialogue.NextLyne.performed += NextLyne_performed;
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
        inputs.Player.Enable();
        inputs.AbilityController.Enable();
        inputs.Dialogue.NextLyne.performed -= NextLyne_performed;
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
            index = 0;
            OnDialogueEnd?.Invoke();
            gameObject.SetActive(false);
        }
    }

}
