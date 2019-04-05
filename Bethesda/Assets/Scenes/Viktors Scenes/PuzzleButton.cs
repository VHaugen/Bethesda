using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour, IAttackable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public virtual void TakeDamage(DamageParams args)
    {
        print("i Was Hit amarifht");

        Destroy(transform.GetComponent<PuzzleButton>());
        transform.parent.GetComponent<PuzzelButtonHandler>().TriggerWasHit();
		
    }
}
