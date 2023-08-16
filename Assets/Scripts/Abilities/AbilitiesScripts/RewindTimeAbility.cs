using UnityEngine;

[CreateAssetMenu(menuName = "Ability/RewindTime")]
public class RewindTimeAbility : Ability
{
    public override void Activate1(GameObject parent)
    {
        if (TimelineManager.Instance.StartForwarding())
            isActive = true;
    }

    public override void Activate2(GameObject parent)
    {
        if (TimelineManager.Instance.StartRewinding())
            isActive = true;
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

}
