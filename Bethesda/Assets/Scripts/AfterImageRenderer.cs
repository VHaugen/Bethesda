using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
public class AfterImageRenderer : MonoBehaviour
{
	public float fadeOutDuration;

	float opacity = 1f;
	MaterialPropertyBlock propertyBlock;
	MeshRenderer meshRenderer;


	// Use this for initialization
	void Start()
	{
		propertyBlock = new MaterialPropertyBlock();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	public void Show(float startingOpacity, Transform trans)
	{
		opacity = startingOpacity;
		transform.position = trans.position;
		transform.localScale = trans.lossyScale;
		transform.rotation = trans.rotation;
		gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update()
	{
		propertyBlock.SetFloat("_Opacity", opacity);
		meshRenderer.SetPropertyBlock(propertyBlock);

		opacity -= Time.deltaTime / fadeOutDuration;
		if (opacity <= 0)
		{
			gameObject.SetActive(false);
		}
	}
}
