using BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eVerticalPosition { Top, Bottom }
public enum eHorizontalPosition { Left, Center, Right }

public class BossPosition : MonoBehaviour
{
    [SerializeField] eVerticalPosition verticalPosition;
    [SerializeField] eHorizontalPosition horizontalPosition;

    [SerializeField] Transform oppositePosition;

    public eVerticalPosition GetVerticalPosition()
    {
        return verticalPosition;
    }
    public eHorizontalPosition GetHorizontalPosition()
    {
        return horizontalPosition;
    }

    public Transform GetOppositePosition()
    {
        return oppositePosition;
    }
}
