using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] Projectile prefab;

    static ProjectilePool _instance;
    static public ProjectilePool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ProjectilePool>();
            }
            return _instance;
        }
    }

    Queue<Projectile> pool;

    private void Start()
    {
        pool = new Queue<Projectile>();
    }

    public Projectile GetProjectile()
    {
        Projectile projectile;
        if (pool.Count > 0)
            projectile = pool.Dequeue();
        else
        {
            projectile = Instantiate(prefab).GetComponent<Projectile>();
        }

        projectile.transform.SetParent(null);

        return projectile;
    }

    public void DismissProjectile(Projectile projectile)
    {
        pool.Enqueue(projectile);
        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(transform);
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
}
