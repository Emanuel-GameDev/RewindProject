using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.InputSystem;

public class AbilityWheel : MonoBehaviour
{
    [SerializeField] RectTransform rotPoint;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float rotationSpeed;
    //[SerializeField] private float rotationDuration;

    private PlayerInputs inputs;
    
    private bool isRotating = false;    

    private void Start()
    {
    }

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.UI.Enable();

        inputs.UI.ScrollWheel.performed += ScrollInput;
    }

    private void OnDisable()
    {
        inputs.UI.Disable();

        inputs.UI.ScrollWheel.performed -= ScrollInput;
    }

    private void ScrollInput(InputAction.CallbackContext scroll)
    {
        float scrollValue = scroll.ReadValue<Vector2>().y;
        float currentRotation = rotPoint.localEulerAngles.z;

        if (scrollValue > 0f)
        {
            Quaternion newRotation = Quaternion.Euler(0f, 0f, currentRotation + rotationAmount);

            if (!isRotating)
                StartCoroutine(Rotate(newRotation));

        }
        else if (scrollValue < 0f)
        {
            Quaternion newRotation = Quaternion.Euler(0f, 0f, currentRotation - rotationAmount);

            if (!isRotating)
                StartCoroutine(Rotate(newRotation));

        }
    }

    private IEnumerator Rotate(Quaternion newRotation)
    {

        isRotating = true;
        Debug.Log("inizio A ruotare");

        // Loop finché l'angolo tra le due rotazioni non è vicino a 0
        while (Quaternion.Angle(rotPoint.localRotation, newRotation) > 0.01f)
        {
            rotPoint.localRotation = Quaternion.Lerp(rotPoint.localRotation, newRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        //float elapsedTime = 0f;
        // Loop finché il tempo passato non è minore della durata
        //while (elapsedTime < rotationDuration)
        //{
        //    elapsedTime += Time.deltaTime;

        //    float t = Mathf.Clamp01(elapsedTime / rotationDuration);
        //    rotPoint.localRotation = Quaternion.Lerp(rotPoint.localRotation, newRotation, t);

        //    yield return null;
        //}

        isRotating = false;
        rotPoint.localRotation = newRotation;
    }
}
