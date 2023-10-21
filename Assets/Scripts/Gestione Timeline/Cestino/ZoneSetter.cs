using System;
using UnityEngine;

public class ZoneSetter : MonoBehaviour
{
    float size = 1000;
    
    private void OnDrawGizmos()
    {
        int i = 0;
        foreach (Transform t in transform)
        {
            Vector3 top = new Vector3(t.position.x, t.position.y + size/2);
            Vector3 bottom = new Vector3(t.position.x, t.position.y - size/2);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(top, bottom);
            t.gameObject.name = "Zone #" + (eZone)i;
            i++;
        }
    }

    private void Awake()
    {
        int i=0;
        foreach (Transform t in transform)
        {
            if (i != 0)
            {
                Debug.Log("i not 0");
                GameObject go1 = CreateTriggers(i - 1, t);
                go1.transform.position = t.position + Vector3.left;
            }

            GameObject go2 = CreateTriggers(i,t);
            go2.transform.position = t.position + Vector3.right;

            Debug.Log(i);
            i++;
        }
    }

    private GameObject CreateTriggers(int i, Transform t)
    {
        GameObject go = new GameObject("Zone #" + (eZone)(i) + " Setter");
        go.AddComponent<TimelineZoneChanger>();
        go.GetComponent<TimelineZoneChanger>().SetZone((eZone)(i));
        go.AddComponent<BoxCollider2D>();
        go.GetComponent<BoxCollider2D>().isTrigger = true;
        go.transform.localScale = new Vector3(1, size, 1);
        go.transform.SetParent(t);
        return go;
    }
}
