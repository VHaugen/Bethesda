﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class HuntingEnemy : MonoBehaviour
{
	enum State
	{
		Idle,
		Wander,
		HuntSurround,
		Hunt,
		Attack,
		EpicDeath,
		Knockback,
	}

	public float maxSpeed;

	[SerializeField] float huntDistance;
	[SerializeField] float attackDistance;
	[SerializeField] float accelerationTime;
	[SerializeField] float stopTime;
	[SerializeField] float wanderSpeed;
	[SerializeField] float wanderAreaRadius;
	[SerializeField] float surroundRadius = 1.0f;
	[SerializeField] float idleDurationMin;
	[SerializeField] float idleDurationMax;
	[SerializeField] Vector3 rotationAdd;

	[SerializeField] AudioClip attackSound;

	Quaternion targetRotation;
	Vector3 hunt_playerOffset;
	Vector3 idle_targetPosition;
	float timer = 0;
	Vector3 startPosition;

	State state;
	Transform player;
	Rigidbody rbody;
	Animator animator;
	AttackHitBox attack;
	AudioSource sound;
	EnemyHealth healthComp;
	SquashEffect squash;

	private void Awake()
	{
		state = State.Idle;
		startPosition = transform.position;
		rbody = GetComponent<Rigidbody>();
		sound = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		var eventsInvoker = animator.GetBehaviour<AnimationEventsInvoker>();
		eventsInvoker.stateEndEvent.AddListener(OnAnimationEnd);
		squash = GetComponent<SquashEffect>();
		attack = GetComponentInChildren<AttackHitBox>();
		healthComp = GetComponent<EnemyHealth>();
	}

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		healthComp.TakeDamageEvent += OnTakeDamage;
		healthComp.DieEvent += OnDie;
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
					SetState(State.HuntSurround);
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
					SetState(State.HuntSurround);
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
					SetState(State.HuntSurround);
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

			case State.HuntSurround:
				{
					Vector3 meToTarget = (player.position + hunt_playerOffset) - transform.position;
					meToTarget.y = 0;
					float distance = meToTarget.magnitude;
					if (distance > attackDistance)
					{
						WalkToPoint(player.position + hunt_playerOffset, maxSpeed);
					}
					else
					{
						SetState(State.Hunt);
					}
					float distanceToPlayer = GetDistanceToPlayer();
					if (distanceToPlayer > huntDistance)
					{
						SetState(State.Idle);
					}
					else if (distanceToPlayer < attackDistance)
					{
						SetState(State.Attack);
					}

					break;
				}

			case State.Hunt:
				{
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
				}

			case State.Attack:
				if (squash.inSquash)
				{
					SetState(State.HuntSurround);
				}

				break;

			case State.EpicDeath:
				break;

			case State.Knockback:
				print("a/b " + healthComp.knockbackVector.magnitude + "/" + healthComp.knockbackDuration);
				float deacceleration = healthComp.knockbackVector.magnitude / healthComp.knockbackDuration;
				print("In knockback " + deacceleration);
				if (rbody.velocity.magnitude <= deacceleration * Time.fixedDeltaTime)
				{
					print("Stop knockback");
					if (healthComp.health > 0)
					{
						rbody.velocity = Vector3.zero;
						SetState(State.Idle);
					}
					else
					{
						SetState(State.EpicDeath);
					}
				}
				else
				{
					rbody.AddForce(-rbody.velocity.normalized * deacceleration);
				}

				break;
		}

		if (rbody.velocity != Vector3.zero && state != State.Knockback && state != State.EpicDeath)
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
			rot *= Quaternion.Euler(rotationAdd);
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
			case State.HuntSurround:
				hunt_playerOffset = Random.insideUnitSphere * surroundRadius;
				hunt_playerOffset.y = 0;
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
				break;
			case State.Knockback:
				//print("Knockback: " + knockbackVector);
				rbody.velocity = healthComp.knockbackVector;
				break;
		}
	}

	void OnTakeDamage(DamageParams args)
	{
		if (args.damageType == DamageType.Hit)
		{
			SetState(State.Knockback);
		}
	}

	void OnDie()
	{
		if (state != State.Knockback)
		{
			SetState(State.EpicDeath);
		}
	}
}
