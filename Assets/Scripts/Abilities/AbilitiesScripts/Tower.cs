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

    // DEBUG
    private Vector2 debugPos;
    private float debugRad;

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

    public GameObject GetContactPoint(Vector3 pos, bool down)
    {
        RaycastHit2D[] hits;

        if (down)
            hits = Physics2D.RaycastAll(pos, Vector2.down);
        else
            hits = Physics2D.RaycastAll(pos, Vector2.up);

        LayerMask mask = LayerMask.GetMask("Player");

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.layer != Mathf.RoundToInt(Mathf.Log(mask.value, 2f)))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    public bool CanBeActivated(Vector3 pos, float radius, LayerMask mask)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, mask);
        debugPos = pos;
        debugRad = radius;

        // Verifica se ci sono colliders nella sfera
        if (colliders.Length > 0)
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(debugPos, debugRad);
    }

    public void Activate(SummonTower activator, Vector2 pos, float rot, bool mirrored)
    {
        parent = activator;
        transform.position = pos;

        if (mirrored)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, rot));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rot));

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

            if (collision.gameObject.layer == LayerMask.NameToLayer("projectile"))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void PlayDismissAudio()
    {
        parent.DismissAudio();
    }

    public void Dismiss()
    {
        hp = maxHp;
        parent.StartCooldown();
    }
}
