using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IAttackable
{
	[SerializeField]
	GameObject fire;

	protected float health;
	protected float fireDamagePerSecond;

	protected float fireStatus = -1;
	float fireSpreadTimer = 0;
	protected float poisonStatus = -1;
	protected float iceStatus = -1;
	protected float lightningStatus = -1;
	protected SquashEffect squash;


	virtual protected void Awake()
	{
		squash = GetComponent<SquashEffect>();
	}

	virtual protected void Update()
	{
		HandleFireStatusEffect();
	}

	virtual protected void HandleFireStatusEffect()
	{
		const float spreadInterval = 0.4f;
		const float fireSpreadRadius = 3.0f;

		if (fireStatus > 0)
		{ 
			fireStatus -= Time.deltaTime;

			if (fireStatus <= 0)
			{
				fireStatus = -1;
				fire.SetActive(false);
			}

			fireSpreadTimer += Time.deltaTime;
			if (fireSpreadTimer > spreadInterval)
			{
				fireSpreadTimer = 0f;
				Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, fireSpreadRadius);
				foreach (Collider colliderToMakeBurn in nearbyColliders)
				{
					IAttackable attackable = colliderToMakeBurn as IAttackable;
					if (attackable != null)
					{
						attackable.TakeDamage(new DamageParams(0, Element.Fire));
					}
				}
			}

			health -= fireDamagePerSecond;
			if (health <= 0)
			{
				Die();
			}
		}
	}

	virtual public void TakeDamage(DamageParams args)
	{
		if (health > 0)
		{
			if (!squash.inSquash)
			{
				if (args.amount > 0)
				{
					health -= args.amount;
					squash.DoSquash(health > 0); 
				}

				switch (args.element)
				{
					case Element.None:
						break;
					case Element.Fire:
						fireStatus = 2.0f;
						fire.SetActive(true);
						break;
					case Element.Ice:
						iceStatus = 2.0f;
						break;
					case Element.Poison:
						poisonStatus = 2.0f;
						break;
					case Element.Lightning:
						lightningStatus = 2.0f;
						break;
				}

				if (health <= 0)
				{
					Die();
				}
			}
		}
	}

	abstract protected void Die();
}
