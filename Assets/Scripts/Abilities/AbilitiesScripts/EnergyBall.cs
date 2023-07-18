using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : Ability
{
    [Header("ENERGY BALL DATA")]
    [SerializeField] private GameObject energyBallPrefab;
    [SerializeField] private GameObject wandPrefab;

    [SerializeField] private float energyBallSpeed;
    [SerializeField] private float cooldown;

    public override void Activate1(GameObject parent)
    {
        if (wandPrefab == null || energyBallPrefab == null)
        {
            Debug.LogError("Error: Missing References, add them from Assets/Prefab/Abilities/EnergyBall");
            return;
        }


    }

    public override void CopyValuesTo(Ability newAbility)
    {
        base.CopyValuesTo(newAbility);

        EnergyBall newEnergyBall = newAbility as EnergyBall;

        newEnergyBall.energyBallPrefab = energyBallPrefab;
        newEnergyBall.wandPrefab = wandPrefab;
        newEnergyBall.energyBallSpeed = energyBallSpeed;    
        newEnergyBall.cooldown = cooldown;
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);
    }

    public override void Start()
    {
        base.Start();
    }
}
