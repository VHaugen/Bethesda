using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public Material mat;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            other.transform.Find("Weapon/Head").GetComponent<MeshRenderer>().material = mat; 
        }
    }
}
