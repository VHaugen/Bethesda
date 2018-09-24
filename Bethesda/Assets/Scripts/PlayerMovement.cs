using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement {

    public float speed;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 movement = new Vector3(0, rb.velocity.y, 0);
        if (Input.GetButtonDown("Horizontal"))
        {
            float rawAxisX;
            rawAxisX = Input.GetAxisRaw("Horizontal");
            rawAxisX = (float)System.Math.Round(rawAxisX);
            movement = new Vector3(rawAxisX * speed, 0, rb.velocity.z);
        }
        if (Input.GetButtonDown("Vertical"))
        {
            float rawAxisY;
            rawAxisY = Input.GetAxisRaw("Vertical");
            rawAxisY = (float)System.Math.Round(rawAxisY);
            movement = new Vector3(rb.velocity.x, 0, rawAxisY * speed);
        }
        else
        {
            rb.velocity *= 0.1f;
        }
        rb.velocity = movement;
	}
   
}
