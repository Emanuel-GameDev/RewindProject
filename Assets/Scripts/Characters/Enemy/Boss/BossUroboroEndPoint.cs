using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUroboroEndPoint : MonoBehaviour
{
    GameObject targetUroboro;
    EnemyThree uroboro;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        uroboro = collision.GetComponent<EnemyThree>();
        if (uroboro == null )
            collision.GetComponentInParent<EnemyThree>();
        if (uroboro != null)
        {
            if (uroboro.gameObject == targetUroboro)
            {
                uroboro.StopChase();
            }
        }
            
    }

    public void Set(GameObject target, Vector2 position)
    {
        targetUroboro = target;
        transform.position = position;   
    }
}
