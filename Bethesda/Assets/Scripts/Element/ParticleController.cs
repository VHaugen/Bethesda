﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
	[SerializeField]
	int initialNumSystems = 1;

	public ParticleSystem particleSystemPrefab;

	List<ParticleSystem> systems;
	bool skinnedMeshRenderer = false;


	void Start()
	{
		systems = new List<ParticleSystem>(initialNumSystems);
		for (int i = 0; i < initialNumSystems; i++)
		{
			ParticleSystem newSystem = Instantiate(particleSystemPrefab, transform, true);
			newSystem.gameObject.SetActive(false);
			newSystem.Stop();
			systems.Add(newSystem);

		}
	}

	private void Update()
	{
		foreach (var system in systems)
		{
			if (system.shape.meshRenderer == null)
			{
				system.gameObject.SetActive(false);
				system.Stop();
			}
		}
	}

	public int Spawn(Renderer attachToMesh = null)
	{
		for (int i = 0; i < systems.Count; i++)
		{
			var system = systems[i];
			if (!system.isEmitting)
			{
				EnableSystem(system, attachToMesh);
				return i;
			}
		}
		// Haven't found an unused slot yet, make a new one
		ParticleSystem newSystem = Instantiate(particleSystemPrefab, transform, true);
		//var partSettings = newFire.main;
		//partSettings.scalingMode = ParticleSystemScalingMode.Shape;
		EnableSystem(newSystem, attachToMesh);
		systems.Add(newSystem);

		return systems.Count - 1;
	}


	public void Stop(int index)
	{
		systems[index].Stop();
		systems[index].Clear();
	}

	void EnableSystem(ParticleSystem system, Renderer attachToMesh)
	{
		system.gameObject.SetActive(true);
		system.Play();
		foreach (ParticleSystem subSystem in system.GetComponentsInChildren<ParticleSystem>())
		{
			var shape = subSystem.shape;
			if (attachToMesh is MeshRenderer)
			{
				shape.shapeType = ParticleSystemShapeType.MeshRenderer;
				shape.meshRenderer = (MeshRenderer)attachToMesh;
				skinnedMeshRenderer = false;
			}
			else if (attachToMesh is SkinnedMeshRenderer)
			{
				shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
				shape.skinnedMeshRenderer = (SkinnedMeshRenderer)attachToMesh;
				skinnedMeshRenderer = true;
			}
			else
			{
				Debug.LogWarning("Renderer must be either a MeshRenderer or SkinnedMeshRenderer " + attachToMesh.gameObject.name);
			}
		}
	}
}
