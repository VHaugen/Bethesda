using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{

    public float speed;
    public float maxSpeed;
    public float dashSpeed;
    private bool startDashTimer;
    public float dashTimer;
    public float coolDownPeriod;
    public float timeStamp = 3;
    private bool iFrames;
    TrailRenderer tRail;
	AfterImages afterImages;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        iFrames = false;
        tRail = GetComponent<TrailRenderer>();
		afterImages = GetComponent<AfterImages>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") && iFrames == false)
        {
            print("dead");
        }
    }

}
