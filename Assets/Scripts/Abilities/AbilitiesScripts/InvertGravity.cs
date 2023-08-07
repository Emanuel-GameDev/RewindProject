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

        RaycastHit2D[] hit;
        Vector3 rayDirection;
        isActive = true;
        bool ignoreHit = false;

        if (PlayerController.instance.IsGravityDownward())
            rayDirection = Vector3.up; // Direzione verso l'alto
        else
            rayDirection = Vector3.down; // Direzione verso il basso

        if (rayDistance > 0)
            hit = Physics2D.RaycastAll(parent.transform.position, rayDirection, rayDistance);
        else
            hit = Physics2D.RaycastAll(parent.transform.position, rayDirection, Mathf.Infinity);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider == null) continue;

            Debug.Log(hit[i].collider.gameObject.name);

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

            if (i > 2) continue;

            // Obj valid to activate ability
            Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

            rBody.gravityScale = -rBody.gravityScale;
            parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);
        }
        //if (hit.collider.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetMask.value, 2f)))
        //{
        //    Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

        //    rBody.gravityScale = -rBody.gravityScale;
        //    parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);

        //}


        // Starting from 1 in order to skip the ability holder
        //for (int i = 1; i < hit.Length; i++)
        //{
        //    if (hit[i].collider != null)
        //    {
        //        if (hit[i].collider.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetMask.value, 2f)))
        //        {
        //            // First obj hit was in the target layer
        //            Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

        //            rBody.gravityScale = -rBody.gravityScale;
        //            parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);

        //            Debug.Log(hit[i].collider.gameObject.name);

        //            break;
        //        }

        //        // First obj hit was not in the target layer
        //        break;
        //    }
        //}

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

