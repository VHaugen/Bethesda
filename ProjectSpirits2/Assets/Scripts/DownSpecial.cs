using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downspecial : MonoBehaviour {

    PlayerMovement player;
    Animator anim;
    Shuriken shuriken;
    public float speed;
    public GameObject shur;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        anim = gameObject.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update ()
    {

        if ((player.isGrounded || player.isGrounded == false) && player.rawAxisY <= -0.5f && player.rawAxisX == 0f && Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(downSpecial(0.3f));
        }
		
	}
     IEnumerator downSpecial(float animationTime)
    {
        player.canMove = false;
        anim.Play("DownSpecial");
        Instantiate(shur, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(animationTime);
        player.canMove = true;
    }
}
