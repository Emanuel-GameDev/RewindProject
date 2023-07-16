using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class InvertGravity : Ability
{
    [SerializeField] LayerMask targetMask;
    [SerializeField] private float cooldown;

    private bool canActivate = true;
 
    public override void Activate1(GameObject parent)
    {
        if (!canActivate) return;

        RaycastHit2D hit;
        Vector3 rayDirection = Vector3.up; // Direzione verso l'alto

        hit = Physics2D.Raycast(parent.transform.position, rayDirection, Mathf.Infinity, targetMask);

        if (hit.collider != null)
        {
            // Il raycast ha colpito un oggetto nel layer "Ground"
            Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

            rBody.gravityScale = -rBody.gravityScale;
            parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);
        }

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        canActivate = false;

        yield return new WaitForSeconds(cooldown);

        canActivate = true;
    }


    public override void Start()
    {
        base.Start();
    }

}
