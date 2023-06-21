using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipRotation : MonoBehaviour
{
    [SerializeField] List<Transform> objetcsToFlip;
    [SerializeField] Transform target;
    void Update()
    {
        if(transform.position.x > target.position.x )
        {
            foreach(Transform t in objetcsToFlip)
            {
                t.localRotation = new Quaternion(180, t.localRotation.y, t.localRotation.z, t.localRotation.w);
            }
        }
        else
        {
            foreach (Transform t in objetcsToFlip)
            {
                t.localRotation = new Quaternion(0, t.localRotation.y, t.localRotation.z, t.localRotation.w);
            }
        }
    }
}
