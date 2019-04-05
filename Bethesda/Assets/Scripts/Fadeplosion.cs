using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadeplosion : MonoBehaviour
{
	public float targetScale = 1.5f;
	public float duration = 0.25f;
	[Tooltip("If left blank, will use Image on this GameObject (if any exists)")]
	public Image targetImage;

	float timer = -1;

	private void Start()
	{
		if (targetImage == null)
		{
			targetImage = GetComponent<Image>();
		}
		targetImage.gameObject.SetActive(false);
	}

	public void Perform()
	{
		timer = 0;
		targetImage.gameObject.SetActive(true);
		targetImage.transform.localScale = Vector3.one;
	}

	// Update is called once per frame
	void Update()
	{
		if (timer == -1)
			return;

		targetImage.transform.localScale = Mathf.Lerp(1, targetScale, timer / duration) * Vector3.one;
		Color color = targetImage.color;
		color.a = Mathf.Lerp(1, 0, timer / duration);
		targetImage.color = color;
		timer += Time.deltaTime;

		if (timer >= duration)
		{
			timer = -1;
			targetImage.gameObject.SetActive(false);
		}
	}
}
