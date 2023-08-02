using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/InvertGravity")]
public class InvertGravity : Ability
{
    [Tooltip("The layer that makes the ray stop and triggers the ability")]
    [SerializeField] LayerMask targetMask;
    [SerializeField] private float cooldown;

    private float lastActivationTime = 0f;

    private bool canActivate = true;

    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        RaycastHit2D hit;
        Vector3 rayDirection;
        isActive = true;

        if (PlayerController.instance.IsGravityDownward())
            rayDirection = Vector3.up; // Direzione verso l'alto
        else
            rayDirection = Vector3.down; // Direzione verso il basso

        hit = Physics2D.Raycast(parent.transform.position, rayDirection, Mathf.Infinity, targetMask);

        if (hit.collider != null)
        {
            // Il raycast ha colpito un oggetto nel layer "Ground"
            Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

            rBody.gravityScale = -rBody.gravityScale;
            parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);
        }

        isActive = false;
        canActivate = false;
        lastActivationTime = Time.time;
    }

    public override void UpdateAbility()
    {
        if (!canActivate && Time.time >= lastActivationTime + cooldown)
        {
            canActivate = true;
        }
    }
}

