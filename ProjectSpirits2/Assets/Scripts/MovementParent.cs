using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class MovementParent : MonoBehaviour {
    [HideInInspector]
    public Rigidbody2D rb2d;
    public bool jump;
    public float jumpForce;
    public float shortJumpForce;
    public bool isGrounded;
    public bool doubleJump;
    
    protected Animator anim;
    protected SpriteRenderer sr;

	// Use this for initialization
	protected virtual void Start ()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        doubleJump = false;
        isGrounded = false;
        jump = false;
	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate ()
    {
        if (jump)
        {
            jump = false;
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            
        }
        
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJump = false;

            print("Grounded");
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            doubleJump = true;
        }
    }
}
