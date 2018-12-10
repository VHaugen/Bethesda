using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
	[SerializeField]
	int initialNumSystems = 1;

	public ParticleSystem particleSystemPrefab;

	List<ParticleSystem> systems;


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

	public int Spawn(MeshRenderer attachToMesh = null)
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

	void EnableSystem(ParticleSystem system, MeshRenderer attachToMesh)
	{
		system.gameObject.SetActive(true);
		system.Play();
		foreach (ParticleSystem subSystem in system.GetComponentsInChildren<ParticleSystem>())
		{
			var shape = subSystem.shape;
			shape.meshRenderer = attachToMesh; 
		}
	}
}
