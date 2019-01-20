using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public Material mat;
	public Element element;

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
			Transform weapon = FindObjectOfType<HammerWeapon>().transform;
			if (weapon)
			{
				weapon.GetComponent<MeshRenderer>().material = mat;
				weapon.GetComponent<DealDamageToEnemies>().currentElement = element; 
			}
			else
			{
				Debug.LogWarning("No weapon found!");
			}
        }
    }
}
