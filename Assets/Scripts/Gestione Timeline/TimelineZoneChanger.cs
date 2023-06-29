using UnityEngine;

public class TimelineZoneChanger : MonoBehaviour
{
    [SerializeField] eZone zone;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            TimelineManager.Instance.ChangeTimeline(zone);
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
