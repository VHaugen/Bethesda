using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{


    public Transform target;
    public float distance;

    void Update()
    {

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + distance, target.transform.position.z - distance * 0.67f);

    }
}