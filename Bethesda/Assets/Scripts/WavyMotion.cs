using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavyMotion : MonoBehaviour
{
	public float period = 1;
	public float height = 1;

	public float rotatePeriod = 1;

	[HideInInspector] public Vector3 basePosition;
	float randomId;

	// Use this for initialization
	void Start()
	{
		basePosition = transform.position;
		randomId = transform.position.x + transform.position.y;
	}

	// Update is called once per frame
	void Update()
	{
		float offset = Mathf.Sin((Time.time + randomId) * 2 * Mathf.PI / period) * height;
		transform.position = basePosition + Vector3.up * offset;
		if (rotatePeriod != 0)
		{
			transform.Rotate(0, (Time.deltaTime * 360f) / rotatePeriod, 0, Space.World);
		}
	}
}
