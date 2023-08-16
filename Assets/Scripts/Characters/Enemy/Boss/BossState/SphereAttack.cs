using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAttack : State
{

    private float elapsed;
    private BossBheaviour bossBheaviour;
    Transform startPosition;
    bool isOncenterPosition;
    List<GameObject> projectiles = new();

    public SphereAttack(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        isOncenterPosition = bossBheaviour.GetCurrentPosition().GetHorizontalPosition() == eHorizontalPosition.Center;
        startPosition = isOncenterPosition ? bossBheaviour.GetCurrentPosition().transform : bossBheaviour.GetOppositePosition().transform;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;
        
        if(elapsed > bossBheaviour.GetWaitBeforeSpawn())
        {
            if (isOncenterPosition)
                SpawnOnCenter();
            else
                SpawnOnBorder();
        }




        if (elapsed > 2)
        {
            bossBheaviour.ChangeState();
        }

    }

    private void SpawnOnBorder()
    {
        throw new NotImplementedException();
    }

    private void SpawnOnCenter()
    {
        float offset = bossBheaviour.GetProjectileVerticalOffset();
        Vector2 spawnPoint = startPosition.transform.position;
        spawnPoint.y += (bossBheaviour.GetCurrentPosition().GetVerticalPosition() == eVerticalPosition.Top ? -offset : offset);
        if(bossBheaviour.GetNumberOfProjectile() % 2 == 0)
        {
            spawnPoint.x -= (bossBheaviour.GetDistanceBetweenProjectile() * (bossBheaviour.GetNumberOfProjectile() + 1)) / 2;
        }
        else
        {
            spawnPoint.x -= bossBheaviour.GetDistanceBetweenProjectile() * (bossBheaviour.GetNumberOfProjectile() - 1) / 2;
        }

        Spawn(spawnPoint);

    }

    private void Spawn(Vector2 spawnPoint)
    {
        for (int i = 0; i < bossBheaviour.GetNumberOfProjectile(); i++)
        {

        }
    }
}
