<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour, IAttackable
{
	[SerializeField]
	Slider healthSlider;

	protected float health;
	protected float fireDamagePerSecond = 0.5f;
    protected float iceDamagePerSecond = 0.25f;
	
	protected float poisonStatus = -1;
	//protected float iceStatus = -1;
	protected float lightningStatus = -1;
	protected SquashEffect squash;
	protected Flammable flammable;
    protected FrostBite freezable;

	virtual protected void Awake()
	{
		flammable = GetComponent<Flammable>();
        freezable = GetComponent<FrostBite>();
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
			healthSlider.value = health;
			//print("HP after fire " + health);
			if (health <= 0)
			{
				Die();
			} 
		}
        if (freezable && freezable.IsFreezing())
        {
            health -= iceDamagePerSecond * Time.deltaTime;
            healthSlider.value = health;
            print("Hp after ICE");
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
                        if (freezable)
                        {
                            freezable.FreezeStart();
                        }
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
=======
﻿using System.Collections;
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
>>>>>>> 1980b61f15e882dd088fc8751a6ffada80a7d9be
