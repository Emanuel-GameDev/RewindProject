using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class InvertGravity : Ability
{
    [SerializeField] LayerMask targetMask;
    private float elapsedTime = 0;
    private EAbilityState state;
 

    public override void Activate1(GameObject parent)
    {
        if (state != EAbilityState.ready)
            return;
        
        RaycastHit2D hit;
        Vector3 rayDirection = Vector3.up; // Direzione verso l'alto

        hit = Physics2D.Raycast(parent.transform.position, rayDirection, Mathf.Infinity, targetMask);

        if (hit.collider != null)
        {
            // Il raycast ha colpito un oggetto nel layer "Ground"
            Rigidbody2D rBody = parent.GetComponent<Rigidbody2D>();

            rBody.gravityScale = -rBody.gravityScale;
            parent.transform.localScale = new Vector3(parent.transform.localScale.x, -parent.transform.localScale.y, parent.transform.localScale.z);
            state = EAbilityState.cooldown;
        }
    }

    public override void Start()
    {
        base.Start();
        elapsedTime = 0;
        state = EAbilityState.ready;
    }

    private void Update()
    {
        if(state == EAbilityState.cooldown)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > cooldownTime) 
            {
                state = EAbilityState.ready;
            }
        }
    }

}
