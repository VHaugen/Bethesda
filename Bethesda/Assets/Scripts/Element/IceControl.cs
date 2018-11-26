using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceControl : MonoBehaviour {

    [SerializeField]
    int initialNumIces = 1;

    [SerializeField]
    Material material;
    public static IceControl Get { get; private set; }

    private void Awake()
    {
        Get = this;
    }
    void Start ()
    {
        GetComponent<MeshRenderer>().material = material;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
