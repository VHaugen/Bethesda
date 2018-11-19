using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour, IAttackable
{
	[SerializeField]
	Slider healthSlider;

	protected float health;
	protected float fireDamagePerSecond = 0.5f;
	
	protected float poisonStatus = -1;
	protected float iceStatus = -1;
	protected float lightningStatus = -1;
	protected SquashEffect squash;
	protected Flammable flammable;

	virtual protected void Awake()
	{
		flammable = GetComponent<Flammable>();
		squash = GetComponent<SquashEffect>();
	}

	virtual protected void Start()
	{
		healthSlider.maxValue = health;
	}

	virtual protected void Update()
	{
		if (flammable && flammable.IsBurning())
		{
			health -= fireDamagePerSecond * Time.deltaTime;
			healthSlider.gameObject.SetActive(true);
			healthSlider.value = health;
			//print("HP after fire " + health);
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
					healthSlider.gameObject.SetActive(true);
					healthSlider.value = health;
					squash.DoSquash(health > 0); 
				}

				switch (args.element)
				{
					case Element.None:
						break;
					case Element.Fire:
						if (flammable)
						{
							flammable.StartBurning();
						}
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
