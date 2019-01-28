using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jabs : MonoBehaviour
{

    PlayerMovement player;
    Animator anim;
    public bool jab1;
    public bool jab2;
    public bool jab3;
    int jabPress = 0;
    float lastJabPress = 0;
    float maxDelay = 1;



    int jabTimer;
    int jabNo;
    int resetJabTimer;

    // Use this for initialization
    void Start()
    {
        jabNo = 0;
        jabTimer = 15;
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - lastJabPress > maxDelay)
        {
            jabPress = 0;
        }      
        if (jab1)
        {
            resetJabTimer = 51;
            jabNo = 1;
            if (player.isGrounded && Input.GetKeyDown(KeyCode.J) && player.rawAxisY == 0f)
            {
                jab2 = true;
            }
            anim.Play("Jab1");
            if (jabTimer <= 0)
            {
                jab1 = false;
                anim.Play("IdleAnimationTest");
                jabTimer = 16;
            }
            jabTimer--;
        }
        else if (jab2)
        {
            resetJabTimer = 51;
            jabNo = 2;
            if (player.isGrounded && Input.GetKeyDown(KeyCode.J) && player.rawAxisY == 0f)
            {
                jab3 = true;
            }
            anim.Play("Jab2");
            if (jabTimer <= 0)
            {
                jab2 = false;
                anim.Play("IdleAnimationTest");
                jabTimer = 31;
            }
            jabTimer--;
        }
        else if (jab3)
        {
            resetJabTimer = 0;
            jabNo = 0;
            anim.Play("Jab3");
            if (jabTimer == 15)
            {
                //anim.Play("Recovery");
            }
            if (jabTimer <= 0)
            {
                anim.Play("IdleAnimationTest");
                jab3 = false;
                jabTimer = 16;
            }
            jabTimer--;
        }
        resetJabTimer--;
        if (player.isGrounded && Input.GetKeyDown(KeyCode.J) && !jab1 && !jab2 && !jab3 && player.rawAxisY == 0f)
        {
            if (jabNo == 0)
            {
                jab1 = true;
            }
            if (resetJabTimer > 0)
            {
                if (jabNo == 1)
                {
                    jab2 = true;
                }
                else if (jabNo == 2)
                {
                    jab3 = true;
                }
            }
        }
        if (resetJabTimer <= 0)
        {
            jabNo = 0;
        }
        if (player.rawAxisX != 0)
        {
            jab1 = false;
            jab2 = false;
            jab3 = false;
        }

        


    }

}
