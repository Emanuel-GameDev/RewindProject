using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1)]
public class FadeEffect : MonoBehaviour
{
    [SerializeField] private float fadeInDuration;

    private Collider2D[] colliders;
    private MonoBehaviour[] scripts;


    bool activated = false;
    private void Start()
    {
        // Triggering Rendering
        Material material = GetComponent<Renderer>().material;
        Color startColor = material.color;

        startColor.a = 0f;
        material.color = startColor;

        DataSerializer.TryLoad(SceneManager.GetActiveScene().name + name + "Fade", out activated);


        if (activated)
        {
            material.color = Color.white;

            TriggerObject(true);
        }
        else
            TriggerObject(false);

    }

    private void TriggerObject(bool mode)
    {
        DataSerializer.Save(SceneManager.GetActiveScene().name + name + "Fade", mode);
        // Triggering Colliders 
        colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = mode;
        }

        // Triggering Scripts
        scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            // Except for this one
            if (script.GetType() != typeof(FadeEffect))
            {
                script.enabled = mode;
            }
        }
    }

    public void StartFade()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Color startColor = Color.clear;
        Color targetColor = Color.white;
        float elapsedTime = 0f;

        // Imposta l'alfa del colore iniziale a 0 per renderli invisibili all'inizio
        Material material = GetComponent<Renderer>().material;
        startColor = material.color;
        startColor.a = 0f;
        material.color = startColor;
        gameObject.SetActive(true);

        while (elapsedTime < fadeInDuration)
        {
            // Calcola l'alpha da interpolare
            float t = elapsedTime / fadeInDuration;

            Color newColor = Color.Lerp(startColor, targetColor, t);
            material.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Imposta l'alfa del colore a 1 per renderli completamente visibili
        targetColor.a = 1f;
        material.color = targetColor;

        TriggerObject(true);
    }
}
