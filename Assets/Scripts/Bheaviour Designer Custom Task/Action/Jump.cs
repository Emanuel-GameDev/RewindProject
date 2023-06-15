using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Movement")]
public class Jump : Action
{
    public SharedVector3 startJumpPoint;
    public SharedVector3 pointToJump;
	public SharedFloat arriveDistance;
    public SharedFloat maxParabolaHeight;
    public SharedFloat jumpSpeed;
    public SharedBool isJumping;

    private float lerp;

	public override TaskStatus OnUpdate()
	{
        if (isJumping.Value)
        {
            isJumping.Value = JumpToPoint(startJumpPoint.Value, pointToJump.Value, maxParabolaHeight.Value);

            if (isJumping.Value)
                return TaskStatus.Running;
            else
                return TaskStatus.Success;
        }
        else
        {
            lerp = 0;
            return TaskStatus.Failure;
        }
	}

    private bool JumpToPoint(Vector3 startJumpPoint, Vector3 pointToJump, float maxYJump)
    {
        lerp += jumpSpeed.Value * Time.deltaTime;

        if (lerp > 1f)
        {
            lerp = 1f;
        }

        float x = Mathf.Lerp(startJumpPoint.x, pointToJump.x, lerp);
        float y = Mathf.Lerp(startJumpPoint.y, pointToJump.y, lerp) + maxYJump * Mathf.Sin(Mathf.Lerp(0f, Mathf.PI, lerp));

        transform.position = new Vector3(x, y, 0);

        if (Vector2.Distance(pointToJump, transform.position) < arriveDistance.Value)
        {
            return false;
        }
        else
            return true;
    }

    public override void OnReset()
    {
        pointToJump.Value = Vector3.zero;
        maxParabolaHeight.Value = 5f;
        arriveDistance.Value = 0.5f;
        jumpSpeed.Value = 5f;
        startJumpPoint.Value = Vector3.zero;
        isJumping.Value = false;

        lerp = 0f;
    }

}