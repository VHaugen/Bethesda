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

    bool damaged;
    public bool iFrames;
    TrailRenderer tRail;
    AfterImages afterImages;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        iFrames = false;
        tRail = GetComponent<TrailRenderer>();
        afterImages = GetComponent<AfterImages>();
        GetComponent<MeshRenderer>();
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

        rb.AddForce(movement, ForceMode.Acceleration);

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            transform.rotation = Quaternion.LookRotation(movement);

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

        StartCoroutine(Flasher());
                print("newColour");
    }
    IEnumerator Flasher()
    {
        var renderer = GameMesh;
        if (renderer != null)
        {
            for (int i = 0; i <= 5; i++)
            {
                renderer.material.color = collideColor;
                yield return new WaitForSeconds(.1f);
                renderer.material.color = normalColor;
                yield return new WaitForSeconds(.1f);
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
