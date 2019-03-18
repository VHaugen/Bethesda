using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FrameByFrameSlider : MonoBehaviour
{
	public Sprite[] frames;
	public Image image;
	public Image whitenessImage;
	public float whitenessDuration = 1.0f;

	[SerializeField] float _maxValue;
	public float maxValue
	{
		get
		{
			return _maxValue;
		}
		set
		{
			_maxValue = value;
			UpdateVisuals();
		}
	}

	[SerializeField] float _value;
	public float value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			UpdateVisuals();
		}
	}

	public int frameIdx;

	// Use this for initialization
	void Start()
	{
	}

	void Update()
	{
		if (whitenessImage)
		{
			if (whitenessImage.color.a > 0)
			{
				Color col = whitenessImage.color;
				col.a -= Time.deltaTime / whitenessDuration;
				whitenessImage.color = col;
			}
		}
	}

	void UpdateVisuals()
	{
		if (whitenessImage)
		{
			if (whitenessImage.color.a < 0.5f)
			{
				if (whitenessImage.color.a < 0.1f)
				{
					whitenessImage.sprite = image.sprite;
				}
				whitenessImage.color = Color.white; 
			}
		}
		int frameCount = frames.Length;
		frameIdx = frameCount - Mathf.CeilToInt((value / maxValue) * frameCount);
		frameIdx = Mathf.Clamp(frameIdx, 0, frames.Length - 1);
		image.sprite = frames[frameIdx];
	}

	private void OnValidate()
	{
		value = _value;
		maxValue = _maxValue;
	}
}
