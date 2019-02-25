using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Dungeon_Layout : MonoBehaviour
{
    //TODO :
    //
    //      -Add funtion for placement of door 
    //      -Prop Spawnin
    //      -Special Rooms
    //


    private GameObject[,] map;
    private GameObject bossRoom;
    public GameObject[] room;
    public GameObject startRoom;
    public GameObject wall;
    public GameObject door;
    public GameObject specialRoom;
    public GameObject Connector;
    public GameObject Player;
    public GameObject colider;
    public GameObject spawnObject;

    private string[] direction;
    public ushort size;
    [Range(10.0f, 100.0f)]
    public float distanceBetweenRooms;

    // Use this for initialization
    void Start()
    {
        direction = new string[4] { "Höger", "Vänster", "Upp", "Ner" };

       // DrawGrid();
        BuildDungeon();


    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    private GameObject RandomRoomType()
    {
        return room[Random.Range(0, room.Length)];
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

        //first room before dungenon 
        //SPAWN just a path forward

        map[0, roomYouCameDownOn] = Instantiate(startRoom, new Vector3(roomYouCameDownOn * distanceBetweenRooms, 0, -1 * distanceBetweenRooms), Quaternion.identity) as GameObject;
        Player.transform.position = new Vector3(roomYouCameDownOn * distanceBetweenRooms, 0, -1 * distanceBetweenRooms);
        Instantiate(spawnObject, new Vector3((roomYouCameDownOn * distanceBetweenRooms) + 1,2, -1 * distanceBetweenRooms), Quaternion.identity);
        // Player.transform.position = new Vector3(roomYouCameDownOn * distanceBetweenRooms, 0, -1 * distanceBetweenRooms);

        for (int i = 0; i < size; i++)
        {

            int roomOnNextFloor = Random.Range(0, size);
            int spaceToNextplace = roomOnNextFloor - roomYouCameDownOn;
            int miniSpace = spaceToNextplace;


            // Entrace on Floor 




            //


            if (spaceToNextplace > 0)
            {
                //for when the algorithm is going right

                if (roomOnNextFloor < (size - 1))
                {
                    map[i, roomOnNextFloor - 1] = Instantiate(specialRoom, new Vector3((roomOnNextFloor + 1) * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
                    //*Instantiate(door, map[i, roomOnNextFloor - 1].transform.Find("Vänster").transform);
                    Instantiate(Connector, map[i, roomOnNextFloor - 1].transform.Find("Vänster").transform);
                    //setDoors
                }
                //going right
                for (int k = 0; k < miniSpace + 1; k++)
                {
                    //Once per room on level
                    map[i, roomYouCameDownOn + k] = Instantiate(RandomRoomType(), new Vector3((roomYouCameDownOn + k) * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;

                    if (roomOnNextFloor == roomYouCameDownOn)
                    {
                        //*Instantiate(door, map[i, roomYouCameDownOn].transform.Find("Ner").transform);
                        Instantiate(Connector, map[i, roomYouCameDownOn].transform.Find("Ner").transform);
                        if (miniSpace > 0)
                        {
                            //*Instantiate(door, map[i, roomYouCameDownOn].transform.Find("Höger").transform);
                            Instantiate(Connector, map[i, roomYouCameDownOn].transform.Find("Höger").transform);

                        }
                    }
                    else if (k != 0)
                    {
                        //*Instantiate(door, map[i, roomYouCameDownOn + k].transform.Find("Vänster").transform);
                        Instantiate(Connector, map[i, roomYouCameDownOn + k].transform.Find("Vänster").transform);

                        if (k != miniSpace)
                        {
                            //*Instantiate(door, map[i, roomYouCameDownOn + k].transform.Find("Höger").transform);
                            Instantiate(Connector, map[i, roomYouCameDownOn + k].transform.Find("Höger").transform);

                        }

                    }

                    if (k == 0 && miniSpace > 0)// gör om
                    {
                        //*Instantiate(door, map[i, roomYouCameDownOn + k].transform.Find("Höger").transform);
                        Instantiate(Connector, map[i, roomYouCameDownOn + k].transform.Find("Höger").transform);

                    }

                    for (int p = 0; p < 4; p++)
                    {
                        if (map[i, roomYouCameDownOn + k].transform.Find(direction[p]).transform.childCount == 0)
                        {
                            Instantiate(wall, map[i, roomYouCameDownOn + k].transform.Find(direction[p]).transform);
                        }
                    }

                }

            }

            else
            {
                if (roomOnNextFloor > 0)
                {// && !map[i, roomOnNextFloor - 1]
                    map[i, roomOnNextFloor - 1] = Instantiate(specialRoom, new Vector3((roomOnNextFloor - 1) * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
                    //set Doors
                }
                //set miniSpace for moving left
                miniSpace = spaceToNextplace * -1;

                //for when the algorithm is going left
                for (int k = 0; k < miniSpace + 1; k++)
                {
                    //Once per room on level
                    map[i, roomYouCameDownOn - k] = Instantiate(RandomRoomType(), new Vector3((roomYouCameDownOn - k) * distanceBetweenRooms, 0, i * distanceBetweenRooms), Quaternion.identity) as GameObject;
                    //Instantiate(door, map[i, roomOnNextFloor - 1].transform.Find("Höger").transform); ////// DETTA KAN GE  ERROR

                    if (k == 0 && miniSpace > 0)
                    {
                        //*Instantiate(door, map[i, roomYouCameDownOn - k].transform.Find("Vänster").transform);
                        Instantiate(Connector, map[i, roomYouCameDownOn - k].transform.Find("Vänster").transform);


                    }
                    else if (k != 0)
                    {
                        //*Instantiate(door, map[i, roomYouCameDownOn - k].transform.Find("Höger").transform);
                        Instantiate(Connector, map[i, roomYouCameDownOn - k].transform.Find("Höger").transform);

                        if (k != miniSpace)
                        {
                            //*Instantiate(door, map[i, roomYouCameDownOn - k].transform.Find("Vänster").transform);
                            Instantiate(Connector, map[i, roomYouCameDownOn - k].transform.Find("Vänster").transform);

                        }
                    }


                    for (int p = 0; p < 4; p++)
                    {
                        if (map[i, roomYouCameDownOn - k].transform.Find(direction[p]).transform.childCount == 0)
                        {
                            Instantiate(wall, map[i, roomYouCameDownOn - k].transform.Find(direction[p]).transform);
                        }
                    }

                }
            }
            //going left

            // Exit from floor

            //*Instantiate(door, map[i, roomYouCameDownOn].transform.Find("Ner").transform);
            Instantiate(Connector, map[i, roomYouCameDownOn].transform.Find("Ner").transform);
           // Destroy(map[i, roomYouCameDownOn].transform.Find("Ner").transform.Find("Wall"));
            Destroy(map[i, roomYouCameDownOn].transform.Find("Ner").Find("Wall(Clone)").gameObject);

            if (map[i, roomYouCameDownOn].transform.Find("Ner").Find("Wall(Clone)") == true)
            {
                print("i work i found Ner");

            }
            
            // .transform.FindChild("Wall(clone)"));

            // fixa att jag sätter väg på dörr
            if (i != 0)
            {
                //*Instantiate(door, map[i - 1, roomYouCameDownOn].transform.Find("Upp").transform);
                Instantiate(Connector, map[i - 1, roomYouCameDownOn].transform.Find("Upp").transform);
               // Destroy(map[i - 1, roomYouCameDownOn].transform.Find("Upp").transform.Find("Wall"));
                Destroy(map[i - 1, roomYouCameDownOn].transform.Find("Upp").Find("Wall(Clone)").gameObject);


            }

            roomYouCameDownOn = roomOnNextFloor;


            //break; ////////////////////// du breakar här
        }
        bossRoom = Instantiate(startRoom, new Vector3(roomYouCameDownOn * distanceBetweenRooms, 15, (1 + size) * distanceBetweenRooms), Quaternion.identity) as GameObject;

    }

}
