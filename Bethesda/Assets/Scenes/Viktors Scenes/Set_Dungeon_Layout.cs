using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Dungeon_Layout : MonoBehaviour {

    private GameObject[,] map;
    public GameObject room;
    public GameObject[] sideRooms;
    public GameObject[] cornerRooms;
    public GameObject[] midRooms;

    public ushort width; //based on direction
    public ushort girth; //based on direction
    private int dir;
    [Range (10.0f,100.0f)]
    public float distanceBetweenRooms;

    // Use this for initialization
    void Start () {
        DrawGrid();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DrawGrid()
    {
        map = new GameObject[width, girth];
        dir = Random.Range(0, 4);

        for (int i = 0; i < width; i++) 
        {
            for (int k = 0; k < girth; k++)
            { 
                map[i, k] = Instantiate(room, new Vector3(k * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
            }
        }
    }
}
