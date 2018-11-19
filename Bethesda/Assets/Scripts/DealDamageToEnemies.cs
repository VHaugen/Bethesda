using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToEnemies : MonoBehaviour
{
	public Transform player;
	public Element currentElement = Element.None;
	public DamageType damageType = DamageType.Hit;
	public float damage;
	public float knockbackStrength = 1.0f;

	// Use this for initialization
	void Start()
	{

	}

	void OnTriggerStay(Collider other)
	{
		IAttackable thingICanKill = other.GetComponent<IAttackable>();
		if (thingICanKill != null)
		{
			Vector3 knockback = Vector3.zero;
			if (player)
			{
				knockback = (other.transform.position - player.position).normalized;
				knockback *= knockbackStrength; 
			}
			else
			{
				Debug.LogWarning("Player field not assigned in inspector");
			}
			thingICanKill.TakeDamage(new DamageParams(damage, currentElement, damageType, knockback));
		}
	}
}
