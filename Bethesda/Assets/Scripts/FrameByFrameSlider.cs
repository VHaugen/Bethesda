using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FrameByFrameSlider : MonoBehaviour
{
	public Sprite[] frames;
	public Image image;

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
	
	void UpdateVisuals()
	{
		int frameCount = frames.Length;
		frameIdx = frameCount - (int)((value / maxValue) * frameCount);
		if (frameIdx >= 0 && frameIdx < frames.Length)
			image.sprite = frames[frameIdx];
	}

	private void OnValidate()
	{
		value = _value;
		maxValue = _maxValue;
	}
}
