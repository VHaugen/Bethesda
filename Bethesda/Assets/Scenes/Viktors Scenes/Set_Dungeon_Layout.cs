using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Dungeon_Layout : MonoBehaviour
{

    private GameObject[,] map;
    private GameObject bossRoom;
    public GameObject room;
    public GameObject startRoom;

    public ushort size;
    private int dir;
    [Range(10.0f, 100.0f)]
    public float distanceBetweenRooms;

    // Use this for initialization
    void Start()
    {
        DrawGrid();
        BuildDungeon();

    }

    // Update is called once per frame
    void Update()
    {
    }

    void DrawGrid()
    {
        map = new GameObject[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k < size; k++)
            {
                map[i, k] = Instantiate(startRoom, new Vector3(k * distanceBetweenRooms, -10, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
            }
        }
    }


    void BuildDungeon()
    {
        map = new GameObject[size, size];

        int roomYouCameDownOn = Random.Range(0, size);
        map[0, roomYouCameDownOn] = Instantiate(startRoom, new Vector3(roomYouCameDownOn * distanceBetweenRooms, 0, -1 * distanceBetweenRooms), Quaternion.identity) as GameObject;

        for (int i = 0; i < size; i++)
        {
            int roomOnNextFloor = Random.Range(0, size);
            int spaceToNextplace = roomOnNextFloor - roomYouCameDownOn;
            int miniSpace = spaceToNextplace;

            if (spaceToNextplace > 0)
            {
                

                //going right
                for (int k = 0; k < miniSpace + 1; k++)
                {
                    map[i, roomYouCameDownOn + k] = Instantiate(room, new Vector3((roomYouCameDownOn + k) * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
                    //print(" roomyoucamedownon + spacetonextplace" + roomYouCameDownOn + spaceToNextplace);
                    print("right");
                }
            }
            else
            {
                miniSpace = spaceToNextplace * -1;
                for (int k = 0; k < miniSpace + 1; k++)
                {
                    map[i, roomYouCameDownOn - k] = Instantiate(room, new Vector3((roomYouCameDownOn - k) * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
                    //print(" roomyoucamedownon + spacetonextplace" + roomYouCameDownOn + spaceToNextplace);
                    print("left");

                }
            }
            //going left

            roomYouCameDownOn = roomOnNextFloor;
            //break; ////////////////////// du breakar här
        }
        bossRoom = Instantiate(startRoom, new Vector3(roomYouCameDownOn * distanceBetweenRooms, 15, (1 + size ) * distanceBetweenRooms), Quaternion.identity) as GameObject;




    }

}
