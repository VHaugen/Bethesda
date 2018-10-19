using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
	readonly float singleShakeDuration = 0.01f;

	float timer = -1;
	float timerWithinSingleShake = -1;
	float amplitude;
	float duration;
	Vector3 targetPos;
	static public CameraEffects Get { get; private set; }

	private void Awake()
	{
		Get = this;
		if (transform.localPosition != Vector3.zero || transform.localEulerAngles != Vector3.zero)
			Debug.LogWarning("Camera with CameraEffects should have local position and rotation 0,0,0!! (make it a child to an empty object and change that transform instead)");
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.G))
		//{
		//	Shake(0.2f, 0.5f);
		//}

		if (timer >= 0)
		{
			Vector3 pos = transform.localPosition;
			pos = Vector3.MoveTowards(pos, targetPos, Time.deltaTime / singleShakeDuration);
			timer += Time.deltaTime;
			timerWithinSingleShake += Time.deltaTime;
			if (timer > duration)
			{
				timer = -1;
				pos = Vector3.zero;
				amplitude = 0;
				duration = 0;
			}
			if (timerWithinSingleShake > singleShakeDuration)
			{
				timerWithinSingleShake = 0;
				PickNewPosition();
			}
			transform.localPosition = pos;

		}
	}

	void PickNewPosition()
	{
		targetPos = Random.insideUnitSphere * amplitude;
	}

	public void Shake(float amplitude, float duration)
	{
		timer = 0;
		timerWithinSingleShake = 0;
		PickNewPosition();
		if (amplitude > this.amplitude)
			this.amplitude = amplitude;
		if (duration > this.duration)
			this.duration = duration;
	}
}
