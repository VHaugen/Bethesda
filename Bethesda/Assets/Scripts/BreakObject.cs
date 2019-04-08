using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour {


    public Transform smallStone;
   
    public AudioClip impact;
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "AOE")
        {
            Sound.Play(impact, 5f);
            ParticleEffectsManager.GetEffect("ObjectBreak").Spawn(transform.position);
            Destroy(gameObject);
            for (int i = 0; i < 10; i++)
            {
                Instantiate(smallStone, transform.position, Quaternion.identity);
            }
            
        }
    }
}
