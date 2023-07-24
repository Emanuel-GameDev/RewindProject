using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Projectile")]

public class ProjectileAbility : Ability
{
    [SerializeField] GameObject prefabProjectile;
    [SerializeField] float projectileSpeed = 1;
    bool readyToUse = true;
    private float cooldown;

    public override void Activate1(GameObject parent)
    {
        if (!readyToUse)
            return;

        GameObject projectile = Instantiate(prefabProjectile, parent.transform.position, Quaternion.Euler(0, 0, PlayerController.instance.groundAngle));
        
        if(PlayerController.instance.bodySprite.transform.localScale.x >= 0)
            projectile.GetComponent<PlayerProjectile>().Inizialize(Quaternion.Euler(0,0,-PlayerController.instance.groundAngle) *  Vector2.right, PlayerController.instance.transform.position + Vector3.right, projectileSpeed);
        else
            projectile.GetComponent<PlayerProjectile>().Inizialize(Quaternion.Euler(0, 0, -PlayerController.instance.groundAngle) * -Vector2.right, PlayerController.instance.transform.position + Vector3.left, projectileSpeed);


        readyToUse = false;
        LevelManager.instance.StartCoroutine(Cooldown());
    }

    private void OnEnable()
    {
        readyToUse = true;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        readyToUse = true;
    }
}
