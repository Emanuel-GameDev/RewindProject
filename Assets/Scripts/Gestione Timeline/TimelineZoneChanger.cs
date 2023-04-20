using UnityEngine;

public class TimelineZoneChanger : MonoBehaviour
{
    [SerializeField] eZone zone;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TestBasicMovement>())
        {
            TimelineManager.Instance.ChangeTimeline(zone);
        }
    }

    public void SetZone(eZone newZone)
    {
        zone = newZone;
    }

    private void OnDrawGizmos()
    {
        gameObject.name = "Zone #" + zone;
    }

}
