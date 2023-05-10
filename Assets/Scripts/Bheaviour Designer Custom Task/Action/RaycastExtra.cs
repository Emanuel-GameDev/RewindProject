using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D
{
    [TaskCategory("Unity/Physics2D")]
    [TaskDescription("Check the distance from borders of given Collider")]
    public class RaycastExtra : Raycast
    {
        [SharedRequired]
        [Tooltip("Stores the last hit object of the raycast.")]
        public SharedGameObject storeLastHitObject;

        public override TaskStatus OnUpdate()
        {
            storeLastHitObject.Value = storeHitObject.Value;
            return base.OnUpdate();
        }

    }
}
