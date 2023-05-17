using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine.AI;

public class SeekWithJump : Action
{
	public SharedGameObject target;
    public SharedFloat maxJumpHeight = 5f;
    public SharedFloat maxJumpWidth = 5f;
    public SharedFloat jumpSpeed = 1f;
	public SharedFloat parabolaHeight = 2f;
    public SharedFloat searchRadius = 15f;
    public SharedFloat arriveDistance = 0.1f;
    public SharedFloat distanceFromPlatformToJump = 2.5f;
    public SharedFloat heightDifferenceTollerance = 1f;
    public SharedLayerMask layerMask; // layer degli oggetti da cercare
    public SharedVector2 raycastOrigins;

    private NavMeshAgent navMeshAgent;
    private List<Collider2D> nearColliders = new();
    private bool isJumping = false;
    private Vector3 pointToJump = Vector3.zero;
    private float lerp = 0f; // il valore di interpolazione per muovere l'oggetto lungo la parabola
    private Vector3 startJumpPoint = Vector3.zero;
    private Vector2 centerOfCollider;
    private Vector3 actualTargetPoint;
    private bool canReachTarget = true;
    private Collider2D nearestPlatform;
    private Vector2 raycastOrigin => new Vector2(transform.position.x + raycastOrigins.Value.x, transform.position.y + raycastOrigins.Value.y);

    




    public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}