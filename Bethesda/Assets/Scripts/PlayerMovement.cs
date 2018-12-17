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
    public Slider healthSlider;
    public Slider manaSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    private Color normalColor = new Color(0.9137255f, 0.6511509f, 0.1960784f, 1f);
    public Color collideColor = new Color(1f, 1f, 1f, 0.1f);
    public Renderer GameMesh;

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
    AudioSource audio;
    public AudioClip dashSound;
    public AudioClip damageSound;
    Flammable flammable;
    Animator anim;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        iFrames = false;
        tRail = GetComponent<TrailRenderer>();
        afterImages = GetComponent<AfterImages>();
        GetComponent<MeshRenderer>();
        currentHealth = startingHealth;
        manaSlider.value = startingMana;
        audio = GetComponent<AudioSource>();
        flammable = GetComponent<Flammable>();
        anim = GetComponent<Animator>();
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
            startDashTimer = true;
            dashTimer = 0.5f;
            iFrames = true;
            coolDownPeriod = timeStamp;
            afterImages.Show();
            CameraEffects.Get.Shake(0.1f, 0.1f);
            audio.PlayOneShot(dashSound);
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
        if (flammable.IsBurning())
        {
            currentHealth -= (fireDamagePerSecond - armorValue) * Time.deltaTime;
            healthSlider.value = currentHealth;
            damaged = true;

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
        
        
            //manaSlider.value = currentMana;
            manaSlider.value += amount;

        
    }

    public void UseMana(int amount)
    {
        //currentMana -= amount;
        manaSlider.value -= amount;
        print("lostmana");
    }

    public void TakeDamage(int amount)
    {

        damaged = true;
        currentHealth -= amount - armorValue;
        healthSlider.value = currentHealth;

        StartCoroutine(Flasher());
        print("newColour");
        CameraEffects.Get.Shake(0.1f, 0.5f);
        audio.PlayOneShot(damageSound);
    }
    IEnumerator Flasher()
    {
        var renderer = GameMesh;
        if (renderer != null)
        {
            renderer.material.SetColor("_TintColor", collideColor);
            for (int i = 0; i <= 10; i++)
            {
                renderer.material.SetFloat("_TintAmount", 0.8f);
                iFrames = true;
                yield return new WaitForSeconds(.06f);
                renderer.material.SetFloat("_TintAmount", 0.0f);
                yield return new WaitForSeconds(.03f);
            }
        }
        iFrames = false;
    }



    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("enemy") && iFrames == false)
    //    {
    //        print("dead");
    //    }
    //}

}