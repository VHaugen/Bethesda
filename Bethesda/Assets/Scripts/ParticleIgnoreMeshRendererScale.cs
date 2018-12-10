using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleIgnoreMeshRendererScale : MonoBehaviour
{
	public float particleSize = 1.0f;
	ParticleSystem system;
	ParticleSystem.ShapeModule shapeModule;
	ParticleSystem.MainModule mainModule;

	// Use this for initialization
	void Start()
	{
		system = GetComponent<ParticleSystem>();
		shapeModule = system.shape;
		mainModule = system.main;
	}

	// Update is called once per frame
	void Update()
	{
		if (system.IsAlive())
		{
			MeshRenderer meshRenderer = shapeModule.meshRenderer;
			if (meshRenderer)
			{
				Vector3 emitterScale = meshRenderer.transform.lossyScale;
				mainModule.startSize = particleSize / emitterScale.magnitude;

			} 
		}
	}
}
