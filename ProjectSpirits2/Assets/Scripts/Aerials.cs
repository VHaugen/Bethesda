using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aerials : MonoBehaviour
{


    PlayerMovement player;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.doubleJump && Input.GetKeyDown(KeyCode.J) && player.rawAxisX == 0f && player.rawAxisY == 0f|| player.doubleJump == false && player.isGrounded == false && Input.GetKeyDown(KeyCode.J) && player.rawAxisY == 0f && player.rawAxisX == 0f)
        {
            print("starting coroutine");
            if (player.canMove)
                StartCoroutine(Nair(0.3f));
        }
        if (player.doubleJump && player.rawAxisX >= 0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump == false && player.isGrounded == false && player.rawAxisX >= 0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump && player.rawAxisX <= -0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump == false && player.isGrounded == false && player.rawAxisX <= -0.5f && Input.GetKeyDown(KeyCode.J))
        {
            if (player.canMove)
            {
                StartCoroutine(Fair(0.5f));
            }
        }
        if (player.doubleJump && player.rawAxisX >= 0.5f && player.rawAxisY <= -0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump == false && player.isGrounded == false && player.rawAxisX >= 0.5f && player.rawAxisY <= -0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump && player.rawAxisX <= -0.5f && player.rawAxisY <= -0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump == false && player.isGrounded == false && player.rawAxisX <= -0.5f && player.rawAxisY <= -0.5f && Input.GetKeyDown(KeyCode.J) || (player.doubleJump || player.doubleJump == false && player.isGrounded == false) && player.rawAxisY <= -0.5f && Input.GetKeyDown(KeyCode.J))
        {
            if (player.canMove)
            {
                StartCoroutine(Dair(0.7f));
            }
        }
        if (player.doubleJump && player.rawAxisX >= 0.5f && player.rawAxisY >= 0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump == false && player.isGrounded == false && player.rawAxisX >= 0.5f && player.rawAxisY >= 0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump && player.rawAxisX <= -0.5f && player.rawAxisY >= 0.5f && Input.GetKeyDown(KeyCode.J) || player.doubleJump == false && player.isGrounded == false && player.rawAxisX <= -0.5f && player.rawAxisY >= 0.5f && Input.GetKeyDown(KeyCode.J) || (player.doubleJump || player.doubleJump == false && player.isGrounded == false) && player.rawAxisY >= 0.5f && Input.GetKeyDown(KeyCode.J))
        {
            if (player.canMove)
            {
                StartCoroutine(UpAir(0.3f));
            }
        }
    }

    IEnumerator Nair(float animationTime)
    {
        
        print("playing coroutine");
        anim.Play("Nair");
        yield return new WaitForSeconds(animationTime);
        
    }
    IEnumerator Fair(float animationTime)
    {
        
        anim.Play("ForwardAir");
        yield return new WaitForSeconds(animationTime);
        
    }
    IEnumerator Dair(float animationTime)
    {
        anim.Play("Dair");
        yield return new WaitForSeconds(animationTime);
    }
    IEnumerator UpAir(float animationTime)
    {
        anim.Play("UpAir");
        yield return new WaitForSeconds(animationTime);
    }
}
