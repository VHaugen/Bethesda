using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour {

    public int organsCollected = 0;
    public string scene;
    public List<GameObject> organs;
    AudioSource audio;
    
	// Use this for initialization
	void Start ()
    {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (GameObject.FindGameObjectsWithTag("organ").Length == organs.Count)
        {
            
            SceneManager.LoadScene(scene);
        }
		
	}
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "organ")
        {
            audio.Play();
            if (!Input.GetButton("Fire1"))
            {
                foreach(var organ in organs)
                {

                    if (organ == col.gameObject)
                        return;
                }
                organs.Add(col.gameObject);
            }
            
        }
    }
}
