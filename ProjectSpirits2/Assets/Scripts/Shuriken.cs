using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    Rigidbody2D rb;
    PlayerMovement player;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        
        Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (player.facingRight)
        {
            rb.GetComponent<Rigidbody2D>().AddForce(transform.right * speed * 10);
        }
        if (player.facingRight == false)
        {
            rb.GetComponent<Rigidbody2D>().AddForce(-transform.right * speed * 10);
        }
        
    }
}
