using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_Room : MonoBehaviour {

    public GameObject fight;
    GameObject lathis;

	// Use this for initialization
	void Start () {
        lathis = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Instantiate(fight, new Vector3(transform.root.position.x, transform.root.position.y, transform.root.position.z), Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}
