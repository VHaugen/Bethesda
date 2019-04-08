using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public Graphic blackness;
	float fadeoutDuration = 0.5f;
	float fadeinDuration = 0.5f;
	int sceneToLoadIndex = -1;

	static bool fadeIn;

	private void Start()
	{
		if (blackness)
		{
			Color col = blackness.color;
			col.a = fadeIn ? 1 : 0;
			blackness.color = col;
		}
	}

	public void LoadScene(string name)
	{
		if (sceneToLoadIndex == -1)
		{
			print("Let's load scene: " + name);
			sceneToLoadIndex = SceneManager.GetSceneByName(name).buildIndex;
		}
	}

	public void LoadScene(int index)
	{
		if (sceneToLoadIndex == -1)
		{
			print("Let's load scene by index: " + SceneManager.GetSceneByBuildIndex(index).path);
			sceneToLoadIndex = index; 
		}
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	void Update()
	{
		if (sceneToLoadIndex != -1)
		{
			Color col = blackness.color;
			col.a += Time.deltaTime / fadeoutDuration;
			blackness.color = col;

			if (col.a >= 1)
			{
				SceneManager.LoadScene(sceneToLoadIndex);
				fadeIn = true;
			}
		}
		else if (fadeIn)
		{
			Color col = blackness.color;
			col.a -= Time.deltaTime / fadeinDuration;
			blackness.color = col;

			if (col.a <= 0)
			{
				fadeIn = false;
			}
		}
	}
}
