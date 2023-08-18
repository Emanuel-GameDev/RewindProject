using UnityEngine;

[CreateAssetMenu(menuName = "Ability/MeleeAttack")]
public class MeleeAttack : Ability
{
    public override void Activate1(GameObject parent)
    {
        if (!PlayerController.instance.canAttack)
            return;

            PlayerController.instance.Attack();
    }
}
