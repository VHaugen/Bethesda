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
    public float timeStamp;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement;
        timeStamp = Time.time + coolDownPeriod;

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
        if (timeStamp >= Time.time)
        {
            if (Input.GetAxis("LeftBumpStick") != 0)
            {
                startDashTimer = true;
                dashTimer = 0.1f;
                dashForward();
            }
            
        }
        print(rb.velocity.normalized);


        //}
        //   void PlayerDirection()
        //   {
    }

    void dashForward()
    {
        rb.velocity = transform.GetChild(0).forward * dashSpeed;

        if (startDashTimer == true)
        {
            dashTimer -= Time.deltaTime;
        }
        if (dashTimer <= 0)
        {
            startDashTimer = false;
            dashTimer = 0;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

}
