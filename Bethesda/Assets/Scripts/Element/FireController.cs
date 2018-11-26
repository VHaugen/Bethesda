using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
	[SerializeField]
	int initialNumFires = 1;

	[SerializeField]
	ParticleSystem firePrefab;

	List<ParticleSystem> fires;

	public static FireController Get { get; private set; }

	private void Awake()
	{
		Get = this;
	}

	void Start()
	{
		fires = new List<ParticleSystem>(initialNumFires);
		for (int i = 0; i < initialNumFires; i++)
		{
			ParticleSystem newFire = Instantiate(firePrefab, transform, true);
			newFire.gameObject.SetActive(false);
			newFire.Stop();
			fires.Add(newFire);
		}
	}

	private void Update()
	{
		foreach (var fire in fires)
		{
			if (fire.shape.meshRenderer == null)
			{
				fire.gameObject.SetActive(false);
				fire.Stop();
			}
		}
	}

	public int NewFire(MeshRenderer attachToMesh)
	{
		for (int i = 0; i < fires.Count; i++)
		{
			var fire = fires[i];
			if (!fire.isEmitting)
			{
				EnableFire(fire, attachToMesh);
				return i;
			}
		}
		// Haven't found an unused slot yet, make a new one
		ParticleSystem newFire = Instantiate(firePrefab, transform, true);
		EnableFire(newFire, attachToMesh);
		fires.Add(newFire);

		return fires.Count - 1;
	}


	public void StopFire(int fireIndex)
	{
		fires[fireIndex].Stop();
		fires[fireIndex].Clear();
	}

	void EnableFire(ParticleSystem fire, MeshRenderer attachToMesh)
	{
		fire.gameObject.SetActive(true);
		fire.Play();
		var shape = fire.shape;
		shape.meshRenderer = attachToMesh;
	}
}
