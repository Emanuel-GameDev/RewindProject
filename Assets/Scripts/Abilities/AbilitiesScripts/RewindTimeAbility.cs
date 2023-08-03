using UnityEngine;

[CreateAssetMenu(menuName = "Ability/RewindTime")]
public class RewindTimeAbility : Ability
{
    public override void Activate1(GameObject parent)
    {
        isActive = true;
        Debug.Log("activate 1");
        TimelineManager.Instance.StartForwarding();
    }

    public override void Activate2(GameObject parent)
    {
        isActive = true;
        Debug.Log("activate 2");
        TimelineManager.Instance.StartRewinding();
    }

    public override void Activate3(GameObject parent)
    {
        Debug.Log("activate 3");
        TimelineManager.Instance.StartStopControlTimeline();
    }

    public override void Disactivate1(GameObject gameObject)
    {
        Debug.Log("disactivate 1 ");
        TimelineManager.Instance.StopForwarding();
    }

    public override void Disactivate2(GameObject gameObject)
    {
        TimelineManager.Instance.StopRewinding();
        Debug.Log("disactivate 2 ");

        isActive = false;
    }

}
