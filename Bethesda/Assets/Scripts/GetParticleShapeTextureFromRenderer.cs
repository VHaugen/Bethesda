using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetParticleShapeTextureFromRenderer : MonoBehaviour
{
	ParticleSystem partSys;

	// Use this for initialization
	void Awake()
	{
		partSys = GetComponent<ParticleSystem>();
	}

	public void MakeTextureTheRightOne()
	{
		var shape = partSys.shape;
		if (shape.shapeType == ParticleSystemShapeType.MeshRenderer)
		{
			shape.texture = (Texture2D)shape.meshRenderer.material.mainTexture;
		}
		else if (shape.shapeType == ParticleSystemShapeType.SkinnedMeshRenderer)
		{
			shape.texture = (Texture2D)shape.skinnedMeshRenderer.material.mainTexture;
		}
	}
}
