using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

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
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	private Color normalColor = new Color(0.9137255f, 0.6511509f, 0.1960784f, 1f);
	public Color collideColor = new Color(1f, 1f, 1f, 0.1f);
	public Renderer meshRenderer;
	public Image dashCooldown;
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
	private bool startDashTimer;
	public float dashTimer;
	public float coolDownPeriod;
	public float timeStamp = 3;

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
	Flammable flammable;
	Animator anim;
	HammerWeapon weapon;


	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		iFrames = false;
		tRail = GetComponent<TrailRenderer>();
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
			anim.SetBool("StandStill", true);
		}
		else
		{
			transform.rotation = Quaternion.LookRotation(movement);
			anim.SetBool("StandStill", false);
		}

		rb.AddForce(movement, ForceMode.Acceleration);

		if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
		{
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}
		if (coolDownPeriod >= 0)
		{
			coolDownPeriod -= Time.deltaTime;
		}
		if (coolDownPeriod < 0)
		{
			coolDownPeriod = 0;
		}
		if (Input.GetAxis("RightBumpStick") != 0 && coolDownPeriod == 0 || Input.GetKey(KeyCode.LeftShift) && coolDownPeriod == 0)
		{
			anim.Play("Run_cycle");
			isCooldown = true;
			startDashTimer = true;
			dashTimer = 0.5f;
			iFrames = true;
			coolDownPeriod = timeStamp;
			afterImages.Show();
			CameraEffects.Get.Shake(0.1f, 0.1f);
			audioSource.PlayOneShot(dashSound);
			dashCooldown.fillAmount = 1;
		}
		if (startDashTimer == true && isCooldown == true)
		{
			rb.velocity = transform.forward * dashSpeed;
			dashCooldown.fillAmount -= 1 / timeStamp * Time.deltaTime;
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0 && dashCooldown.fillAmount <= 0)
			{
				startDashTimer = false;
				dashCooldown.fillAmount = 0;
				dashTimer = 0;
				rb.velocity = new Vector3(0, 0, 0);
				iFrames = false;
				dashCooldown.GetComponent<Fadeplosion>().Perform();
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
		currentMana = Mathf.Min(currentMana + amount, manaSlider.maxValue);
		manaSlider.value = currentMana;
		//print("gained mana" + amount + " -- " + manaSlider.value);
	}

	public void UseMana(int amount)
	{
		currentMana -= amount;
		manaSlider.value -= amount;
		print("lostmana " + amount);
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
		gameObject.SetActive(false);
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

	public void ScreenShake()
	{
		CameraEffects.Get.Shake(0.25f, 0.4f);
	}
	public void SwingHit()
	{
		for (int i = 0; i < numRays; i++)
		{

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
				}
			}
		}
	}
	public void SwingSound()
	{
		audioSource.PlayOneShot(swingSound);
	}


	//private void OnTriggerEnter(Collider other)
	//{
	//    if (other.gameObject.CompareTag("enemy") && iFrames == false)
	//    {
	//        print("dead");
	//    }
	//}

}