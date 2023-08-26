using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Animator animator;
    private int maxHp;
    private int hp;
    private LayerMask collisionMask;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(int hp, LayerMask mask)
    {
        maxHp = hp;
        this.hp = maxHp;

        collisionMask = mask;
    }

    public void Activate(Vector2 pos)
    {
        transform.position = pos;

        animator.SetTrigger("Activated");

        if (animator.GetBool("Destroyed"))
            animator.SetBool("Destroyed", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (((1 << collision.gameObject.layer) & collisionMask) != 0)
        {
            hp -= 1;

            if (hp == 0)
            {
                animator.SetBool("Destroyed", true);
                Dismiss();
            }
        }
    }

    public void Dismiss()
    {
        hp = maxHp;
    }
}
