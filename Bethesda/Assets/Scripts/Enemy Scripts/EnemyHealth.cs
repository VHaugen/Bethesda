using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SquashEffect), typeof(Rigidbody), typeof(AudioSource))]
class EnemyHealth : MonoBehaviour, IAttackable
{
	static public int numEnemies { get; private set; }
	static float lastDeathSoundTime = 0;

	public delegate void TakeDamageEventHandler(DamageParams args);
	public event TakeDamageEventHandler TakeDamageEvent;
	public delegate void DieEventHandler();
	public event DieEventHandler DieEvent;

	[HideInInspector] public Vector3 knockbackVector;
	public float health = 5;
	public float knockbackDuration = 0.5f;

	[SerializeField] AudioClip burn;
	[SerializeField] AudioClip slow;
	[SerializeField] AudioClip paralysis;
	[SerializeField] AudioClip poisoned;
	[SerializeField] AudioClip deathSound;
	[SerializeField] GameObject canvasPrefab;
	[SerializeField] GameObject[] powerUps;

	[SerializeField] float fireDamagePerSecond = 0.5f;
	[SerializeField] float iceDamagePerSecond = 0.25f;
	[SerializeField] float poisonDamagePerSecond = 0.5f;
	[SerializeField] float poisonStatusDuration = 5.0f;

	[SerializeField] float iFramesDuration = 0.5f;
	[SerializeField] float knockbackMultiplier = 1.0f;
	[SerializeField] Color collideColor = Color.white;

	float iFramesTimer = -1;
	float poisonStatus = -1;
	int poisonEffectIndex = -1;
	bool startedDeathFlashing = false;

	bool created = false;
	bool destroyed = false;

	SquashEffect squash;
	Flammable flammable;
	FrostBite freezable;
	Electracuted electracuted;
	AudioSource audioSource;
	Rigidbody rbody;
	Slider healthSlider;
	Renderer meshRenderer;
	PuzzleHandler puzzleHandler;

	// Use this for initialization
	void Awake()
	{
		flammable = GetComponent<Flammable>();
		freezable = GetComponent<FrostBite>();
		electracuted = GetComponent<Electracuted>();
		squash = GetComponent<SquashEffect>();
		//speed = GetComponent<TestEnemy>().maxSpeed;
		audioSource = GetComponent<AudioSource>();
		healthSlider = Instantiate(canvasPrefab, transform, false).GetComponentInChildren<Slider>();
		rbody = GetComponent<Rigidbody>();
		meshRenderer = GetComponentInChildren<Renderer>();
		puzzleHandler = GameObject.Find("World").GetComponent<PuzzleHandler>();

		AddMe();
	}

	void OnDestroy()
	{
		RemoveMe();
	}

	private void Start()
	{
		healthSlider.maxValue = health;
		MusicController.PlayBattleMusic();
	}

	public void AddMe()
	{
		if (!created)
		{
			numEnemies++;
			print("Add enemy " + gameObject.name + "=" + numEnemies);
			created = true;
		}
	}

