using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[CreateAssetMenu(menuName = "Ability/Projectile")]

public class ProjectileAbility : Ability
{
    [SerializeField] GameObject prefabPathCreator;
    [SerializeField] GameObject prefabProjectile;

    [SerializeField] float normalProjectileLifeTime = 5;
    [SerializeField] float normalProjectileSpeed = 1;

    [SerializeField] float guidedProjectileSpeed = 1;
    [SerializeField] float timeToDraw = 0;

    bool readyToUse = true;
    PathCreator instantietedPathCreator;



    public override void Activate1(GameObject parent)
    {
        if (!readyToUse || instantietedPathCreator.isDrawing)
            return;

        GameObject projectile = Instantiate(prefabProjectile, parent.transform.position, Quaternion.Euler(0, 0, PlayerController.instance.groundAngle));
        
        if(PlayerController.instance.bodySprite.transform.localScale.x >= 0)
            projectile.GetComponent<PlayerProjectile>().Inizialize(Quaternion.Euler(0,0,-PlayerController.instance.groundAngle) *  Vector2.right, PlayerController.instance.transform.position + Vector3.right, normalProjectileSpeed*60);
        else
            projectile.GetComponent<PlayerProjectile>().Inizialize(Quaternion.Euler(0, 0, -PlayerController.instance.groundAngle) * -Vector2.right, PlayerController.instance.transform.position + Vector3.left, normalProjectileSpeed* 60);

        projectile.GetComponent<PlayerProjectile>().lifeTime = normalProjectileLifeTime;

        readyToUse = false;
        LevelManager.instance.StartCoroutine(Cooldown());
    }

    public override void Activate2(GameObject parent)
    {
        if (!readyToUse)
            return;

        PlayerController.instance.inputs.Player.Disable();

        GameObject instantietedPathCreatorObj = Instantiate(prefabPathCreator);
        instantietedPathCreator = instantietedPathCreatorObj.GetComponent<PathCreator>();
        instantietedPathCreator.projectileSpeed = guidedProjectileSpeed;
        instantietedPathCreator.isDrawing = true;

        instantietedPathCreator.StartCoroutine(DrawTimer());
        
    }

    IEnumerator DrawTimer()
    {
        yield return new WaitForSeconds(timeToDraw);
        if (readyToUse && !instantietedPathCreator.instatietedProjectile)
        {
            SpawnGuidedProjectile();
            PlayerController.instance.inputs.Player.Enable();
        }
        LevelManager.instance.StopCoroutine(DrawTimer());
    }

    public override void Disactivate2(GameObject gameObject)
    {
        if (!readyToUse || instantietedPathCreator.instatietedProjectile)
            return;
        LevelManager.instance.StopCoroutine(DrawTimer());
        SpawnGuidedProjectile();
        PlayerController.instance.inputs.Player.Enable();
    }

    private void SpawnGuidedProjectile()
    {
        LevelManager.instance.StartCoroutine(Cooldown());
        instantietedPathCreator.isDrawing = false;
        instantietedPathCreator.SpawnProjectile(prefabProjectile);
        readyToUse = false;
    }

    private void OnEnable()
    {
        readyToUse = true;
    }

    

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        readyToUse = true;
    }
}
