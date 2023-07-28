using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerTriggerCause : Cause
{
    [Tooltip("Check this if you want the target to press a button inside an area to trigger effect")]
    [SerializeField] private bool buttonInteraction;
    private bool areaEntered = false;

    [SerializeField] private LayerMask targetLayer;
    private Character target;

    private PlayerInputs inputs;

    protected override void ActivateEffect()
    {
        effect?.Invoke();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();

        inputs.Player.Interaction.performed += Interaction;
    }

    protected override void OnValidate()
    {
        if (!GetComponent<CircleCollider2D>().isTrigger)
            GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void Interaction(InputAction.CallbackContext obj)
    {
        if (!buttonInteraction || !areaEntered) return;

        if (target != null)
        {
            ActivateEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetLayer.value, 2f)))
        {
            areaEntered = true;

            if (buttonInteraction) return;

            target = collision.gameObject.GetComponent<Character>();
            ActivateEffect();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetLayer.value, 2f)))
        {
            areaEntered = false;

            if (buttonInteraction) return;
            target = null;
        }
    }
}
