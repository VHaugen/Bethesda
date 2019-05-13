using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	bool inProcessOfDying = false;

	float timer = 0;

	public void Die()
	{
		inProcessOfDying = true;
		timer = 3.0f;
		MusicController.DisableMusic();
	}

	void Update()
	{
		if (inProcessOfDying)
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				FindObjectOfType<MenuController>().LoadScene(2);
			}
		}
	}
}
