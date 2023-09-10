using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/InvertGravity")]
public class InvertGravity : Ability
{
    [Tooltip("distance of the ray used to check layer, set this to 0 in order to have an infinite ray")]
    [SerializeField, Min(0f)] private float rayDistance;
    [SerializeField] private AudioClip upClip;
    [SerializeField] private AudioClip downClip;

    private float lastActivationTime = 0f;
    private Animator animator;
    private bool canActivate = true;

    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        animator.SetBool("UsingCard", true);
        animator.SetTrigger("ActivateCard");

        parent.GetComponent<PlayerController>().StartCoroutine(WaitToInvert(parent));
    }

    private void Invert(GameObject parent)
    {
        RaycastHit2D[] hits;
        Vector3 rayDirection;
        isActive = true;
        //bool ignoreHit = false;
        LayerMask layerMask = LayerMask.GetMask("Ground");
        bool hitted = false;
        bool downGravity = PlayerController.instance.IsGravityDownward();

        if (downGravity)
            rayDirection = Vector3.up; // Direzione verso l'alto
        else
            rayDirection = Vector3.down; // Direzione verso il basso

        // Apply ray distance if needed
        if (rayDistance > 0)
            hits = Physics2D.RaycastAll(parent.transform.position, rayDirection, rayDistance);
        else
            hits = Physics2D.RaycastAll(parent.transform.position, rayDirection, Mathf.Infinity);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.layer == Mathf.RoundToInt(Mathf.Log(layerMask.value, 2f)))
            {
                hitted = true;
                break;
            }
        }

        if (!hitted) return;

        if (downGravity)
            parent.GetComponent<MainCharacter_SoundsGenerator>().PlaySound(upClip);
        else
            parent.GetComponent<MainCharacter_SoundsGenerator>().PlaySound(downClip);

        // Obj valid to activate ability
        Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

        rBody.gravityScale = -rBody.gravityScale;
        parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);

        parent.GetComponent<PlayerController>().activateCurrentAbility = false;
        parent.GetComponent<PlayerController>().animator.SetBool("UsingCard", false);

        isActive = false;
        canActivate = false;
        lastActivationTime = Time.time;
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);

        animator = picker.gameObject.GetComponent<Animator>();
    }

    public override void UpdateAbility()
    {
        if (!canActivate && Time.time >= lastActivationTime + cooldownTime)
        {
            canActivate = true;
        }
    }

    private IEnumerator WaitToInvert(GameObject parent)
    {
        yield return new WaitUntil(() => parent.GetComponent<PlayerController>().activateCurrentAbility == true);
        Invert(parent);
    }


}

