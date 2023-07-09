using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/InvertGravity")]
public class InvertGravity : Ability
{
    [SerializeField] LayerMask targetMask;
    //private float elapsedTime = 0;
    //private EAbilityState state = EAbilityState.ready;
    bool readyToUse = true;

    public override void Activate1(GameObject parent)
    {
        if (!readyToUse)
            return;
        
        RaycastHit2D hit;
        Vector3 rayDirection;

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
            readyToUse = false;
            LevelManager.instance.StartCoroutine(Cooldown());
        }


    }

    private void OnEnable()
    {
        readyToUse = true;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        readyToUse = true;
    }

    //private void Update()
    //{
    //    if(state == EAbilityState.cooldown)
    //    {
    //        elapsedTime += Time.deltaTime;

    //        if (elapsedTime > cooldownTime) 
    //        {
    //            state = EAbilityState.ready;
    //        }
    //    }
    //}

}
