using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moire : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform focalPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<PlayerController>())
            return;

        animator.SetBool("InSight", true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<PlayerController>())
            return;


        float rad = Mathf.Abs(Mathf.Atan2(focalPoint.position.y - PlayerController.instance.transform.position.y, focalPoint.position.x - PlayerController.instance.transform.position.x) * 180 / Mathf.PI);

        if (rad <= 50)
            animator.SetFloat("SightPosition", 0);
        else if(rad <= 75)
            animator.SetFloat("SightPosition", 0.25f);
        else if (rad <= 100)
            animator.SetFloat("SightPosition", 0.5f);
        else if (rad <= 125)
            animator.SetFloat("SightPosition", 0.75f);
        else
            animator.SetFloat("SightPosition", 1);

    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<PlayerController>())
            return;

        animator.SetBool("InSight", false);
    }

}
