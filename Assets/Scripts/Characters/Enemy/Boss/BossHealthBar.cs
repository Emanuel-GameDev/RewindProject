using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : HealthBar
{
    float maxHealth;
    float maxRotation = 360;
    [SerializeField] float rotaionDuration = 1f;

    private float currentRotation = 360;
    private float finalRotation;
    private float elapsedTime = 0.0f;

    public override void InitializeHealthBar(int health)
    {
        this.maxHealth = health;
        UpdateHealthBar(health);
    }

    public override void UpdateHealthBar(int health)
    {
        if (healthIconPrefab == null)
        {
            Debug.LogError("L'oggetto da ruotare non è stato assegnato.");
            return;
        }

        float percentuale = Mathf.Clamp01((health/maxHealth));

        float angoloDiRotazione = percentuale * maxRotation;

        finalRotation = -angoloDiRotazione;
        elapsedTime = 0.0f;
    }

    private void Update()
    {
        if(elapsedTime < rotaionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotaionDuration);
            currentRotation = Mathf.Lerp(currentRotation,finalRotation, t);
            Vector3 newRotation = healthIconPrefab.transform.eulerAngles;
            newRotation.z = currentRotation;
            healthIconPrefab.transform.eulerAngles = newRotation;
        }
    }

}
