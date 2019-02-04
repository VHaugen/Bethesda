using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementParent
{

    public float speed;
    public float rawAxisX;
    public float rawAxisY;
    public bool canMove = true;
    public bool facingRight;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        jumpForce = 2.5f;
        shortJumpForce = 2f;
        facingRight = true;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Vector2 movement = new Vector2(rb2d.velocity.x, rb2d.velocity.y);

        //float rawAxisX;
        rawAxisX = Input.GetAxisRaw("Horizontal");
        rawAxisX = (float)System.Math.Round(rawAxisX);
        rawAxisY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(rawAxisX * speed, rb2d.velocity.y);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump = true;
            isGrounded = false;
            anim.Play("JumpAnimationTest");
            anim.SetBool("standingStillBool", false);
        }
        if (Input.GetButtonDown("Jump") && doubleJump)
        {
            jump = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            doubleJump = false; isGrounded = false;
            anim.Play("DoubleJumpAnimationTest");
            anim.SetBool("standingStillBool", false);
        }
        if (rawAxisX >= 0.5f && isGrounded || rawAxisX <= -0.5f && isGrounded)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                facingRight = true;
                
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                facingRight = false;
                
            }
            anim.SetBool("standingStillBool", false);

        }


        else if (isGrounded)
        {
            anim.SetBool("standingStillBool", true);
        }
        if (facingRight == true)
        {
            transform.localScale = new Vector3(0.581f, 0.581f, 0.581f);
        }
        if (facingRight == false)
        {
            transform.localScale = new Vector3(-0.581f, 0.581f, -0.581f);
        }
        if (canMove == false)
        {
            movement = Vector2.zero; 
        }

        if (canMove)
        {
            rb2d.velocity = movement;
        }
    }
}
