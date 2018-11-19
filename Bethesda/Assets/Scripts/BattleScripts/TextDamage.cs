using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDamage : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    bool playerInRange;
    PlayerMovement playerHealth;
    GameObject player;
    float timer;
    
	// Use this for initialization
	void Start ()
    {
        playerInRange = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange)
        {
            Attack();
        }
	}
    void Attack()
    {
        timer = 0f;
        if (playerHealth.currentHealth > 0 && playerHealth.iFrames == false)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject)
    //    {
    //        playerInRange = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        playerInRange = false;
    //    }
    //}
}
