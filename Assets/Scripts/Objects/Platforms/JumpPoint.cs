using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    [SerializeField] bool isEndPoint = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
