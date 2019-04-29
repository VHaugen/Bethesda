using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBite : MonoBehaviour
{
    [SerializeField]
    Material material;

    Material normalMat;

    public float duration;
    public bool infinite = false;

    float iceTimer = -1;
    protected int frostIndex = -1;

    Renderer meshRenderer;

    // Use this for initialization
    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        normalMat = meshRenderer.material;
    }

    // Update is called once per frame
    public void FreezeStart()
    {
        print("yay he froze");
        if (frostIndex == -1)
        {
            frostIndex = ParticleEffectsManager.GetEffect("Ice").Spawn(meshRenderer);
        }
        iceTimer = duration;
        meshRenderer.material = material;
    }

    public bool IsFreezing()
    {
        return iceTimer > 0;
    }

    public void StopFreezing()
    {
        meshRenderer.material = normalMat;
		meshRenderer.material.SetFloat("_TintAmount", 0.0f);
        iceTimer = -1;
        ParticleEffectsManager.GetEffect("Ice").Stop(frostIndex);
        frostIndex = -1;
    }

    void Update()
    {
        if (iceTimer > 0)
        {
            if (!infinite)
            {
                iceTimer -= Time.deltaTime;
            }

            if (iceTimer <= 0)
            {
                StopFreezing();
            }
        }
    }
}
