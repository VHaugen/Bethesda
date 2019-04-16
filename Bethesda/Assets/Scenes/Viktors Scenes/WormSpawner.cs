using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawner : MonoBehaviour {

    private PuzzleHandler PuzzelHandler;


    // Use this for initialization
    void Start () {
        PuzzelHandler = GameObject.Find("World").GetComponent<PuzzleHandler>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
