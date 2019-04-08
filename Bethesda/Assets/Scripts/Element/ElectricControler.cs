using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricControler : MonoBehaviour
{

    [SerializeField]
    int initialNumElc = 1;


    [SerializeField]
    ParticleSystem lightningPreFab;

    List<ParticleSystem> lightning;

    public static ElectricControler Get { get; private set; }

    // Use this for initialization
    private void Awake()
    {
        Get = this;
    }

    void Start()
    {

        lightning = new List<ParticleSystem>(initialNumElc);
        for (int i = 0; i < initialNumElc; i++)
        {
            ParticleSystem newLightning = Instantiate(lightningPreFab, transform, true);
            newLightning.gameObject.SetActive(false);
            newLightning.Stop();
            lightning.Add(newLightning);
        }

    }

    private void Update()
    {
        foreach (var lights in lightning)
        {
            if (lights.shape.meshRenderer == null)
            {
                lights.gameObject.SetActive(false);
                lights.Stop();
            }
        }
    }

    public int NewLight(MeshRenderer attachToMesh)
    {
        for (int i = 0; i < lightning.Count; i++)
        {
            var light = lightning[i];
            if (!light.isEmitting)
            {
                EnableLight(light, attachToMesh);
                return i;
            }
        }
        ParticleSystem newLightning = Instantiate(lightningPreFab, transform, true);
        EnableLight(newLightning, attachToMesh);
        lightning.Add(newLightning);
        return lightning.Count - 1;
    }


    public void StopLightning(int lightningIndex)
    {
        lightning[lightningIndex].Stop();
        lightning[lightningIndex].Clear();
    }

    void EnableLight(ParticleSystem light, MeshRenderer attachToMesh)
    {
        light.gameObject.SetActive(true);
        light.Play();
        var shape = light.shape;
        shape.meshRenderer = attachToMesh;
    }


}
