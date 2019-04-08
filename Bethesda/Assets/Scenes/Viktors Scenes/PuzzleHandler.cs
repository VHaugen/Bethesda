using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour {

    public List<GameObject> Doors;

    // Use this for initialization
    void Start () {
        Doors = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PuzzelIsCleared()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Destroy(Doors[i]);
        }


    }
}
