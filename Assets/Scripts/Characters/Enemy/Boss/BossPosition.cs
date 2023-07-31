using BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePosition { Top, Bottom }

public class BossPosition : MonoBehaviour
{
    [SerializeField] ePosition position;

    [SerializeField] Transform oppositePosition;

    public ePosition GetPosition()
    {
        return position;
    }

    public Transform GetOppositePosition()
    {
        return oppositePosition;
    }
}
