using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {


    protected Rigidbody rb;
    
	// Use this for initialization
	protected virtual void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
	}

    void Update()
    {

        
    }
}
