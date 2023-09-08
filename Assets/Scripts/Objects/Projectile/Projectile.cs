using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] public  float lifeTime;
    [SerializeField] protected LayerMask targetLayers;
    protected float elapsedTime;
    protected private Vector2 direction;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D body;
    protected Collider2D coll;
    protected Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        body.isKinematic = true;
    }

    protected virtual void Update()
    {
        body.velocity = direction * speed * Time.deltaTime;

        if (elapsedTime >= lifeTime)
            StopAndExplode();

        elapsedTime += Time.deltaTime;
    }

    public virtual void Dismiss()
    {
        ProjectilePool.Instance.DismissProjectile(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, targetLayers))
        {
            if (!collision.isTrigger)
            {
                StopAndExplode();
            }
        } 
    }

    protected void StopAndExplode()
    {
        coll.enabled = false;
        speed = 0;
        animator.SetTrigger("Explode");
    }

    public virtual void Inizialize(Vector2 direction, Vector2 position, float speed)
    {
        this.direction = direction;
        if(direction == Vector2.left)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
        this.speed = speed;
        transform.position = position;
        elapsedTime = 0;
        gameObject.SetActive(true);
    }

    protected bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        // Converte la LayerMask in un intero bit a bit
        int layerMaskValue = layerMask.value;

        // Controlla se il bit corrispondente alla layer dell'oggetto è attivo
        return (layerMaskValue & (1 << layer)) != 0;
    }

}
