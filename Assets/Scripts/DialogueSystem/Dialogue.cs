using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Drawing;
using UnityEngine.Events;
using ToolBox.Serialization;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{

    [SerializeField] Interaction[] dialogues;
    [SerializeField] float characterPerSecond;

    [Header ("Boxes")]
    [SerializeField] TextMeshProUGUI speakerTextBox;
    [SerializeField] TextMeshProUGUI dialogueTextBox;
    [SerializeField] AudioSource audioSource;

    public PlayerInputs inputs { get; private set; }

    int lineIndex;
    int dialogueIndex;

    
    public UnityEvent OnDialoguesEnded;

    [System.Serializable]
    struct Interaction
    {
        public Line[] lines;

        public Interaction(Line[] lines)
        {
            this.lines = lines;
        }
    }

    [System.Serializable]
    struct Line
    {
        public string speaker;
        public UnityEngine.Color speakerColor;
        public string textLine;
        public UnityEngine.Color textColor;
        public AudioClip speakerVoiceAudio;

        public Line(string speaker, UnityEngine.Color speakerColor, string line, UnityEngine.Color textColor, AudioClip speakerVoiceAudio)
        {
            this.speaker = speaker;
            this.speakerColor = speakerColor;
            this.textLine = line;
            this.textColor = textColor;
            this.speakerVoiceAudio = speakerVoiceAudio;
        }
    }



    private void OnEnable()
    {
        inputs = PlayerController.instance.inputs;
        inputs.Dialogue.Enable();
        inputs.Player.Disable();
        inputs.AbilityController.Disable();

        audioSource = GetComponent<AudioSource>();

        if (DataSerializer.TryLoad<int>(SceneManager.GetActiveScene().name + transform.parent.parent.position.x + transform.parent.parent.position.y + "dialogueIndex", out int dIndex))
            dialogueIndex = dIndex;

        inputs.Dialogue.NextLyne.performed += NextLyne_performed;
        dialogueTextBox.text = string.Empty;
        StartDialogue();
    }

    private void NextLyne_performed(InputAction.CallbackContext obj)
    {
        if(dialogueTextBox.text == dialogues[dialogueIndex].lines[lineIndex].textLine)
            NextLine();
        else
        {
            StopAllCoroutines();
            dialogueTextBox.text = dialogues[dialogueIndex].lines[lineIndex].textLine;
        }
    }

    private void OnDisable()
    {
        inputs.Dialogue.Disable();
        inputs.Player.Enable();
        inputs.AbilityController.Enable();
        inputs.Dialogue.NextLyne.performed -= NextLyne_performed;
    }

    

    void StartDialogue()
    {
        lineIndex = 0;
        speakerTextBox.color = dialogues[dialogueIndex].lines[lineIndex].speakerColor;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        speakerTextBox.text = dialogues[dialogueIndex].lines[lineIndex].speaker;
        dialogueTextBox.color = dialogues[dialogueIndex].lines[lineIndex].textColor;
        
        if(audioSource!=null && dialogues[dialogueIndex].lines[lineIndex].speakerVoiceAudio != null)
        {
            audioSource.clip = dialogues[dialogueIndex].lines[lineIndex].speakerVoiceAudio;
            audioSource.Play();
        }

        foreach (char c in dialogues[dialogueIndex].lines[lineIndex].textLine.ToCharArray())
        {
            dialogueTextBox.text += c;
            yield return new WaitForSeconds(1/characterPerSecond);
                 
        }
    }

    void NextLine()
    {
        if (lineIndex < dialogues[dialogueIndex].lines.Length - 1)
        {
            if (audioSource != null)
                audioSource.Stop();

            lineIndex++;
            dialogueTextBox.text = string.Empty;
            speakerTextBox.color = dialogues[dialogueIndex].lines[lineIndex].speakerColor;
            StartCoroutine(TypeLine());
        }
        else
        {
            lineIndex = 0;

            dialogueIndex++;

            if (dialogueIndex == dialogues.Length)
            {
                dialogueIndex--;
                OnDialoguesEnded?.Invoke();
            }
            DataSerializer.Save(SceneManager.GetActiveScene().name + transform.parent.parent.position.x + transform.parent.parent.position.y + "dialogueIndex", dialogueIndex);
            gameObject.SetActive(false);
        }
    }

}
