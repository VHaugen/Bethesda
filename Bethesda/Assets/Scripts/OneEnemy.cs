using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEnemy : MonoBehaviour
{
	public GameObject enemyPrefab;

	// Use this for initialization
	void Start()
	{
		Instantiate(enemyPrefab);
	}

	private void OnDrawGizmos()
	{
		if (enemyPrefab)
		{
			MeshFilter meshFilter = enemyPrefab.GetComponent<MeshFilter>();
			if (meshFilter)
			{
				Gizmos.DrawWireMesh(meshFilter.sharedMesh, transform.position + enemyPrefab.transform.position, transform.rotation * enemyPrefab.transform.rotation, Vector3.Scale(transform.lossyScale, enemyPrefab.transform.lossyScale));
			}
			else
			{
				SkinnedMeshRenderer meshRenderer = enemyPrefab.GetComponent<SkinnedMeshRenderer>();
				if (meshRenderer)
				{
					Gizmos.DrawWireMesh(meshRenderer.sharedMesh, transform.position + enemyPrefab.transform.position, transform.rotation * enemyPrefab.transform.rotation, Vector3.Scale(transform.lossyScale, enemyPrefab.transform.lossyScale));
				}
			}
		}
		else
		{
			Gizmos.DrawSphere(transform.position, 2.0f);
		}
	}
}
