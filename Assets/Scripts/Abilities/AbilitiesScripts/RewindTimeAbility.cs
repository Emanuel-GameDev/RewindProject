using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTimeAbility : Ability
{
    public override void Activate1(GameObject parent)
    {
        TimelineManager.Instance.StartForwarding();
    }

    public override void Activate2(GameObject parent)
    {
        TimelineManager.Instance.StartRewinding();
    }

    public override void Activate3(GameObject parent)
    {
        TimelineManager.Instance.StartStopControlTimeline();
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
