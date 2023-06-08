using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTimeAbility : Ability
{
    public override void Activate1(GameObject parent)
    {
        TimelineManager.Instance.RewindTimeline();
    }

    public override void Activate2(GameObject parent)
    {
        TimelineManager.Instance.ForwardingTimeline();
    }

    public override void Activate3(GameObject parent)
    {
        TimelineManager.Instance.StartStopRewinding();
    }
}
