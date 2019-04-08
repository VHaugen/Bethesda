using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takedamage : MonoBehaviour {

    public int attackDamage = 10;
    
    GameObject player;
    PlayerMovement playerHealth;
    bool playerInRange;
    float timer;

	// Use this for initialization
	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (playerHealth.currentHealth > 0 && playerHealth.iFrames == false)
            {
                playerHealth.TakeDamage(attackDamage);
                print("Damage");
            }
            else if(playerHealth.currentHealth > 0 && playerHealth.iFrames)
            {
                print("Invincibility");
            }
            else if (playerHealth.currentHealth <= 0)
            {
                print("Dead");
            }
        }
    }
}
