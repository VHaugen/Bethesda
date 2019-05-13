using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	const float fadeDurationToBattle = 2.0f;
	const float fadeDurationToCalm = 5.0f;

	public enum Music
	{
		Calm,
		Battle
	}
	static public Music currentSong { get; private set; }

	public static int numEnemies = 0;

	public AudioSource battleMusicSource;
	public AudioSource calmMusicSource;

	public static MusicController instance { get; private set; }

	float battleVolume;
	float calmVolume;

	// Use this for initialization
	void Awake()
	{
		battleVolume = battleMusicSource.volume;
		calmVolume = calmMusicSource.volume;
		currentSong = Music.Calm;
		instance = this;
	}

	static public void DisableMusic()
	{
		instance.battleMusicSource.enabled = false;
		instance.calmMusicSource.enabled = false;
	}

	static public void PlayBattleMusic()
	{
		if (currentSong != Music.Battle && instance)
		{
			instance.StartCoroutine(instance.CrossFade(instance.calmMusicSource, instance.battleMusicSource, fadeDurationToBattle, instance.battleVolume, true));
			currentSong = Music.Battle; 
		}
	}

	static public void PlayCalmMusic()
	{
		if (currentSong != Music.Calm && instance)
		{
			instance.StartCoroutine(instance.CrossFade(instance.battleMusicSource, instance.calmMusicSource, fadeDurationToCalm, instance.calmVolume, false));
			currentSong = Music.Calm; 
		}
	}

	IEnumerator CrossFade(AudioSource from, AudioSource to, float duration, float targetVolume, bool restartSong)
	{
		if (!to.isPlaying || (to.volume == 0 && restartSong))
		{
			to.Play();
			to.volume = 0;
		}
		else
		{
			to.UnPause();
		}

		float fromStartVolume = from.volume;
		while (to.volume < targetVolume || from.volume > 0)
		{
			yield return null; // Wait for next frame
			from.volume -= (Time.deltaTime * fromStartVolume) / duration;
			to.volume += (Time.deltaTime * targetVolume) / duration;
		}

		from.Pause();
		to.volume = targetVolume; // Just to be sure
	}
}
