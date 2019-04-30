using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
	[SerializeField] int initialNumSystems = 1;
	[SerializeField] ParticleSystem particleSystemPrefab;
	[SerializeField] bool destroySystemWhenMeshIsNull = true;

	List<ParticleSystem> systems;
	List<ParticleSystem> meshAttachedSystems;
	bool skinnedMeshRenderer = false;

	void Start()
	{
		systems = new List<ParticleSystem>(initialNumSystems);
		meshAttachedSystems = new List<ParticleSystem>(initialNumSystems);
		for (int i = 0; i < initialNumSystems; i++)
		{
			ParticleSystem newSystem = Instantiate(particleSystemPrefab, transform, true);
			newSystem.gameObject.SetActive(false);
			newSystem.Stop();
			newSystem.name += i;
			systems.Add(newSystem);

		}
	}

	private void Update()
	{
		if (!destroySystemWhenMeshIsNull)
			return;

		foreach (var system in meshAttachedSystems)
		{
			if (system.shape.meshRenderer == null && system.shape.skinnedMeshRenderer == null)
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
				meshAttachedSystems.Add(system);
				EnableSystem(system, attachToMesh);
				return i;
			}
		}
		// Haven't found an unused slot yet, make a new one
		ParticleSystem newSystem = Instantiate(particleSystemPrefab, transform, true);
		//var partSettings = newFire.main;
		//partSettings.scalingMode = ParticleSystemScalingMode.Shape;
		EnableSystem(newSystem, attachToMesh);
		newSystem.name += systems.Count;
		systems.Add(newSystem);
		meshAttachedSystems.Add(newSystem);

		return systems.Count - 1;
	}

	public int Spawn(Vector3 position, Quaternion rotation)
	{
		for (int i = 0; i < systems.Count; i++)
		{
			var system = systems[i];
			if (!system.isEmitting)
			{
				EnableSystem(system, null, position, rotation);
				return i;
			}
		}
		// Haven't found an unused slot yet, make a new one
		ParticleSystem newSystem = Instantiate(particleSystemPrefab, transform, true);
		//var partSettings = newFire.main;
		//partSettings.scalingMode = ParticleSystemScalingMode.Shape;
		EnableSystem(newSystem, null, position);
		newSystem.name += systems.Count;
		systems.Add(newSystem);

		return systems.Count - 1;
	}

	public int Spawn(Vector3 position)
	{
		return Spawn(position, Quaternion.identity);
	}

	public int Spawn(Transform transform)
	{
		return Spawn(transform.position, transform.rotation);
	}


	public void Stop(int index)
	{
		var shape = systems[index].shape;
		shape.skinnedMeshRenderer = null;
		shape.meshRenderer = null;
		systems[index].Stop();
		systems[index].Clear();
		meshAttachedSystems.Remove(systems[index]);
	}

	void EnableSystem(ParticleSystem system, Renderer attachToMesh, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
	{
		system.gameObject.SetActive(true);
		system.Play();
		foreach (ParticleSystem subSystem in system.GetComponentsInChildren<ParticleSystem>())
		{
			if (attachToMesh)
			{
				var shape = subSystem.shape;
				if (attachToMesh is MeshRenderer)
				{
					shape.shapeType = ParticleSystemShapeType.MeshRenderer;
					shape.meshRenderer = (MeshRenderer)attachToMesh;
					shape.skinnedMeshRenderer = null;
				}
				else if (attachToMesh is SkinnedMeshRenderer)
				{
					shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
					shape.skinnedMeshRenderer = (SkinnedMeshRenderer)attachToMesh;
					print("Enable a skinnedmeshrenderer particle fx on " + attachToMesh.name + "; " + system.name);
					shape.meshRenderer = null;
				}
				else
				{
					Debug.LogWarning("Renderer must be either a MeshRenderer or SkinnedMeshRenderer " + attachToMesh.gameObject.name);
				} 

				var textureSetter = system.GetComponent<GetParticleShapeTextureFromRenderer>();
				if (textureSetter)
				{
					textureSetter.MakeTextureTheRightOne();
				}
			}
			else
			{
				system.transform.position = position;
				system.transform.rotation = rotation;
			}
		}
	}
}
