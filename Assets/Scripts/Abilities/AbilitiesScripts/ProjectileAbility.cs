using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/ProjectileAbility")]
public class ProjectileAbility : Ability
{
    [SerializeField] GameObject prefabPathCreator;
    [SerializeField] GameObject prefabProjectile;

    [SerializeField] float normalProjectileLifeTime = 5;
    [SerializeField] float normalProjectileSpeed = 1;

    [SerializeField] float guidedProjectileSpeed = 1;
    [SerializeField] float timeToDraw = 0;

    [SerializeField] AudioClip shotProjectileSound;
    [SerializeField] AudioClip drawPathProjectileSound;

    bool readyToUse = true;
    PathCreator instantietedPathCreator;
    PlayerController player;

    private void OnEnable()
    {
        readyToUse = true;
    }

    public override void Activate1(GameObject parent)
    {
        if (!readyToUse || instantietedPathCreator != null)
            return;

        if(parent.GetComponent<PlayerController>())
            player = parent.GetComponent<PlayerController>();

        if (!player.grounded)
            return;

        player.animator.SetBool("UsingCard", true);
        player.animator.SetTrigger("ActivateCard");
        player.StartCoroutine(WaitAndSpawnNormalProjectile());

        player.inputs.Player.Disable();
    }

    private void ShotProjectile( )
    {
        GameObject projectile = Instantiate(prefabProjectile, player.projectileSpawn.position, Quaternion.Euler(0, 0, player.groundAngle));

        if (PlayerController.instance.bodySprite.transform.localScale.x >= 0)
            projectile.GetComponent<PlayerProjectile>().Inizialize(Quaternion.Euler(0, 0, -player.groundAngle) * Vector2.right, player.projectileSpawn.position, normalProjectileSpeed * 60);
        else
            projectile.GetComponent<PlayerProjectile>().Inizialize(Quaternion.Euler(0, 0, -player.groundAngle) * -Vector2.right, player.projectileSpawn.position, normalProjectileSpeed * 60);

        projectile.GetComponent<PlayerProjectile>().lifeTime = normalProjectileLifeTime;

        player.activateCurrentAbility = false;
        player.animator.SetBool("UsingCard", false);

        player.inputs.Player.Enable();

        readyToUse = false;

        player.StartCoroutine(Cooldown());
    }

    public override void Activate2(GameObject parent)
    {
        if (!readyToUse)
            return;

        if (parent.GetComponent<PlayerController>())
            player = parent.GetComponent<PlayerController>();

        if (!PlayerController.instance.grounded)
            return;
        
        player.animator.SetBool("UsingCard", true);
        player.animator.SetTrigger("ActivateCard");

        player.inputs.Player.Disable();

        player.StartCoroutine(WaitAndSpawnGuidedProjectile());

    }

    public override void Disactivate2(GameObject gameObject)
    {
        if (!readyToUse || instantietedPathCreator == null)
        {
            return;
        }

        player.animator.SetBool("UsingCard", false);

        player.StopCoroutine(DrawTimer());

        SpawnGuidedProjectile();

        player.inputs.Player.Enable();
    }

    private void ShotGuidedProjectile()
    {
        GameObject instantietedPathCreatorObj = Instantiate(prefabPathCreator, player.projectileSpawn.position, Quaternion.identity);
        
        instantietedPathCreator = instantietedPathCreatorObj.GetComponent<PathCreator>();
        instantietedPathCreator.projectileSpeed = guidedProjectileSpeed;
        instantietedPathCreator.audioSource.clip = drawPathProjectileSound;
        instantietedPathCreator.isDrawing = true;
        player.activateCurrentAbility = false;

        instantietedPathCreator.StartCoroutine(DrawTimer());
    }


    private void SpawnGuidedProjectile()
    {
        player.StartCoroutine(Cooldown());
        PlayerController.instance.GetComponent<AudioSourceGenerator>().PlaySound(shotProjectileSound);

        instantietedPathCreator.isDrawing = false;
        instantietedPathCreator.SpawnProjectile(prefabProjectile);
        instantietedPathCreator = null;

        readyToUse = false;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        readyToUse = true;
    }

    IEnumerator DrawTimer()
    {
        yield return new WaitForSeconds(timeToDraw);

        if (readyToUse && !instantietedPathCreator.instatietedProjectile)
        {
            SpawnGuidedProjectile();
            player.inputs.Player.Enable();
        }

        player.animator.SetBool("UsingCard", false);

        player.StopCoroutine(DrawTimer());
    }
    IEnumerator WaitAndSpawnGuidedProjectile( )
    {
        yield return new WaitUntil(() => player.activateCurrentAbility == true);
        ShotGuidedProjectile();
    }

    IEnumerator WaitAndSpawnNormalProjectile()
    {
        yield return new WaitUntil(() => player.activateCurrentAbility == true);
        ShotProjectile();
        PlayerController.instance.GetComponent<AudioSourceGenerator>().PlaySound(shotProjectileSound);
    }

    
}
