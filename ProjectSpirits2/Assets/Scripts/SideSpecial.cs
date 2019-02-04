using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSpecial : MonoBehaviour {

    PlayerMovement player;
    Animator anim;
    //Animator anim2;
    public GameObject ropedart;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        anim = gameObject.GetComponent<Animator>();
        //anim2 = ropedart.gameObject.GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((player.isGrounded || player.isGrounded == false) && (player.rawAxisX >= 0.5f || player.rawAxisX <= -0.5f) && player.rawAxisY == 0 && Input.GetKeyDown(KeyCode.K))
        {
            if (player.canMove)
            {
                StartCoroutine(sideSpecial(1f));
            }
        }
	}
    IEnumerator sideSpecial(float animationTime)
    {
        player.canMove = false;
        anim.Play("SideSpecial1");
        Instantiate(ropedart, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(animationTime);
        player.canMove = true;
    }
}
