using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImages : MonoBehaviour
{
	[SerializeField]
	int numberOfImages = 3;

	public float duration = 1.0f;

	[SerializeField]
	float fadeOutDuration = 1f;

	[SerializeField]
	[Range(0, 1)]
	float startingOpacity = 0.5f;

	[SerializeField]
	Color tintColor = Color.white;

	[SerializeField]
	[Range(0, 1)]
	float tintFactor = 0f;

	AfterImageRenderer[] imageObjects;
	MeshFilter meshFilter;
	SkinnedMeshRenderer skinnedMeshRenderer;
	Material afterImageMaterial;
	int currentIndex = -1;
	float timer = 0;
	bool hasSkinnedRenderer;

	void Awake()
	{
		// One of these will return null, most likely
		meshFilter = GetComponent<MeshFilter>();
		skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
	}

	void Start()
	{
		imageObjects = new AfterImageRenderer[numberOfImages];
		string baseName = "AfterImage--" + gameObject.name + "--";
		afterImageMaterial = CreateMaterial();

		for (int i = 0; i < numberOfImages; i++)
		{
			GameObject obj = new GameObject(baseName + i, typeof(AfterImageRenderer), typeof(MeshRenderer), typeof(MeshFilter));
			obj.SetActive(false);
			obj.GetComponent<MeshRenderer>().sharedMaterial = afterImageMaterial;
			obj.GetComponent<AfterImageRenderer>().fadeOutDuration = fadeOutDuration;
			imageObjects[i] = obj.GetComponent<AfterImageRenderer>();
		}
	}

	Material CreateMaterial()
	{
		afterImageMaterial = new Material(Shader.Find("Custom/AfterImage"));
		Material ownerMaterial = GetComponentInChildren<Renderer>().sharedMaterial;
		afterImageMaterial.SetTexture("_MainTex", ownerMaterial.GetTexture("_MainTex"));
		afterImageMaterial.SetColor("_Color", ownerMaterial.GetColor("_Color"));
		afterImageMaterial.SetColor("_TintColor", tintColor);
		afterImageMaterial.SetFloat("_TintFactor", tintFactor);

		return afterImageMaterial;
	}

	public void Show()
	{
		currentIndex = 0;
		afterImageMaterial = CreateMaterial();
		CreateAfterImage(0);
		timer = 0;
	}

	void Update()
	{
		if (currentIndex != -1)
		{
			timer += Time.deltaTime;
			float timeBetweenImages = duration / numberOfImages;
			if (timer > timeBetweenImages)
			{
				currentIndex++;
				if (currentIndex >= numberOfImages)
				{
					currentIndex = -1;
				}
				else
				{
					CreateAfterImage(currentIndex);
					timer = 0;
				}
			}
		}
	}

	void CreateAfterImage(int index)
	{
		AfterImageRenderer afterImage = imageObjects[index];
		afterImage.Show(startingOpacity, transform);
		afterImage.GetComponent<MeshRenderer>().sharedMaterial = afterImageMaterial;
		if (meshFilter)
		{
			afterImage.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
		}
		else if (skinnedMeshRenderer)
		{
			skinnedMeshRenderer.BakeMesh(afterImage.GetComponent<MeshFilter>().mesh);
		}
	}
}
