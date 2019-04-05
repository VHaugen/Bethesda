using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
	public float duration;

	float timer;

	// Use this for initialization
	void Start()
	{
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	public void Attack()
	{
		gameObject.SetActive(true);
		timer = duration;
	}

	private void Update()
	{
		if (timer > 0)
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				timer = 0;
				gameObject.SetActive(false);
			}
		}
	}

	void Reset()
	{
		gameObject.tag = "enemy";
		GetComponent<Collider>().isTrigger = true;
	}

	private void OnDrawGizmos()
	{
		Collider collider = GetComponent<Collider>();
		if (collider is SphereCollider)
		{
			var sc = collider as SphereCollider;
			Vector3 scale = transform.lossyScale;
			Gizmos.DrawWireSphere(transform.position + sc.center, sc.radius * Mathf.Max(scale.x, scale.y, scale.z));
		}
	}
}
