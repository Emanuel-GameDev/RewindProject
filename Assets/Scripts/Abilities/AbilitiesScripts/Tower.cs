using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Animator animator;
    private int maxHp;
    private int hp;
    private LayerMask collisionMask;
    private SummonTower parent;


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

    public bool CanBeActivated()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return false;

        return true;
    }

    public void Activate(SummonTower activator, Vector2 pos, bool mirrored)
    {
        parent = activator;
        transform.position = pos;

        if (mirrored)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);

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
        parent.StartCooldown();
    }
}
