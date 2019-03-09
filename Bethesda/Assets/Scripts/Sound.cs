using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
	static AudioSource audioSource;

	static void CreateInstance()
	{
		GameObject go = new GameObject("Sound");
		DontDestroyOnLoad(go);
		go.AddComponent<Sound>();
		audioSource = go.AddComponent<AudioSource>();
	}

	static public void Play(AudioClip audioClip, float volumeScale = 1.0f)
	{
		if (!audioSource)
			CreateInstance();
		audioSource.PlayOneShot(audioClip, volumeScale);
	}

}
