using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilts : MonoBehaviour
{

    PlayerMovement player;
    Animator anim;


    // Use this for initialization
    void Start()
    {

        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded && Input.GetKeyDown(KeyCode.J) && (player.rawAxisX >= 0.5f || player.rawAxisX <= -0.5f))
        {
            if (player.canMove)
                StartCoroutine(SideTilt(0.5f));


        }

        if (player.isGrounded && player.rawAxisY >= 0.5f && Input.GetKeyDown(KeyCode.J) && player.rawAxisX == 0f)
        {
            if (player.canMove)
                StartCoroutine(UpTilt(0.75f));
        }
        if (player.isGrounded && player.rawAxisY <= -0.5f && Input.GetKeyDown(KeyCode.J) && player.rawAxisX == 0f)
        {
            /* anim.Play("DTilt");
             print("yey");
             player.canMove = false;
             print("slide");
             player.rb2d.velocity = Vector2.zero;
             player.rb2d.AddForce(transform.right * 1, ForceMode2D.Impulse);
             print("done");*/
            if (player.canMove)
                StartCoroutine(TiltAction(0.3f));

        }
    }

    IEnumerator TiltActionLeft(float animationTime)
    {
        player.canMove = false;
        anim.Play("DTilt");
        player.rb2d.velocity = Vector2.zero;
        player.rb2d.AddForce(transform.right * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(animationTime);
        player.canMove = true;
    }
    IEnumerator TiltAction(float animationTime)
    {
        player.canMove = false;
        anim.Play("DTilt");
        player.rb2d.velocity = Vector2.zero;
        player.rb2d.AddForce(transform.right * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(animationTime);
        player.canMove = true;
    }
    IEnumerator SideTilt(float animationTime)
    {
        player.canMove = false;
        anim.Play("SideTilt");
        yield return new WaitForSeconds(animationTime);
        player.canMove = true;
    }
    IEnumerator UpTilt(float animationTime)
    {
        player.canMove = false;
        anim.Play("UpTilt");
        yield return new WaitForSeconds(animationTime);
        player.canMove = true;

    }

}
