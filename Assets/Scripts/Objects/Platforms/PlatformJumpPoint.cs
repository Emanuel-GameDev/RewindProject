using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformJumpPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        int i = 1;
        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.parent == transform)
            {
                t.name = "Link#" + i.ToString();
                int j = 1;
                foreach(Transform p in t.GetComponentsInChildren<Transform>())
                {
                    if (p.parent == t.transform)
                    {
                        p.name = i.ToString() + "-" + j.ToString();
                        j++;
                    }
                }
                i++;
            }
            
        }
    }
}
