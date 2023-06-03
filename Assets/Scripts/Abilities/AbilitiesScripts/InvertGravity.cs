using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertGravity : Ability
{
    [SerializeField] LayerMask targetMask;

    public override void Activate(GameObject parent)
    {
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
    }

    public override void Start()
    {
        base.Start();
    }
}
