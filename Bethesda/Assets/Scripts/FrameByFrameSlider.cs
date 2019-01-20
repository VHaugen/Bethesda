using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameByFrameSlider : MonoBehaviour
{
	public Sprite[] frames;
	public Image image;

	public int frameIdx;

	Slider slider;

	// Use this for initialization
	void Start()
	{
		slider = GetComponent<Slider>();
		slider.onValueChanged.AddListener(OnValueChanged);
	}
	
	void OnValueChanged(float newValue)
	{
		int frameCount = frames.Length;
		frameIdx = frameCount - (int)((newValue / slider.maxValue) * frameCount);
		print("frameidx " + frameIdx);
		if (frameIdx >= 0 && frameIdx < frames.Length)
			image.sprite = frames[frameIdx];
	}
}
