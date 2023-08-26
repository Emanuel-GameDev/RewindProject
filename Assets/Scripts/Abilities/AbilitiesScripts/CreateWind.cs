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
    [SerializeField] private float cooldown;
    [Tooltip("with this bool checked the cooldown will scale based on the time the ability is used, start cooldown is 0")]
    [SerializeField] private bool scalingCooldown;
    [Tooltip("The divider applied to the scale. the operation is seconds / this var")]
    [SerializeField] private float cooldownDivider = 1f;
    [Tooltip("The magnitude of the force to be applied to the wind")]
    [SerializeField] private float forceMagnitude = 10f;
    [Tooltip("The multiplier applied to the magnitude while pressing. the operation is seconds * this var")]
    [SerializeField] private float forceMultiplier = 1f;    
    [Tooltip("The angle in worldSpace of the force to be applied to the wind")]
    [SerializeField] private float forceAngle = 0f;
    [Tooltip("The layers that are affected by the wind")]
    [SerializeField] private LayerMask[] colliderMask;

    private GameObject windZoneObj;
    private PlayerInputs inputs;
    private bool facingRight = true;
    private GameObject currentHolder;
    private bool canActivate = true;
    private float lastActivationTime;

    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        isActive = true;

        if (windZoneObj == null)
        {
            windZoneObj = Instantiate(windZonePrefab, currentHolder.transform, false);
            windZoneObj.SetActive(false);
            InitializeWindZone();
        }

        if (scalingCooldown)
            cooldown = 0f;

        if (facingRight) SetWindZoneRotation(0f);
        else SetWindZoneRotation(180f);

        currentHolder.GetComponent<PlayerController>().inputs.Player.Disable();
        windZoneObj.SetActive(true);
    }

    public override void Disactivate1(GameObject gameObject)
    {
        if (!isActive) return;

        isActive = false;

        windZoneObj.GetComponent<AreaEffector2D>().forceMagnitude = forceMagnitude;
        windZoneObj.SetActive(false);
        currentHolder.GetComponent<PlayerController>().inputs.Player.Enable();

        canActivate = false;
        lastActivationTime = Time.time;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (isActive)
        {
            windZoneObj.GetComponent<AreaEffector2D>().forceMagnitude += (Time.deltaTime * forceMultiplier);

            if (scalingCooldown)
                cooldown += (Time.deltaTime / cooldownDivider);
        }
        if (!canActivate && Time.time >= (lastActivationTime + cooldown))
        {
            canActivate = true;
        }
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);

        currentHolder = picker.gameObject;

        windZoneObj = Instantiate(windZonePrefab, currentHolder.transform, false);
        windZoneObj.SetActive(false);
        InitializeWindZone();

        if (!canActivate)
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
