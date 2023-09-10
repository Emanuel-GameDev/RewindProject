using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/RewindTime")]
public class RewindTimeAbility : Ability
{
    [SerializeField] float parryTime = 0.3f;
    private float elapsedTime = 0;
    bool started = false;
    private PlayerController player;

    public override void Activate1(GameObject parent)
    {
        if (TimelineManager.Instance.StartForwarding())
            isActive = true;
        else
            Parry(parent);
    }

    public override void Activate2(GameObject parent)
    {
        
        if (TimelineManager.Instance.StartRewinding())
            isActive = true;
        else
            Parry(parent);
    }

    public override void Activate3(GameObject parent)
    {
        if (TimelineManager.Instance.StartStopControlTimeline())
            isActive = true;
        else
            isActive = false;

    }

    public override void Disactivate1(GameObject gameObject)
    {
        TimelineManager.Instance.StopForwarding();
    }

    public override void Disactivate2(GameObject gameObject)
    {
        TimelineManager.Instance.StopRewinding();
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
            player.ActivateCardAnimation(this);
            player.StartCoroutine(StopAimation(parent));
            PubSub.Instance.Notify(EMessageType.ParryStart, this);
            isActive = true;
            started = true;
            elapsedTime = 0;
            player = parent.GetComponent<PlayerController>();
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
