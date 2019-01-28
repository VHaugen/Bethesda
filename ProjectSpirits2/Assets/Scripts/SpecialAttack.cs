using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour {

    PlayerMovement player;
    Animator anim;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((player.isGrounded || player.isGrounded == false) && Input.GetKeyDown(KeyCode.K) && player.rawAxisX == 0f && player.rawAxisY == 0f)
        {
            if (player.canMove)
            {
                StartCoroutine(NeturalSpecial(1f));
            }
        }
        if ((player.isGrounded || player.isGrounded == false) && Input.GetKeyDown(KeyCode.K) && player.rawAxisY >= 0.5f)
        {
            if (player.canMove)
            {
                StartCoroutine(UpSpecial(2f));
            }
        }
    }
    IEnumerator NeturalSpecial(float animationTime)
    {
        anim.Play("NeturalSpecial");
        yield return new WaitForSeconds(animationTime);
        anim.Play("NeutralSpecialEnding");
    }
    IEnumerator UpSpecial(float animationTime)
    {        
        anim.Play("UpSpecial");
        player.rb2d.velocity = Vector2.zero;
        player.rb2d.AddForce(transform.up * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(animationTime);
        player.jump = false;
    }
}
