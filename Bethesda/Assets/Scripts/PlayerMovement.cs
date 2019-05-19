using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PlayerMovement : Movement
{

	public int startingHealth = 100;
	public float startingMana = 150;
	public float fireDamagePerSecond;
	public int armorValue;
	public float currentHealth;
	public float currentMana;
	public FrameByFrameSlider healthSlider;
	public FrameByFrameSlider manaSlider;
	public Color collideColor = new Color(1f, 1f, 1f, 0.1f);
	public Renderer meshRenderer;
	public Image dashCooldown;
	public Image heavyAttackIndicator;
	bool isCooldown;
	int numRays = 7;
	public GameObject rayCastPoint;
	public float knockbackStrength = 2.0f;
	public float swingDamage;
	public Element currentElement = Element.None;
	public DamageType damageType = DamageType.Hit;

	public float speed;
	public float maxSpeed;
	public float dashSpeed;
	public float dashCooldownDuration = 3;
	public float dashDuration = 0.5f;

	bool startDashTimer;
	float dashTimer;
	float dashCooldownTimer;

	public float vignetteStart = 0.4f;
	public float vignetteDead = 0.625f;
	Vignette vignetteLayer = null;
	public GameObject postProcessingObject;

	bool damaged;
	public bool iFrames;
	TrailRenderer tRail;
	AfterImages afterImages;
	AudioSource audioSource;
	public AudioClip dashSound;
	public AudioClip damageSound;
	public AudioClip swingSound;
	public AudioClip swingSoundHit;
	public AudioClip deathSound;
	public AudioClip deathSound2;
	Flammable flammable;
	Animator anim;
    SkinnedMeshRenderer meshRen;
    public ParticleSystem armorPickUp;
	HammerWeapon weapon;
	[HideInInspector] public bool inAttack;
    public AudioClip[] walkingSounds;


	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		iFrames = false;
		tRail = GetComponent<TrailRenderer>();
        meshRen = GetComponent<SkinnedMeshRenderer>();
		afterImages = transform.Find("front1").GetComponent<AfterImages>();
		healthSlider.value = currentHealth = startingHealth;
		manaSlider.value = currentMana = startingMana;
		audioSource = GetComponent<AudioSource>();
		flammable = GetComponent<Flammable>();
		anim = GetComponent<Animator>();
		weapon = GetComponentInChildren<HammerWeapon>();

	}

	// Update is called once per frame
	void Update()
	{

		if (!startDashTimer)
		{
			Vector3 movement;


			float rawAxisX, rawAxisY;
			rawAxisX = Input.GetAxisRaw("Horizontal");
			//rawAxisX = (float)System.Math.Round(rawAxisX);        
			rawAxisY = Input.GetAxisRaw("Vertical");
			//rawAxisY = (float)System.Math.Round(rawAxisY);
			movement = new Vector3(rawAxisX * speed, 0, rawAxisY * speed);



			if (movement.sqrMagnitude == 0)
			{
				rb.velocity *= 0.1f;
				anim.SetFloat("Speed", 0.0f);
			}
			else
			{
				transform.rotation = Quaternion.LookRotation(movement);
				anim.SetFloat("Speed", 1.0f);
			}

			rb.AddForce(movement, ForceMode.Acceleration);

			float currentSpeed = rb.velocity.magnitude;

			if (currentSpeed > maxSpeed)
			{
				rb.velocity = (rb.velocity / currentSpeed) * maxSpeed;
			} 
		}

		if (dashCooldownTimer > 0)
		{
			dashCooldownTimer -= Time.deltaTime;
			dashCooldown.fillAmount -= 1 / dashCooldownDuration * Time.deltaTime;
			if (dashCooldownTimer < 0)
			{
				dashCooldownTimer = 0;
				dashCooldown.fillAmount = 0;
				dashCooldown.GetComponent<Fadeplosion>().Perform();
			}
		}
		if ((Input.GetAxis("RightBumpStick") != 0 || Input.GetKeyDown(KeyCode.LeftShift)) && dashCooldownTimer == 0)
		{
			startDashTimer = true;
			dashTimer = dashDuration;
			afterImages.duration = dashDuration - dashDuration / afterImages.numberOfImages;
			meshRenderer.material.SetFloat("_TintAmount", afterImages.tintFactor);
			meshRenderer.material.SetColor("_TintColor", afterImages.tintColor);
			iFrames = true;
			dashCooldownTimer = dashCooldownDuration;
			afterImages.Show();
			CameraEffects.Get.Shake(0.1f, 0.1f);
			audioSource.PlayOneShot(dashSound);
			dashCooldown.fillAmount = 1;
		}
		if (startDashTimer == true)
		{
			rb.velocity = transform.forward * dashSpeed;
			anim.SetFloat("Speed", 2.5f);
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
			{
				startDashTimer = false;
				dashTimer = 0;
				rb.velocity = new Vector3(0, 0, 0);
				iFrames = false;
				meshRenderer.material.SetFloat("_TintAmount", 0);
			}
		}

		if (flammable.IsBurning())
		{
			currentHealth -= (fireDamagePerSecond - armorValue) * Time.deltaTime;
			healthSlider.value = currentHealth;
			damaged = true;
			if (currentHealth <= 0)
			{
				Die();
			}

		}

		if (damaged)
		{

			//damageImage.color = flashColour;
			float vignetteIntensity = 0.0f;
			if (currentHealth <= 50)
			{
				vignetteIntensity = Mathf.Lerp(vignetteStart, vignetteDead, 1f - currentHealth / 50f);
			}
			//print(vignetteIntensity);
			CameraEffects.Get.SetDamageVignette(vignetteIntensity);

		}
		else
		{
			//damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;


		//if (iFrames)
		//{
		//    tRail.enabled = true;
		//}
		//else if (!iFrames)
		//{
		//    tRail.enabled = false;
		//}
		//print(rb.velocity.normalized);


		//}
		//   void PlayerDirection()
		//   {
	}
	public void AddMana(int amount)
	{
		float prevMana = currentMana;
		currentMana = Mathf.Min(currentMana + amount, manaSlider.maxValue);
		manaSlider.value = currentMana;
		if (heavyAttackIndicator)
		{
			if (currentMana >= weapon.overHeadCost && prevMana < weapon.overHeadCost)
			{
				heavyAttackIndicator.CrossFadeAlpha(1f, 0.3f, true);
				heavyAttackIndicator.GetComponent<Fadeplosion>().Perform();
			}
		}
	}

	public void UseMana(int amount)
	{
		currentMana -= amount;
		manaSlider.value -= amount;
		if (heavyAttackIndicator)
		{
			if (currentMana < weapon.overHeadCost)
			{
				heavyAttackIndicator.CrossFadeAlpha(0.5f, 0.3f, true);
			}
		}
		else
		{
			Debug.LogError("heavyattackindicator not assigned");
		}
	}

	public void TakeDamage(int amount)
	{

		damaged = true;
		currentHealth -= amount - armorValue;
		healthSlider.value = currentHealth;

		StartCoroutine(Flasher());
		print("newColour");
		CameraEffects.Get.Shake(0.1f, 0.5f);
		audioSource.PlayOneShot(damageSound);

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		ParticleEffectsManager.GetEffect("DeathExplosion").Spawn(meshRenderer);
		Sound.Play(deathSound);
		Sound.Play(deathSound2);
		gameObject.SetActive(false);
		GameObject go = new GameObject("PlayerDeath", typeof(PlayerDeath));
		go.GetComponent<PlayerDeath>().Die();
	}

	IEnumerator Flasher()
	{
		if (meshRenderer != null)
		{
			meshRenderer.material.SetColor("_TintColor", collideColor);
			for (int i = 0; i <= 10; i++)
			{
				meshRenderer.material.SetFloat("_TintAmount", 0.8f);
				iFrames = true;
				yield return new WaitForSeconds(.06f);
				meshRenderer.material.SetFloat("_TintAmount", 0.0f);
				yield return new WaitForSeconds(.03f);
			}
		}
		iFrames = false;
	}

	public void PlaySound(Object audioClip)
	{
		audioSource.PlayOneShot((AudioClip)audioClip);
	}

	public void Anim_EnableHitbox(int enable)
	{
		weapon.hitbox.enabled = enable == 0 ? false : true;
	}

	public void EnableSlowDown(int enableSlowness)
	{
		maxSpeed = enableSlowness == 1 ? 1.0f : 10.0f;
	}

	public void Anim_ShowAfterImages()
	{
		weapon.afterImages.Show();
	}

	public void Anim_EnableAoeHitbox(int enable)
	{
		weapon.overHeadHitBox.enabled = enable == 0 ? false : true;
	}

	public void PlayParticleEffect(string effectName)
	{
		ParticleEffectsManager.GetEffect(effectName).Spawn(weapon.overHeadHitBox.transform.position);
	}

	public void HeavyAttackImpactEvent()
	{
		PlayParticleEffect("Slam");
		CameraEffects.Get.Shake(0.5f, 0.2f);
		Anim_EnableAoeHitbox(1);
		foreach (Rigidbody rbody in FindObjectsOfType<Rigidbody>())
		{
			rbody.AddForce(Vector3.up * Random.Range(8.0f, 12.0f), ForceMode.Impulse);
		}

	}

	public void ScreenShake()
	{
		CameraEffects.Get.Shake(0.5f, 0.2f);
	}
	public void SwingHit()
	{
		for (int i = 0; i < numRays; i++)
		{
			ParticleEffectsManager.GetEffect("SwingFX").Spawn(rayCastPoint.transform.position, transform.rotation);

			Vector3 fwd = rayCastPoint.transform.forward;
			float Afwd = Mathf.Atan2(fwd.z, fwd.x);
			float angle = Afwd + (-40 + i * 80 / numRays) * Mathf.Deg2Rad;
			Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

			direction.y = 0;

			Ray r = new Ray(rayCastPoint.transform.position, direction);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit, 10f))
			{

				IAttackable thingICanKill = hit.collider.GetComponent<IAttackable>();
				if (thingICanKill != null)
				{

					audioSource.PlayOneShot(swingSoundHit, 0.5f);

					Vector3 knockback = Vector3.zero;
					knockback = hit.transform.position - transform.position;
					knockback.y = 1f;
					knockback.Normalize();
					knockback *= knockbackStrength;

					thingICanKill.TakeDamage(new DamageParams(swingDamage, currentElement, damageType, knockback));

					ParticleEffectsManager.GetEffect("Hit1").Spawn(hit.transform.position);
				}
			}
		}
	}
	public void SwingSound()
	{
		audioSource.PlayOneShot(swingSound);
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "armor")
        {
            armorPickUp.Play();
        }
    }

    public void PlayWalkSounds()
    {
        Sound.Play(walkingSounds[Random.Range(0, walkingSounds.Length)], 0.5f);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("enemy") && iFrames == false)
    //    {puzzl
    //        print("dead");
    //    }
    //}

}