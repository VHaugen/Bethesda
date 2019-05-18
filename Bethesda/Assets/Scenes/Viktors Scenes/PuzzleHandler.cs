using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
	public List<GameObject> Doors;
	public AudioClip openSound;

	// Use this for initialization
	void Start()
	{
		Doors = new List<GameObject>();
	}

	public void PuzzelIsCleared()
	{

		StartCoroutine(WaitIguess2());


	}
	IEnumerator WaitIguess()
	{

		yield return new WaitForSeconds(3.2f);

		for (int i = 0; i < Doors.Count; i++)
		{
			Destroy(Doors[i]);
		}

	}
	IEnumerator WaitIguess2()
	{
		yield return new WaitForSeconds(0.7f);

		Sound.Play(openSound);

		for (int i = 0; i < Doors.Count; i++)
		{
			if (Doors[i] != null)
				Destroy(Doors[i].GetComponentInChildren<BoxCollider>());
		}

		float distanceTravelled = 0;
		while (distanceTravelled < 20)
		{
			for (int i = 0; i < Doors.Count; i++)
			{
				if (Doors[i] != null)
				{
					float deltaMove = Time.deltaTime * 10;
					Doors[i].transform.Translate(Vector3.up * deltaMove);
					distanceTravelled += deltaMove;
				}
			}
			yield return null;
		}

		for (int i = 0; i < Doors.Count; i++)
		{
			Destroy(Doors[i]);
		}

	}
}
