using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Seek the target specified using the Unity NavMesh, but move only in the selected Coordinates")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class SelectiveSeek : NavMeshMovement
    {
        [Tooltip("The GameObject that the agent is seeking")]
        public SharedGameObject target;
        [Tooltip("If target is null then use the target position")]
        public SharedVector3 targetPosition;
        [Tooltip("If is True use the Target X Position as Destination")]
        public SharedBool followOnX;
        [Tooltip("If is True use the Target Y Position as Destination")]
        public SharedBool followOnY;
        [Tooltip("If is True use the Target Z Position as Destination")]
        public SharedBool followOnZ;
        [Tooltip("If is True use the don't move if the target is Null")]
        public SharedBool dontMoveIfTargetIsNull;

        public override void OnStart()
        {
            base.OnStart();

            SetDestination(Target());
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            if (HasArrived())
            {
                return TaskStatus.Success;
            }

            SetDestination(Target());

            return TaskStatus.Running;
        }

        // Return targetPosition if target is null
        private Vector3 Target()
        {
            if (target.Value != null)
            {
                Vector3 destination = target.Value.transform.position;

                if (!followOnX.Value)
                    destination.x = gameObject.transform.position.x;

                if (!followOnY.Value)
                    destination.y = gameObject.transform.position.y;

                if (!followOnZ.Value)
                    destination.z = gameObject.transform.position.z;

                return destination;
            }

            if (dontMoveIfTargetIsNull.Value)
            {
                return gameObject.transform.position;
            }


            return targetPosition.Value;
        }

        public override void OnReset()
        {
            base.OnReset();
            target = null;
            targetPosition = Vector3.zero;
            followOnX = true;
            followOnY = true;
            followOnZ = true;
            dontMoveIfTargetIsNull = true;
        }
    }
}