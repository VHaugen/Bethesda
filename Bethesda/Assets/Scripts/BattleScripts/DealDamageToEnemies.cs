using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToEnemies : MonoBehaviour
{
	int manaCharge = 5;
	public Transform player;
	public Element currentElement = Element.None;
	public DamageType damageType = DamageType.Hit;
	public float damage;
	public float knockbackStrength = 1.0f;
	PlayerMovement mana;
    public AudioClip impact;
    public AudioSource audioSource;



	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		mana = player.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		IAttackable thingICanKill = other.GetComponent<IAttackable>();
		if (thingICanKill != null)
		{
			Vector3 knockback = Vector3.zero;
			Transform me = player ? player : transform;
			knockback = other.transform.position - me.position;
			knockback.y = 1f;
			knockback.Normalize();
			knockback *= knockbackStrength;
			thingICanKill.TakeDamage(new DamageParams(damage, currentElement, damageType, knockback));
			mana.AddMana(manaCharge);
            audioSource.PlayOneShot(impact, 0.3f);
		}
	}
}
