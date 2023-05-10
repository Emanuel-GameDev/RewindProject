using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
    [TaskCategory("Unity/Collider")]
    [TaskDescription("Check if the GameObject Collider is touching the target collider.")]
    public class TouchingColliders2D : Conditional
    {
        public SharedCollider2D targetCollider;

        public override TaskStatus OnUpdate()
        {
            if (gameObject.GetComponent<Collider2D>().IsTouching(targetCollider.Value))
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;  
            }
        }

        public override void OnReset()
        {
            targetCollider = null;
        }
    }
}