	public void RemoveMe()
	{
		if (!destroyed)
		{
			numEnemies--;
			print("Remove enemy " + gameObject.name + "=" + numEnemies);
			destroyed = true;
			if (numEnemies <= 0)
			{
				MusicController.PlayCalmMusic();
				numEnemies = 0;
				if (puzzleHandler)
					puzzleHandler.PuzzelIsCleared();
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		//speed = GetComponent<TestEnemy>().maxSpeed = 6;
		if (flammable && flammable.IsBurning())
		{
			SetHealth(health - fireDamagePerSecond * Time.deltaTime);

		}

		if (freezable && freezable.IsFreezing())
		{
			SetHealth(health - iceDamagePerSecond * Time.deltaTime);
			//slowSpeed = GetComponent<TestEnemy>().maxSpeed = 1.5f;

		}

		if (poisonStatus > 0)
		{
			SetHealth(health - poisonDamagePerSecond * Time.deltaTime);
			poisonStatus -= Time.deltaTime;

			if (poisonStatus <= 0)
			{
				poisonStatus = -1;
				ParticleEffectsManager.GetEffect("Poison").Stop(poisonEffectIndex);
			}
		}

		if (electracuted && electracuted.IsElc())
		{
			//healthSlider.value = health;
			//healthSlider.gameObject.SetActive(true);
			//stun = GetComponent<TestEnemy>().maxSpeed = 0f;

			print("STUNNED");
		}

		if (iFramesTimer > 0)
		{
			iFramesTimer -= Time.deltaTime;
			if (iFramesTimer <= 0)
			{
				iFramesTimer = -1;
				GetComponentInChildren<Renderer>().material.SetFloat("_TintAmount", 0);
			}
		}
		//slowSpeed = speed;
		//stun = speed;
	}

	void SetHealth(float newHealth)
	{
		health = newHealth;
		healthSlider.value = health;
		healthSlider.gameObject.SetActive(true);
		if (health <= 0)
		{
			Die();
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
					SetHealth(health - args.amount);
					if (args.damageType == DamageType.Squash)
						squash.DoSquash(health > 0);
					else if (args.damageType == DamageType.Hit)
					{
						iFramesTimer = iFramesDuration;
						knockbackVector = knockbackMultiplier * args.knockback;
						StartCoroutine(Flasher());
					}
					TakeDamageEvent.Invoke(args);
				}

				CameraEffects.Get.FreezeFrames(0.15f, 0.01f);

				switch (args.element)
				{
					case Element.None:
						break;
					case Element.Fire:
						if (flammable)
						{
							flammable.StartBurning();
							if (!audioSource.isPlaying)
							{
								audioSource.PlayOneShot(burn, 0.5f);
							}
						}
						break;
					case Element.Ice:
						if (freezable)
						{
							freezable.FreezeStart();
							if (!audioSource.isPlaying)
							{
								audioSource.PlayOneShot(slow, 1f);
							}
						}
						break;
					case Element.Poison:
						poisonStatus = poisonStatusDuration;
						if (poisonStatus > 0)
						{
							poisonEffectIndex = ParticleEffectsManager.GetEffect("Poison").Spawn(meshRenderer);
							if (!audioSource.isPlaying)
							{
								audioSource.PlayOneShot(poisoned, 1f);
							}
						}
						break;
					case Element.Lightning:
						if (electracuted)
						{
							electracuted.ElcStart();
							if (!audioSource.isPlaying)
							{
								audioSource.PlayOneShot(paralysis, 0.10f);
							}
						}
						break;
				}

				if (health <= 0)
				{
					Die();
					if (Random.Range(1, 5) == 1)
					{
						Instantiate(powerUps[Random.Range(0, powerUps.Length - 1)], transform.position + Vector3.up * 1, Quaternion.identity);
					}
				}
			}
		}
	}

	void Die()
	{
		StartCoroutine(ShakeAndDie());
		DieEvent.Invoke();
	}

	IEnumerator Flasher()
	{
		if (meshRenderer != null)
		{
			meshRenderer.material.SetColor("_TintColor", collideColor);
			while (iFramesTimer > 0)
			{
				meshRenderer.material.SetFloat("_TintAmount", 0.8f);
				yield return new WaitForSeconds(.06f);
				meshRenderer.material.SetFloat("_TintAmount", 0.0f);
				yield return new WaitForSeconds(.03f);
			}
		}
	}

	IEnumerator ShakeAndDie()
	{
		startedDeathFlashing = true;
		Vector3 basePosition = rbody.position;
		rbody.isKinematic = true;
		float radius = 0.2f;

		WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
		while (squash.inSquash && !squash.isFlat)
		{
			yield return waitFrame;
		}

		for (int i = 0; i < 20; i++)
		{
			rbody.position = basePosition + Random.onUnitSphere * radius;
			yield return new WaitForSeconds(0.05f - i * 0.02f);
		}
		transform.localScale = Vector3.one;
		ParticleEffectsManager.GetEffect("DeathExplosion").Spawn(meshRenderer);
		if (Time.time - lastDeathSoundTime > 0.25f)
		{
			lastDeathSoundTime = Time.time;
			Sound.Play(deathSound, 0.5f); 
		}
		Destroy(gameObject);
	}
}
