using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownSpecial : MonoBehaviour {

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
		
	}
}
