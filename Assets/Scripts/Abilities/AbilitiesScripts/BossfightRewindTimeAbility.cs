using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/BossRewindTime")]
public class BossfightRewindTimeAbility : Ability
{
    [SerializeField] float parryTime;
    private float elapsedTime = 0;
    bool started = false;
    private PlayerController player;

    public override void Activate1(GameObject parent)
    {
        Parry(parent);
    }

    public override void Activate2(GameObject parent)
    {
        Activate1(parent);
    }

    public override void Activate3(GameObject parent)
    {
        Activate1(parent);
    }

    public override void UpdateAbility()
    {
        if (isActive)
        {
            elapsedTime += Time.deltaTime;

            if (started && elapsedTime > parryTime)
            {
                PubSub.Instance.Notify(EMessageType.ParryStop, this);
                started = false;
            }

            if (elapsedTime > (cooldownTime + parryTime))
            {
                isActive = false;
            }
        }
    }

    private void Parry(GameObject parent)
    {
        if (!isActive)
        {
            PubSub.Instance.Notify(EMessageType.ParryStart, this);
            isActive = true;
            started = true;
            elapsedTime = 0;
            player = parent.GetComponent<PlayerController>();
            player.ActivateCardAnimation(this);
            player.StartCoroutine(StopAimation(parent));
            player.inputs.Player.Disable();
        }
    }

    IEnumerator StopAimation(GameObject parent)
    {
        yield return new WaitForSeconds(0.4f);
        player.DeactivateCardAnimation(this);
        player.inputs.Player.Enable();
    }

}
