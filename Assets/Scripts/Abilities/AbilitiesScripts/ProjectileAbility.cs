using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Projectile")]

public class ProjectileAbility : Ability
{
    [SerializeField] GameObject prefabProjectile;
    bool readyToUse = true;
    private float cooldown;

    public override void Activate1(GameObject parent)
    {
        if (!readyToUse)
            return;

        Instantiate(prefabProjectile, parent.transform.position, Quaternion.identity);

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
