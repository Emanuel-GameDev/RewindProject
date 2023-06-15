using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
    [TaskCategory("Unity/Collider")]
    [TaskDescription("Check if the GameObject Collider is touching the target collider.")]
    public class TouchingGameObject2D : Conditional
    {
        public SharedGameObject targetGameObject;

        public override TaskStatus OnUpdate()
        {
            if (gameObject.GetComponent<Collider2D>().IsTouching(targetGameObject.Value.GetComponent<Collider2D>()))
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
            targetGameObject = null;
        }
    }
}



