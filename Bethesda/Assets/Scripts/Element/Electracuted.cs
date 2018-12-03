using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electracuted : MonoBehaviour {

    Material normalMat;

    public float duration;
    public bool infinite = false;

    float elcTimer = -1;
    protected int lightIndex = -1;

    MeshRenderer meshRenderer;

    // Use this for initialization
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
    }

    public void ElcStart()
    {
        if (lightIndex == -1)
        {
            lightIndex = ElectricControler.Get.NewLight(meshRenderer);
        }
        elcTimer = duration;
    }

    public bool IsElc()
    {
        return elcTimer > 0;
    }

    public void ElcStop()
    {
        elcTimer = -1;
        ElectricControler.Get.StopLightning(lightIndex);
        lightIndex = -1;
    }

    void Update()
    {
        if (elcTimer > 0)
        {
            if (!infinite)
            {
                elcTimer -= Time.deltaTime;
            }
            if (elcTimer <= 0)
            {
                ElcStop();
            }
        }
    }

}
