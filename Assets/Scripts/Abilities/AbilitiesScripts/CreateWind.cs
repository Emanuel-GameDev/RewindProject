using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CreateWind", menuName = "Ability/CreateWind")]
public class CreateWind : Ability
{
    [Header("WIND DATA")]
    [SerializeField] GameObject windZonePrefab;
    [SerializeField] private float activeTime;
    [SerializeField] private float cooldown;
    [Tooltip("The magnitude of the force to be applied to the wind")]
    [SerializeField] private float forceMagnitude = 10f;
    [Tooltip("The angle in worldSpace of the force to be applied to the wind")]
    [SerializeField] private float forceAngle = 0f;
    [Tooltip("The layers that are affected by the wind")]
    [SerializeField] private LayerMask[] colliderMask;

    private GameObject windZoneObj;
    private bool canActivate = true;
    private PlayerInputs inputs;
    private bool facingRight = true;
    private float lastActivationTime = 0f;
    private GameObject currentHolder;

    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        if (windZoneObj == null)
        {
            windZoneObj = Instantiate(windZonePrefab, parent.transform, false);
            windZoneObj.SetActive(false);
            InitializeWindZone();
        }

        if (facingRight) SetWindZoneRotation(0f);
        else SetWindZoneRotation(180f);

        if (currentHolder == parent)
            //currentHolder.GetComponent<PlayerController>().enabled = false;
            currentHolder.GetComponent<PlayerController>().inputs.Disable();

        windZoneObj.SetActive(true);

        canActivate = false;
        lastActivationTime = Time.time;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (windZoneObj == null) return;

        if (!canActivate && Time.time >= (lastActivationTime + activeTime) + cooldown)
        {
            canActivate = true;
        }
        else if (!canActivate && Time.time >= lastActivationTime + activeTime && windZoneObj.activeSelf)
        {
            windZoneObj.SetActive(false);
            currentHolder.GetComponent<PlayerController>().inputs.Enable();
        }

    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);

        currentHolder = picker.gameObject;
        canActivate = true;
    }

    private void InitializeWindZone()
    {
        AreaEffector2D windArea = windZoneObj.GetComponent<AreaEffector2D>();

        windArea.forceAngle = forceAngle;
        windArea.forceMagnitude = forceMagnitude;

        int combinedMask = 0;
        foreach (LayerMask layerMask in colliderMask)
        {
            combinedMask |= layerMask;
        }
        windArea.colliderMask = combinedMask;
    }

    private void CheckHorizontalFacing(InputAction.CallbackContext obj)
    {
        float value = obj.ReadValue<float>();

        if (value < 0)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

    private void SetWindZoneRotation(float angleZ)
    {
        windZoneObj.transform.rotation = Quaternion.Euler(windZoneObj.transform.rotation.x, windZoneObj.transform.rotation.y, angleZ);
    }

    private void OnEnable()
    {
        inputs = new PlayerInputs();

        if (!inputs.Player.enabled)
            inputs.Player.Enable();

        inputs.Player.Walk.performed += CheckHorizontalFacing;
        inputs.Player.Run.performed += CheckHorizontalFacing;
    }

    private void OnDisable()
    {
        inputs.Player.Walk.performed -= CheckHorizontalFacing;
        inputs.Player.Run.performed -= CheckHorizontalFacing;
    }
}
