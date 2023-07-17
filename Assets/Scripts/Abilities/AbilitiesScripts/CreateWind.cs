using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] LayerMask[] colliderMask;

    private GameObject windZoneObj;
    private bool canActivate = true;

    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        StartCoroutine(ActivatePower());
    }

    private IEnumerator ActivatePower()
    {
        canActivate = false;
        windZoneObj.SetActive(true);

        yield return new WaitForSeconds(activeTime);

        windZoneObj.SetActive(false);
        
        yield return new WaitForSeconds(cooldown);

        canActivate = true;
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);

        windZoneObj = Instantiate(windZonePrefab, picker.transform, false);
        windZoneObj.SetActive(false);

        InitializeWindZone();
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
}
