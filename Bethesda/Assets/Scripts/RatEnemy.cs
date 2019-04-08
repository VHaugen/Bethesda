using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemy : MonoBehaviour
{
	Animator animator;
	Rigidbody rbody;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		animator.SetFloat("Speed", rbody.velocity.magnitude);
	}
}
