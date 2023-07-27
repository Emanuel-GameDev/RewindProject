using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class DeadZoneCause : Cause
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform respawnPos;

    private Vector2 _respawnPos;
    private GameObject objToRespawn;

    protected override void ActivateEffect()
    {
        if (objToRespawn == null)
        {
            Debug.LogError("Error: objToRespawn is null, probably there was a problem in OnTriggerEnter2D");
            return;
        }
        if (_respawnPos == null)
        {
            Debug.LogError("Error: respawnPos is null, you probably need to assign it");
            return;
        }

        effect?.Invoke();
    }

    public void Respawn()
    {
        objToRespawn.transform.position = _respawnPos;
    }

    private void OnValidate()
    {
        if (!GetComponent<PolygonCollider2D>().isTrigger)
            GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    protected override void Start()
    {
        base.Start();

        _respawnPos = new Vector2(respawnPos.position.x, respawnPos.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetLayer.value, 2f)))
        {
            objToRespawn = collision.gameObject;
            ActivateEffect();
        }
    }

    // No need for this
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(targetLayer.value, 2f)))
        {
            objToRespawn = null;
        }
    }
}
