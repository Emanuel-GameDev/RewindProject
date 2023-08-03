using UnityEngine;

[CreateAssetMenu(menuName = "Ability/RewindTime")]
public class RewindTimeAbility : Ability
{
    public override void Activate1(GameObject parent)
    {
        isActive = true;

        TimelineManager.Instance.StartForwarding();
    }

    public override void Activate2(GameObject parent)
    {
        isActive = true;

        TimelineManager.Instance.StartRewinding();
    }

    public override void Activate3(GameObject parent)
    {
        TimelineManager.Instance.StartStopControlTimeline();

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
