using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceControl : MonoBehaviour {

    [SerializeField]
    int initialNumIces = 1;

    //[SerializeField]
    //Material material;

    [SerializeField]
    ParticleSystem frostPreFab;

    List<ParticleSystem> frost;

    public static IceControl Get { get; private set; }

    private void Awake()
    {
        Get = this;
    }
    void Start ()
    {
        //GetComponent<MeshRenderer>().material = material;
        frost = new List<ParticleSystem>(initialNumIces);
        for (int i = 0; i < initialNumIces; i++)
        {
            ParticleSystem newFrost = Instantiate(frostPreFab, transform, true);
            newFrost.gameObject.SetActive(false);
            newFrost.Stop();
            frost.Add(newFrost);
        }
	}

    private void Update()
    {
        foreach (var fog in frost)
        {
            if (fog.shape.meshRenderer == null)
            {
                fog.gameObject.SetActive(false);
                fog.Stop();
            }
        }
    }

    public int NewFog(MeshRenderer attachToMesh)
    {
        for (int i = 0; i < frost.Count; i++)
        {
            var Fog = frost[i];
            if (!Fog.isEmitting)
            {
                EnableFog(Fog, attachToMesh);
                return i;
            }
        }
        ParticleSystem newFrost = Instantiate(frostPreFab, transform, true);
        EnableFog(newFrost, attachToMesh);
        frost.Add(newFrost);
        return frost.Count - 1;
    }

    public void StopFrost(int frostIndex)
    {
        frost[frostIndex].Stop();
        frost[frostIndex].Clear();
    }

    void EnableFog(ParticleSystem Fog, MeshRenderer attachToMesh)
    {
        Fog.gameObject.SetActive(true);
        Fog.Play();
        var shape = Fog.shape;
        shape.meshRenderer = attachToMesh;
    }
    
   
}
