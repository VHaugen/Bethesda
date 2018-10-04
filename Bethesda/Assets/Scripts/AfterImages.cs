using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AfterImages : MonoBehaviour
{
	[SerializeField]
	bool preview = false;

	[SerializeField]
	int numberOfImages = 3;

	[SerializeField]
	float timeBetweenImages = 0.5f;

	[SerializeField]
	float fadeOutDuration = 1f;

	[SerializeField]
	[Range(0, 1)]
	float startingOpacity = 0.5f;

	[SerializeField]
	Color tintColor;

	[SerializeField]
	[Range(0, 1)]
	float tintFactor = 0f;

	GameObject[] imageObjects;
	MeshFilter meshFilter;
	int currentIndex = -1;
	float timer = 0;

	void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
	}

	void Start()
	{
#if UNITY_EDITOR
		if (preview && !Application.isPlaying)
			return;
#endif

		imageObjects = new GameObject[numberOfImages];
		string baseName = "AfterImage--" + gameObject.name + "--";
		Material afterImageMaterial = CreateMaterial();

		for (int i = 0; i < numberOfImages; i++)
		{
			GameObject obj = new GameObject(baseName + i, typeof(AfterImageRenderer), typeof(MeshRenderer), typeof(MeshFilter));
			obj.SetActive(false);
			obj.GetComponent<MeshRenderer>().sharedMaterial = afterImageMaterial;
			obj.GetComponent<AfterImageRenderer>().fadeOutDuration = fadeOutDuration;
			imageObjects[i] = obj;
		}
	}

	Material CreateMaterial()
	{
		Material afterImageMaterial = new Material(Shader.Find("Custom/AfterImage"));
		Material ownerMaterial = GetComponent<MeshRenderer>().sharedMaterial;
		afterImageMaterial.SetTexture("_MainTex", ownerMaterial.GetTexture("_MainTex"));
		afterImageMaterial.SetColor("_Color", ownerMaterial.GetColor("_Color"));
		afterImageMaterial.SetColor("_TintColor", tintColor);
		afterImageMaterial.SetFloat("_TintFactor", tintFactor);

		return afterImageMaterial;
	}

	public void Show()
	{
		currentIndex = 0;
		CreateAfterImage(0);
		timer = 0;
	}

	void Update()
	{
		if (currentIndex != -1)
		{
			timer += Time.deltaTime;
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

	void OnRenderObject()
	{
#if UNITY_EDITOR
		if (!preview || Application.isPlaying)
			return;
#endif

		Material afterImageMaterial = CreateMaterial();
		afterImageMaterial.SetFloat("_Opacity", startingOpacity);
		afterImageMaterial.SetPass(0);
		Matrix4x4 matrix = Matrix4x4.TRS(transform.position + Vector3.back * 2f, transform.rotation, transform.lossyScale);
		Graphics.DrawMeshNow(meshFilter.sharedMesh, matrix);
	}

	void CreateAfterImage(int index)
	{
		GameObject obj = imageObjects[index];
		AfterImageRenderer afterImage = obj.GetComponent<AfterImageRenderer>();
		afterImage.Show(startingOpacity, transform);
		obj.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
	}
}
