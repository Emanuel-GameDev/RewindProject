using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/InvertGravity")]
public class InvertGravity : Ability
{
    [Tooltip("The layers that the ray has to ignore")]
    [SerializeField] private List<LayerMask> layerToIgnore;
    [Tooltip("distance of the ray used to check layer, set this to 0 in order to have an infinite ray")]
    [SerializeField, Min(0f)] private float rayDistance;
    [SerializeField] private float cooldown;



    private float lastActivationTime = 0f;

    private bool canActivate = true;

    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        // TODO: Riguardare questo codice perché non è ottimale al massimo

        RaycastHit2D[] hit;
        Vector3 rayDirection;
        isActive = true;
        bool ignoreHit = false;

        if (PlayerController.instance.IsGravityDownward())
            rayDirection = Vector3.up; // Direzione verso l'alto
        else
            rayDirection = Vector3.down; // Direzione verso il basso

        // Apply ray distance if needed
        if (rayDistance > 0)
            hit = Physics2D.RaycastAll(parent.transform.position, rayDirection, rayDistance);
        else
            hit = Physics2D.RaycastAll(parent.transform.position, rayDirection, Mathf.Infinity);

        // Looping through objs hit to avoid objs in ignore layers
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider == null) continue;

            foreach (LayerMask mask in layerToIgnore)
            {
                if (hit[i].collider.gameObject.layer == Mathf.RoundToInt(Mathf.Log(mask.value, 2f)))
                {
                    ignoreHit = true;
                    break;
                }
            }

            if (ignoreHit)
            {
                ignoreHit = false;
                continue;
            }

            // Make sure to trigger abilty only with the first obj hit by skipping first two layer hit
            if (i > 2) continue;

            // Obj valid to activate ability
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

