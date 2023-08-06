using UnityEngine;

[CreateAssetMenu(menuName = "Ability/InvertGravity")]
public class InvertGravity : Ability
{
    [Tooltip("The layer that makes the ray stop and triggers the ability")]
    [SerializeField] private LayerMask targetMask;
    [Tooltip("distance of the ray used to check layer, set this to 0 in order to have an infinite ray")]
    [SerializeField] private float rayDistance;
    [SerializeField] private float cooldown;

    [SerializeField] private LayerMask layerToIgnore;

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

        int layerMask = Physics2D.AllLayers;
        layerMask &= ~(1 << layerToIgnore);

        if (rayDistance > 0)
            hit = Physics2D.Raycast(parent.transform.position, rayDirection, rayDistance, ~layerToIgnore);
        else
            hit = Physics2D.Raycast(parent.transform.position, rayDirection, Mathf.Infinity, ~layerToIgnore);


        if (hit.collider.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetMask.value, 2f)))
        {
            Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

            rBody.gravityScale = -rBody.gravityScale;
            parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);

        }


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

