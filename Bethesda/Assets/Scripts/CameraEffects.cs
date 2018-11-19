using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour
{

	static public CameraEffects Get { get; private set; }

	// Screen-shake //
	readonly float singleShakeDuration = 0.01f;
	float timer = -1;
	float timerWithinSingleShake = -1;
	float amplitude;
	float duration;
	Vector3 targetPos;

	// Damage vignette //
	[Header("Damage vignette")]
	float vignetteTargetIntensity;
	float vignetteStartingIntensity;
	[SerializeField] float vignetteIntensityMinOvershootTarget = 0.5f;
	[SerializeField] float vignetteIntensityExtraOvershoot = 0.1f;
	[SerializeField] float vignetteIncreaseDuration = 0.1f;
	[SerializeField] float vignetteDecreaseDuration = 0.1f;
	enum VignetteEffectState { Inactive, Increase, DecreaseFromOvershoot }
	VignetteEffectState vignetteEffectState;
	Vignette vignetteLayer;


	private void Awake()
	{
		Get = this;
		if (transform.localPosition != Vector3.zero || transform.localEulerAngles != Vector3.zero)
			Debug.LogWarning("Camera with CameraEffects should have local position and rotation 0,0,0!! (make it a child to an empty object and change that transform instead)");
		PostProcessVolume postVolume = FindObjectOfType<PostProcessVolume>();
		vignetteLayer = postVolume.profile.GetSetting<Vignette>();
	}

	// Update is called once per frame
	void Update()
	{
		// Screen shake //
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

		// Damage vignette //
		if (vignetteEffectState == VignetteEffectState.Increase)
		{
			float vignetteIntensityOvershoot = Mathf.Max(vignetteTargetIntensity + vignetteIntensityExtraOvershoot, vignetteIntensityMinOvershootTarget);
			vignetteLayer.intensity.value += (vignetteIntensityOvershoot - vignetteStartingIntensity) * Time.deltaTime / vignetteIncreaseDuration;
			if (vignetteLayer.intensity.value >= vignetteIntensityOvershoot)
			{
				vignetteEffectState = VignetteEffectState.DecreaseFromOvershoot;
				vignetteLayer.intensity.value = vignetteIntensityOvershoot;
				vignetteStartingIntensity = vignetteIntensityOvershoot;
			}
		}
		else if (vignetteEffectState == VignetteEffectState.DecreaseFromOvershoot)
		{
			vignetteLayer.intensity.value -= (vignetteStartingIntensity - vignetteTargetIntensity) * Time.deltaTime / vignetteDecreaseDuration;
			if (vignetteLayer.intensity.value <= vignetteTargetIntensity)
			{
				vignetteEffectState = VignetteEffectState.Inactive;
				vignetteLayer.intensity.value = vignetteTargetIntensity;
			}
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

	public void SetDamageVignette(float intensity)
	{
		vignetteStartingIntensity = vignetteLayer.intensity.value;
		vignetteTargetIntensity = intensity;
		vignetteEffectState = VignetteEffectState.Increase;
	}
}
