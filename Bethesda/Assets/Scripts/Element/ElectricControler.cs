using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricControler : MonoBehaviour {

    [SerializeField]
    int initialNumElc = 1;

    [SerializeField]
    Material material;
    public static ElectricControler Get { get; private set; }

    // Use this for initialization
    private void Awake()
    {
        Get = this;
    }

    void Start ()
    {

        GetComponent<MeshRenderer>().material = material;

    }
	
	
}
