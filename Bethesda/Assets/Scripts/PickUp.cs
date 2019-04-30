using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

	public Material mat;
	public Element element;
	public AudioClip impact;

	// These are for pick up effect
	float fadeOutDuration = 0.3f;
	float growAmount = 0.5f;
	float moveUpAmount = 2.0f;

	float hasBeenPickedUp = -1;
	MeshRenderer meshRenderer;

	// Use this for initialization
	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		if (hasBeenPickedUp >= 0)
		{
			transform.localScale += (Vector3.one * growAmount * Time.deltaTime) / fadeOutDuration;
			Color col = meshRenderer.material.color;
			col.a -= Time.deltaTime / fadeOutDuration;
			meshRenderer.material.color = col;
			hasBeenPickedUp += Time.deltaTime;
			transform.position += (Vector3.up * Time.deltaTime * moveUpAmount) / fadeOutDuration;

			if (hasBeenPickedUp >= fadeOutDuration)
			{
				Destroy(gameObject);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			hasBeenPickedUp = 0;
			GetComponent<Collider>().enabled = false;
			GetComponent<WavyMotion>().enabled = false;
			Sound.Play(impact, 1f);
			HammerWeapon weapon = FindObjectOfType<HammerWeapon>();
			if (weapon)
			{
				weapon.SetMaterial(mat);
				weapon.GetComponent<DealDamageToEnemies>().currentElement = element;
			}
			else
			{
				Debug.LogWarning("No weapon found!");
			}
		}
	}
}
