using System.Collections;
using UnityEngine;

public class TestEnemy : Enemy, IAttackable
{
	enum State
	{
		Idle,
		Wander,
		Hunt,
		Attack,
		EpicDeath,
		Knockback,
	}

	[SerializeField] float knockbackDuration;

	[SerializeField]
	float huntDistance;

	[SerializeField]
	float attackDistance;

	[SerializeField]
	float accelerationTime;

	[SerializeField]
	float stopTime;

	[SerializeField]
	public float maxSpeed;

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

	AudioSource audio;
	[SerializeField] AudioClip attackSound;

	Quaternion targetRotation;
	Vector3 hunt_playerOffset;
	Vector3 idle_targetPosition;
	float timer = 0;
	Vector3 startPosition;
	bool startedDeathFlashing = false;

	// Use this for initialization
	override protected void Awake()
	{
		base.Awake();

		health = 3;

		state = State.Idle;
		startPosition = transform.position;
		rbody = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		var eventsInvoker = animator.GetBehaviour<AnimationEventsInvoker>();
		eventsInvoker.stateEndEvent.AddListener(OnAnimationEnd);

		attack = GetComponentInChildren<AttackHitBox>();
	}

	protected override void Start()
	{
		base.Start();
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
		idle_targetPosition = startPosition + new Vector3(xy.x, 0, xy.y);
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
					WalkToPoint(player.position + hunt_playerOffset, maxSpeed);

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

			case State.Attack:
				if (squash.inSquash)
				{
					SetState(State.Hunt);
				}

				break;

			case State.EpicDeath:
				if (!startedDeathFlashing && (squash.isFlat || !squash.inSquash))
				{
					StartCoroutine(FlashAndDie());
				}
				break;

			case State.Knockback:
				print("a/b " + knockbackVector.magnitude + "/" +  knockbackDuration);
				float deacceleration = knockbackVector.magnitude / knockbackDuration;
				print("In knockback " + deacceleration);
				if (rbody.velocity.magnitude <= deacceleration * Time.fixedDeltaTime)
				{	
					print("Stop knockback");
					rbody.velocity = Vector3.zero;
					SetState(State.Idle);
				}
				else
				{
					rbody.AddForce(-rbody.velocity.normalized * deacceleration);
				}

				break;
		}

		if (rbody.velocity != Vector3.zero && state != State.Knockback)
		{
			Quaternion rot;
			if (state == State.Attack)
			{
				Vector3 meToPlayer = player.position - transform.position;
				meToPlayer.y = 0;
				rot = Quaternion.LookRotation(meToPlayer, Vector3.up);
			}
			else
			{
				rot = Quaternion.LookRotation(new Vector3(rbody.velocity.x, 0, rbody.velocity.z), Vector3.up);
			}
			rot *= Quaternion.Euler(0, -90, 0);
			rbody.rotation = Quaternion.SlerpUnclamped(rbody.rotation, rot, 0.1f);
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
				hunt_playerOffset = Random.insideUnitSphere * 1.0f;
				hunt_playerOffset.y = 0;
				break;
			case State.Attack:
				rbody.velocity = Vector3.zero;
				animator.Play("EnemyAttack");
				attack.Attack();
				//audio.PlayOneShot(attackSound);
				break;
			case State.EpicDeath:
				if (flammable)
					flammable.StopBurning();
				break;
			case State.Knockback:
				//print("Knockback: " + knockbackVector);
				rbody.velocity = knockbackVector;
				break;
		}
	}

	override public void TakeDamage(DamageParams args)
	{
		base.TakeDamage(args);
	}

	protected override void ExtraTakeDamage(DamageParams args)
	{
		base.ExtraTakeDamage(args);
		print("extra take damage");
		if (args.damageType == DamageType.Hit)
		{
			SetState(State.Knockback);
		}
	}

	override protected void Die()
	{
		SetState(State.EpicDeath);

	}

	IEnumerator FlashAndDie()
	{
		startedDeathFlashing = true;
		Renderer renderer = GetComponent<Renderer>();
		for (int i = 0; i < 6; i++)
		{
			renderer.enabled = false;
			yield return new WaitForSeconds(0.1f - i * 0.01f);
			renderer.enabled = true;
			yield return new WaitForSeconds(0.05f - i * 0.01f);
		}
		Destroy(gameObject);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, huntDistance);
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere(transform.position, wanderAreaRadius);
		Gizmos.DrawSphere(idle_targetPosition, 0.1f);
	}
}
