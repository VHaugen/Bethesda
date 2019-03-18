using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGDirection : MonoBehaviour
{

    public float zebulonPOW;
    Rigidbody rbd;
    // Use this for initialization
    void Start()
    {
        rbd = GetComponent<Rigidbody>();
        rbd.AddForce(new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(5.0f, 10.0f), Random.Range(-5.0f, 5.0f))* zebulonPOW, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
