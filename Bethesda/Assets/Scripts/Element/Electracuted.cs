using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electracuted : MonoBehaviour {

    [SerializeField]
    Material material;

    Material normalMat;

    public float duration;
    public bool infinite = false;

    float elcTimer = -1;

    MeshRenderer meshRenderer;

    // Use this for initialization
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        normalMat = meshRenderer.material;
    }

    public void ElcStart()
    {
        elcTimer = duration;
        meshRenderer.material = material;
    }

    public bool IsElc()
    {
        return elcTimer > 0;
    }

    public void ElcStop()
    {
        meshRenderer.material = normalMat;
        elcTimer = -1;
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
