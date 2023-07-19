using UnityEngine;
using UnityEngine.Rendering;

public class TimelineZoneChanger : MonoBehaviour
{
    [SerializeField] eZone zone;

    private Volume volume;

    private void Start()
    {
        if (transform.childCount > 0 && transform.GetChild(0) != null)
            volume = transform.GetChild(0).GetComponent<Volume>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PubSub.Instance.Notify(EMessageType.RewindZoneEntered, volume);

            TimelineManager.Instance.ChangeTimeline(zone);
            TimelineManager.Instance.SetCanUseRewind(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            TimelineManager.Instance.SetCanUseRewind(false);
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
