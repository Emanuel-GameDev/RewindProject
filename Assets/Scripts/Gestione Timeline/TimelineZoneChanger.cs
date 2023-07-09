using UnityEngine;

public class TimelineZoneChanger : MonoBehaviour
{
    [Tooltip("Imposta la zona di riferimento")]
    [SerializeField] eZone zone;
    [Tooltip("Se spuntato riproduce la Timeline nel momento in cui il player entra nella zona solo la prima volta")]
    [SerializeField] bool playOnEnter;
    [Tooltip("Se spuntato la Timeline viene impostata per partire già alla fine")]
    [SerializeField] bool startAtEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
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

    private void Start()
    {
        if (startAtEnd)
        {
            TimelineManager.Instance.SetAtEnd(zone);
        }
    }

}
