using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();

        if (controller == null) return;

        controller.canDoubleJump = true;
        gameObject.SetActive(false);
    }

    public override void Start()
    {
        base.Start();
    }
}
