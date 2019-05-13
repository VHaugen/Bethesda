using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
	static Dictionary<string, ParticleController> effectControllers;

	void Start()
	{
		effectControllers = new Dictionary<string, ParticleController>(transform.childCount);
		foreach (ParticleController effect in GetComponentsInChildren<ParticleController>())
		{
			print("Effect: " + effect.name);
			effectControllers.Add(effect.gameObject.name, effect);
		}
	}

	public static ParticleController GetEffect(string name)
	{
		if (effectControllers.ContainsKey(name))
			return effectControllers[name];
		else
		{
			print("!!!!!! Couldn't find particle effect " + name);
			return null;
		}
	}
}
