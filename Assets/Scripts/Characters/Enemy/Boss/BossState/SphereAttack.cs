using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SphereAttack : State
{

    private float elapsed;
    private BossBheaviour bossBheaviour;
    Transform startPosition;
    bool isOnCenterPosition;
    List<BossProjectile> projectiles = new();
    bool readyToShoot;
    bool shooting;
    Vector2 directionToShoot;

    public SphereAttack(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        isOnCenterPosition = bossBheaviour.GetCurrentPosition().GetHorizontalPosition() == eHorizontalPosition.Center;
        startPosition = isOnCenterPosition ? bossBheaviour.GetCurrentPosition().transform : bossBheaviour.GetOppositePosition().transform;
        readyToShoot = false;
        shooting = false;
        directionToShoot = Vector2.zero;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;
        if (shooting)
        {
            if(elapsed> bossBheaviour.GetProjectileLifeTime())
            {
                bossBheaviour.ChangeState();
            }
        }
        else if (readyToShoot)
        {
            if(elapsed > bossBheaviour.GetWaitBeforeShot())
            {
                Shoot();
            }
        }
        else if(elapsed > bossBheaviour.GetWaitBeforeSpawn())
        {
            SpawnProjectiles();
        }
    }

    private void Shoot()
    {
        
        foreach(BossProjectile projectile in projectiles)
        {
            if (projectile != null)
            {
                projectile.Inizialize(directionToShoot, projectile.transform.position, bossBheaviour.GetProjectileSpeed());
            }
        }
        
        shooting = true;
        elapsed = 0;
    }

    private Vector2 SpawnOnBorder(Vector2 spawnPoint)
    {
        if (bossBheaviour.GetCurrentPosition().GetHorizontalPosition() == eHorizontalPosition.Left)
        {
            spawnPoint.x -= bossBheaviour.GetProjectileHorizontalOffset() + (bossBheaviour.GetDistanceBetweenProjectile() * (bossBheaviour.GetNumberOfProjectile() - 1));
            directionToShoot = Vector2.right;
        }
        else
        {
            spawnPoint.x += bossBheaviour.GetProjectileHorizontalOffset();
            directionToShoot = Vector2.left;
        }
            

        return spawnPoint;
    }

    private Vector2 SpawnOnCenter(Vector2 spawnPoint)
    {
        spawnPoint.x -= bossBheaviour.GetDistanceBetweenProjectile() * (bossBheaviour.GetNumberOfProjectile() - 1) / 2;

        return spawnPoint;
    }

    private void SpawnProjectiles()
    {
        float offset = bossBheaviour.GetProjectileVerticalOffset();
        Vector2 spawnPoint = startPosition.transform.position;
        if (bossBheaviour.GetCurrentPosition().GetVerticalPosition() == eVerticalPosition.Top)
        {
            spawnPoint.y -= offset;
            directionToShoot = Vector2.down;
        }
        else
        {
            spawnPoint.y += offset;
            directionToShoot = Vector2.up;
        }

        if (isOnCenterPosition)
            spawnPoint = SpawnOnCenter(spawnPoint);
        else
            spawnPoint = SpawnOnBorder(spawnPoint);

        for (int i = 0; i < bossBheaviour.GetNumberOfProjectile(); i++)
        {
            Vector2 point = spawnPoint;
            point.x += bossBheaviour.GetDistanceBetweenProjectile() * i;
            projectiles.Add(bossBheaviour.GenerateProjectile(point));
        }
        readyToShoot = true;
        elapsed = 0;
    }
}
