using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToEnemies : MonoBehaviour
{

	public float damage;

	// Use this for initialization
	void Start()
	{

	}

	void OnTriggerStay(Collider other)
	{
		print("OMG CAN WE KILL THIS THING PLZ???" + other.gameObject.name);
		IAttackable thingICanKill = other.GetComponent<IAttackable>();
		if (thingICanKill != null)
		{
			print("YES WE CAN DIE");
			thingICanKill.TakeDamage(damage); 
		}
	}
}
