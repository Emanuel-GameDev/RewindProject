using UnityEngine;
using UnityEngine.Rendering;

public class TimelineZoneChanger : MonoBehaviour
{
    [Tooltip("Imposta la zona di riferimento")]
    [SerializeField] eZone zone;
    [Tooltip("Se spuntato riproduce la Timeline nel momento in cui il player entra nella zona solo la prima volta")]
    [SerializeField] bool playOnEnter;
    [Tooltip("Se spuntato la Timeline viene impostata per partire gi� alla fine")]
    [SerializeField] bool startAtEnd;

    private Volume volume;

    private void Start()
    {
        // Aggiunto da Manu
        if (transform.childCount > 0 && transform.GetChild(0) != null)
            volume = transform.GetChild(0).GetComponent<Volume>();

        if (startAtEnd)
        {
            TimelineManager.Instance.SetAtEnd(zone);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            // Aggiunto da Manu
            PubSub.Instance.Notify(EMessageType.RewindZoneEntered, volume);

            TimelineManager.Instance.ChangeTimeline(zone);

            if (playOnEnter)
            {
                TimelineManager.Instance.PlayCurrentTimeline();
                playOnEnter = false;
            }

            TimelineManager.Instance.SetCanUseRewind(true);
        }
        else if (collision.GetComponent<EnemyThree>()) 
        {
            collision.GetComponent<EnemyThree>().SetMoovingZone(zone);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            TimelineManager.Instance.SetCanUseRewind(false);
        }
        else if (collision.GetComponent<EnemyThree>())
        {
            collision.GetComponent<EnemyThree>().Despawn(zone);
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
