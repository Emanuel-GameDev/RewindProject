using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/BossRewindTime")]
public class BossfightRewindTimeAbility : Ability
{
    [SerializeField] float parryTime;
    private float elapsedTime = 0;
    bool started = false;
    
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
            parent.GetComponent<PlayerController>().ActivateCardAnimation(this);
            parent.GetComponent<PlayerController>().StartCoroutine(StopAimation(parent));
        }
    }

    IEnumerator StopAimation(GameObject parent)
    {
        yield return new WaitForSeconds(0.01f);
        parent.GetComponent<PlayerController>().DeactivateCardAnimation(this);
    }

}
