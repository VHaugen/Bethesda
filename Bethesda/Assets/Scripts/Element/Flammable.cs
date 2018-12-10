using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
	public float fireSpreadRadius = 1.0f;
	public float duration = 1.0f;
	public bool infinite = false;

	float fireTimer = -1;
	float fireSpreadTimer = 0;
	protected int fireIndex = -1;
	const float spreadInterval = 0.3f;

	MeshRenderer meshRenderer;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
	}

	public void StartBurning()
	{
		if (fireIndex == -1)
			fireIndex = ParticleEffectsManager.GetEffect("Fire").Spawn(meshRenderer);
		fireTimer = duration;
	}

	public bool IsBurning()
	{
		return fireTimer > 0;
	}

	public void StopBurning()
	{
		fireTimer = -1;
		if (fireIndex != -1)
			ParticleEffectsManager.GetEffect("Fire").Stop(fireIndex);
		fireIndex = -1;
	}

	void Update()
	{
		if (fireTimer > 0)
		{
			if (!infinite)
			{
				fireTimer -= Time.deltaTime; 
			}

			if (fireTimer <= 0)
			{
				StopBurning();
			}

			fireSpreadTimer += Time.deltaTime;
			if (fireSpreadTimer > spreadInterval)
			{
				print("Spread fire!");
				fireSpreadTimer = 0f;
				Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, fireSpreadRadius);
				foreach (Collider colliderToMakeBurn in nearbyColliders)
				{
					Flammable flammable = colliderToMakeBurn.GetComponent<Flammable>();
					if (flammable != null && flammable.gameObject != gameObject && !flammable.IsBurning())
					{
						flammable.StartBurning();
					}
				}
			}
		}
	}

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, fireSpreadRadius);
	}
}
