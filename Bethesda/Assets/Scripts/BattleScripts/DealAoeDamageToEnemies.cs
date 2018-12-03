using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealAoeDamageToEnemies : MonoBehaviour
{

    public Element currentElement = Element.None;
    public float damage;


	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerStay(Collider other)
    {
        print("7 SECONDS HAVE PASSED!!!! ROADO ROLLA DA!!!!!! DIE!" + other.gameObject.name);
        IAttackable thingICanKill = other.GetComponent<IAttackable>();
        if (thingICanKill != null)
        {
            print("IT'S TO LATE! MUDA MUDA MUDA MUDA MUDA MUDA MUDA");
            thingICanKill.TakeDamage(new DamageParams(damage, currentElement, DamageType.Squash));
        }
    }
}
