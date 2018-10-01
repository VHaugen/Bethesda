using UnityEngine;

public class TestEnemy : Enemy
{
	enum State
	{
		Idle,
		Wander,
		Hunt,
		Attack,
	}

	[SerializeField]
	float huntDistance;

	[SerializeField]
	float attackDistance;

	[SerializeField]
	float accelerationTime;

	[SerializeField]
	float stopTime;

	[SerializeField]
	float maxSpeed;

	[SerializeField]
	float wanderSpeed;

	[SerializeField]
	float wanderAreaRadius;

	[SerializeField]
	State state;

	[SerializeField]
	float idleDurationMin;

	[SerializeField]
	float idleDurationMax;

	Transform player;
	Rigidbody rbody;
	Animator animator;
	AttackHitBox attack;

	Vector3 idle_targetPosition;
	float timer = 0;

	// Use this for initialization
	void Awake()
	{
		state = State.Idle;
		rbody = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		var eventsInvoker = animator.GetBehaviour<AnimationEventsInvoker>();
		eventsInvoker.stateEndEvent.AddListener(OnAnimationEnd);

		attack = GetComponentInChildren<AttackHitBox>();
	}

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	float GetDistanceToPlayer()
	{
		Vector3 meToPlayer = (player.position - transform.position);
		meToPlayer.y = 0;
		return meToPlayer.magnitude;
	}

	void RandomizeNewTargetPosition()
	{
		Vector2 xy = Random.insideUnitCircle * wanderAreaRadius;
		idle_targetPosition = new Vector3(xy.x, transform.position.y, xy.y);
	}

	void WalkToPoint(Vector3 targetPosition, float maxSpeed)
	{
		Vector3 dir = (targetPosition - transform.position).normalized;
		dir.y = 0;
		float acceleration = maxSpeed / accelerationTime;
		rbody.AddForce(dir * acceleration, ForceMode.Acceleration);
		if (rbody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
		{
			rbody.velocity = rbody.velocity.normalized * maxSpeed;
		}
	}

	void OnAnimationEnd(AnimatorStateInfo stateInfo)
	{
		switch (state)
		{
			case State.Attack:
				if (stateInfo.IsName("EnemyAttack"))
				{
					SetState(State.Hunt);
				}
				break;
			default:
				break;
		}
	}

	void FixedUpdate()
	{
		switch (state)
		{
			case State.Wander:
				if (GetDistanceToPlayer() <= huntDistance)
				{
					SetState(State.Hunt);
					break;
				}


				WalkToPoint(idle_targetPosition, wanderSpeed);

				if ((idle_targetPosition - transform.position).magnitude <= attackDistance)
				{
					SetState(State.Idle);
				}

				break;

			case State.Idle:
				if (GetDistanceToPlayer() <= huntDistance)
				{
					SetState(State.Hunt);
					break;
				}

				float retardation = maxSpeed / stopTime;
				if (rbody.velocity.magnitude < retardation * Time.fixedDeltaTime)
					rbody.velocity = Vector3.zero;
				rbody.AddForce(-rbody.velocity.normalized * retardation, ForceMode.Acceleration);

				timer -= Time.deltaTime;

				if (timer <= 0)
				{
					SetState(State.Wander);
				}

				break;

			case State.Hunt:
				float distance = GetDistanceToPlayer();
				if (distance > attackDistance)
				{
					WalkToPoint(player.position, maxSpeed);

					if (distance > huntDistance)
					{
						SetState(State.Idle);
					}
				}
				else
				{
					SetState(State.Attack);
				}
				break;
		}

		if (rbody.velocity != Vector3.zero)
		{
			rbody.MoveRotation(Quaternion.LookRotation(rbody.velocity, Vector3.up));
		}
	}

	void SetState(State state)
	{
		this.state = state;

		switch (state)
		{
			case State.Idle:
				timer = Random.Range(idleDurationMin, idleDurationMax);
				break;
			case State.Wander:
				RandomizeNewTargetPosition();
				break;
			case State.Hunt:
				break;
			case State.Attack:
				rbody.velocity = Vector3.zero;
				animator.Play("EnemyAttack");
				attack.Attack();
				break;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, huntDistance);
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere(Vector3.zero, wanderAreaRadius);
		Gizmos.DrawSphere(idle_targetPosition, 0.1f);
	}
}
