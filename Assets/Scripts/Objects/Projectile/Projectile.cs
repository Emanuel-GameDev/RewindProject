using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    float elapsedTime;
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;
    }

    void Update()
    {
        body.velocity = direction * speed * Time.deltaTime;

        if (elapsedTime >= lifeTime)
            Dismiss();

        elapsedTime += Time.deltaTime;
    }

    public void Dismiss()
    {
        ProjectilePool.Instance.DismissProjectile(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        Dismiss();
    }

    public void Inizialize(Vector2 direction, Vector2 position, float speed)
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
}
