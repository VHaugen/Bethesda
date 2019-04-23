using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    public List<GameObject> Doors;

    // Use this for initialization
    void Start()
    {
        Doors = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PuzzelIsCleared()
    {

        StartCoroutine(WaitIguess2());
        StartCoroutine(WaitIguess());



    }
    IEnumerator WaitIguess()
    {

        yield return new WaitForSeconds(3.2f);
        print(Time.time + "destroy");
        for (int i = 0; i < Doors.Count; i++)
        {
            Destroy(Doors[i]);
        }

    }
    IEnumerator WaitIguess2()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Destroy(Doors[i].GetComponent<BoxCollider>());
            for (int p = 0; p < 20; p++)
            {
                yield return new WaitForSeconds(0.1f);
                Doors[i].transform.Translate(Vector3.up);
            }

            print("iran");
        }

        print(Time.time + "rigid body and colider");

    }
}
