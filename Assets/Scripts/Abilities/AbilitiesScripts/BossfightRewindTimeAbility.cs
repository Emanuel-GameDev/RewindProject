using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/BossRewindTime")]
public class BossfightRewindTimeAbility : Ability
{
    public override void Activate1(GameObject parent)
    {
        PubSub.Instance.Notify(EMessageType.TimeRewindStart, this);
    }

    public override void Activate2(GameObject parent)
    {
        PubSub.Instance.Notify(EMessageType.TimeRewindStart, this);
    }

    public override void Activate3(GameObject parent)
    {
        PubSub.Instance.Notify(EMessageType.TimeRewindStart, this);
    }

    public override void Disactivate1(GameObject gameObject)
    {
        
    }

    public override void Disactivate2(GameObject gameObject)
    {
        
    }

}
