using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Movement
{

	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public Color collideColor = new Color(1f, 1f, 1f, 1f);

	public float speed;
	public float maxSpeed;
	public float dashSpeed;
	private bool startDashTimer;
	public float dashTimer;
	public float coolDownPeriod;
	public float timeStamp = 3;

	bool damaged;
	public bool iFrames;
	TrailRenderer tRail;
	AfterImages afterImages;
	Renderer gameMesh;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		iFrames = false;
		tRail = GetComponent<TrailRenderer>();
		afterImages = GetComponent<AfterImages>();
		gameMesh = GetComponent<MeshRenderer>();
		currentHealth = startingHealth;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 movement;


		float rawAxisX, rawAxisY;
		rawAxisX = Input.GetAxis("Horizontal");
		//rawAxisX = (float)System.Math.Round(rawAxisX);        
		rawAxisY = Input.GetAxis("Vertical");
		//rawAxisY = (float)System.Math.Round(rawAxisY);
		movement = new Vector3(rawAxisX * speed, 0, rawAxisY * speed);

		if (movement.sqrMagnitude == 0)
		{
			rb.velocity *= 0.1f;
		}
		else
		{
			transform.rotation = Quaternion.LookRotation(movement);
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
		if (Input.GetAxis("LeftBumpStick") != 0 && coolDownPeriod == 0 || Input.GetKey(KeyCode.Q) && coolDownPeriod == 0)
		{
			startDashTimer = true;
			dashTimer = 0.5f;
			iFrames = true;
			coolDownPeriod = timeStamp;
			afterImages.Show();
			CameraEffects.Get.Shake(0.1f, 0.04f);
		}
		if (startDashTimer == true)
		{
			rb.velocity = transform.forward * dashSpeed;
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
			{
				startDashTimer = false;
				dashTimer = 0;
				rb.velocity = new Vector3(0, 0, 0);
				iFrames = false;
			}
		}
		if (damaged)
		{
			damageImage.color = flashColour;
		}
		else
		{
			damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
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
	public void TakeDamage(int amount)
	{
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;

		gameMesh.material.SetColor("_TintColor", collideColor);
		StartCoroutine(Flasher());
		print("newColour");

		CameraEffects.Get.Shake(0.1f, 0.2f);
	}
	IEnumerator Flasher()
	{
		if (gameMesh != null)
		{
			for (int i = 0; i <= 5; i++)
			{
				gameMesh.material.SetFloat("_TintAmount", 0.8f);
				yield return new WaitForSeconds(.1f);
				gameMesh.material.SetFloat("_TintAmount", 0);
				yield return new WaitForSeconds(.05f);
			}
		}
	}



	//private void OnTriggerEnter(Collider other)
	//{
	//    if (other.gameObject.CompareTag("enemy") && iFrames == false)
	//    {
	//        print("dead");
	//    }
	//}

}
