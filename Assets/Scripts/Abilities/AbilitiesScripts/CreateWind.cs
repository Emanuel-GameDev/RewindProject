using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        if (windZoneObj == null)
        {
            if (facingRight)
                windZoneObj = Instantiate(windZonePrefab, parent.transform, false);
            else
            {
                windZoneObj = Instantiate(windZonePrefab, parent.transform, false);
                SetWindZoneRotation(180f);
            }

            windZoneObj.SetActive(false);

            InitializeWindZone();
        }

        StartCoroutine(ActivatePower());
    }

    private IEnumerator ActivatePower()
    {
        canActivate = false;

        if (facingRight) SetWindZoneRotation(0f);
        else SetWindZoneRotation(180f);

        windZoneObj.SetActive(true);

        yield return new WaitForSeconds(activeTime);

        windZoneObj.SetActive(false);
        
        yield return new WaitForSeconds(cooldown);

        canActivate = true;
    }

    public override void CopyValuesTo(Ability newAbility)
    {
        base.CopyValuesTo(newAbility);

        CreateWind newCreateWind = newAbility as CreateWind;

        newCreateWind.windZonePrefab = windZonePrefab;
        newCreateWind.activeTime = activeTime;
        newCreateWind.cooldown = cooldown;
        newCreateWind.forceMagnitude = forceMagnitude;
        newCreateWind.forceAngle = forceAngle;
        newCreateWind.colliderMask = colliderMask;
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);
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

    public override void Start()
    {
        base.Start();
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

    private void OnEnable()
    {
        inputs = new PlayerInputs();

        if (!inputs.Player.enabled)
            inputs.Player.Enable();

        inputs.Player.Walk.performed += CheckHorizontalFacing;
        inputs.Player.Run.performed += CheckHorizontalFacing;
    }

    private void SetWindZoneRotation(float angle)
    {
        if (windZoneObj == null) return;

        Vector3 currentRotation = windZoneObj.transform.localEulerAngles;
        windZoneObj.transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, angle);
    }

    private void OnDisable()
    {
        inputs.Player.Walk.performed -= CheckHorizontalFacing;
        inputs.Player.Run.performed -= CheckHorizontalFacing;
    }
}
