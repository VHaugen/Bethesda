using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloudController : MonoBehaviour
{
	[SerializeField]
	int initialNumClouds = 1;

	[SerializeField]
	ParticleSystem poisonCloudPrefab;

	List<ParticleSystem> poisonClouds;

	public static PoisonCloudController Get { get; private set; }

	private void Awake()
	{
		Get = this;
	}

	void Start()
	{
		poisonClouds = new List<ParticleSystem>(initialNumClouds);
		for (int i = 0; i < initialNumClouds; i++)
		{
			ParticleSystem newPoison = Instantiate(poisonCloudPrefab, transform, true);
			newPoison.gameObject.SetActive(false);
			newPoison.Stop();
			poisonClouds.Add(newPoison);
		}
	}

	private void Update()
	{
		foreach (var poision in poisonClouds)
		{
			if (poision.shape.meshRenderer == null)
			{
				poision.gameObject.SetActive(false);
				poision.Stop();
			}
		}
	}

	public int NewPoisonCloud(MeshRenderer attachToMesh)
	{
		for (int i = 0; i < poisonClouds.Count; i++)
		{
			var poison = poisonClouds[i];
			if (!poison.isEmitting)
			{
				EnablePoison(poison, attachToMesh);
				return i;
			}
		}
		// Haven't found an unused slot yet, make a new one
		ParticleSystem newPoison = Instantiate(poisonCloudPrefab, transform, true);
		EnablePoison(newPoison, attachToMesh);
		poisonClouds.Add(newPoison);

		return poisonClouds.Count - 1;
	}


	public void StopPoison(int poisonIndex)
	{
		poisonClouds[poisonIndex].Stop();
		poisonClouds[poisonIndex].Clear();
	}

	void EnablePoison(ParticleSystem poison, MeshRenderer attachToMesh)
	{
		poison.gameObject.SetActive(true);
		poison.Play();
		var shape = poison.shape;
		shape.meshRenderer = attachToMesh;
	}
}
