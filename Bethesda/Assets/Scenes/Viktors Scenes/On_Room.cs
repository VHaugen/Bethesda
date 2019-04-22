using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_Room : MonoBehaviour
{

    public GameObject[] Fights;
    public GameObject[] puzzls;
    public GameObject door;
    public List<GameObject> tempDoors;
    public bool isFirstRoom;
    public GameObject uppThing;

    // Use this for initialization
    void Start()
    {
        tempDoors = new List<GameObject>();
        if(isFirstRoom == true)
            {
            Instantiate(uppThing, transform.position, Quaternion.identity);
            }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            if(Fights.Length > 0)
            {
                Instantiate(RandFight(), new Vector3(transform.root.position.x, transform.root.position.y, transform.root.position.z), Quaternion.identity);
            }
            if(puzzls.Length > 0)
            {
                Instantiate(RandPuzzel(), new Vector3(transform.root.position.x, transform.root.position.y, transform.root.position.z), Quaternion.identity);
            }   
            // transform.parent.transform.Find("Bridge(Clone)");
            if (transform.parent.transform.Find("Höger/Bridge(Clone)"))
                tempDoors.Add(Instantiate(door, transform.parent.transform.Find("Höger")));

            if (transform.parent.transform.Find("Vänster/Bridge(Clone)"))
                tempDoors.Add(Instantiate(door, transform.parent.transform.Find("Vänster")));

            if (transform.parent.transform.Find("Upp/Bridge(Clone)"))
                tempDoors.Add(Instantiate(door, transform.parent.transform.Find("Upp")));

            if (transform.parent.transform.Find("Ner/Bridge(Clone)"))
                tempDoors.Add(Instantiate(door, transform.parent.transform.Find("Ner")));

            print(GameObject.Find("World"));
            print("HEYYYYEBGYFYUBEFUYBOUYOUYOUBVOUY");
            print(tempDoors.Count);
            print("HEYYYYEBGYFYUBEFUYBOUYOUYOUBVOUY2");
            for (int i = 0; i < tempDoors.Count; i++)
            {
                GameObject.Find("World").GetComponent<PuzzleHandler>().Doors.Add(tempDoors[i]);
            }
    


            //instantiate Puzzle
            Destroy(transform.GetComponent<On_Room>());


        }
    }
    private GameObject RandPuzzel()
    {
        
        int k = Random.Range(0, puzzls.Length);
        
        return puzzls[k];
        
       
    }
    private GameObject RandFight()
    {
        
        
        int k = Random.Range(0, Fights.Length);

        return puzzls[k];
        
    }
}
