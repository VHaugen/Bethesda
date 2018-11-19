using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour, IAttackable
{
	[SerializeField]
	Slider healthSlider;

	public float iFramesDuration = 0.5f;
	public float knockbackMultiplier = 1.0f;
	public Color collideColor = Color.white;
	protected Vector3 knockbackVector;

	protected float iFramesTimer = -1.0f;
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

		if (iFramesTimer > 0)
		{
			iFramesTimer -= Time.deltaTime;
			if (iFramesTimer <= 0)
			{
				iFramesTimer = -1;
				GetComponent<Renderer>().material.SetFloat("_TintAmount", 0);
			}
		}
	}

	IEnumerator Flasher()
	{
		var renderer = GetComponent<Renderer>();
		if (renderer != null)
		{
			renderer.material.SetColor("_TintColor", collideColor);
			while (iFramesTimer > 0)
			{
				renderer.material.SetFloat("_TintAmount", 0.8f);
				yield return new WaitForSeconds(.06f);
				renderer.material.SetFloat("_TintAmount", 0.0f);
				yield return new WaitForSeconds(.03f);
			}
		}
	}

	virtual public void TakeDamage(DamageParams args)
	{
		if (health > 0)
		{
			if (!squash.inSquash && iFramesTimer <= 0)
			{
				if (args.amount > 0)
				{
					health -= args.amount;
					healthSlider.gameObject.SetActive(true);
					healthSlider.value = health;
					if (args.damageType == DamageType.Squash)
						squash.DoSquash(health > 0);
					else if (args.damageType == DamageType.Hit)
					{
						iFramesTimer = iFramesDuration;
						knockbackVector = knockbackMultiplier * args.knockback;
						StartCoroutine(Flasher());
					}
					ExtraTakeDamage(args);
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
	
	virtual protected void ExtraTakeDamage(DamageParams args)
	{

	}

	abstract protected void Die();
}
