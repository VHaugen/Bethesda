using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	static public int numEnemies { get; private set; }

	bool created = false;
	bool destroyed = false;

	// Use this for initialization
	void Awake()
	{
		AddMe();
	}

	void OnDestroy()
	{
		RemoveMe();
	}

	public void AddMe()
	{
		if (!created)
		{
			numEnemies++;
			created = true;
		}
	}

	public void RemoveMe()
	{
		if (!destroyed)
		{
			numEnemies--;
			destroyed = true;
		}
	}
}
