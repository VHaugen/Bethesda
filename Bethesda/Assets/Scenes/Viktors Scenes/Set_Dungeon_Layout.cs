using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Dungeon_Layout : MonoBehaviour
{

    private GameObject[,] map;
    public GameObject room;
    public GameObject startRoom;
    public GameObject[] sideRooms;
    public GameObject[] cornerRooms;
    public GameObject[] midRooms;

    public ushort size;
    private int dir;
    [Range(10.0f, 100.0f)]
    public float distanceBetweenRooms;

    // Use this for initialization
    void Start()
    {
        BuildDungeon();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void DrawGrid()
    {
        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k < size; k++)
            {
                map[i, k] = Instantiate(room, new Vector3(k * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
            }
        }
    }


    void BuildDungeon()
    {
        map = new GameObject[size, size];

        int roomYouCameDownOn = Random.Range(0, size);
        map[0,roomYouCameDownOn] = Instantiate(startRoom, new Vector3(roomYouCameDownOn * distanceBetweenRooms, 0, -1 * distanceBetweenRooms), Quaternion.identity) as GameObject;

        for (int i = 0; i < size; i++)
        {
            int roomOnNextFloor = Random.Range(0, size);
            int spaceToNextplace = roomOnNextFloor - roomYouCameDownOn;
            int miniSpace = spaceToNextplace;

            if (spaceToNextplace < 0)
                miniSpace = spaceToNextplace * -1;

            for (int k = 0; k < miniSpace; k++)
            {
                map[i, k] = Instantiate(startRoom, new Vector3(k * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
                print(roomYouCameDownOn + spaceToNextplace);
            }
        }



    }

}
