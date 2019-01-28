using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeLeft = 15f;
    public Text time;
    public GameObject timeCanvas, restartCanvas;

    void Start()
    {
        StartCoroutine("LoseTime");
        restartCanvas.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        time.text = "Time left: " + timeLeft;

        if(timeLeft == 10)
        {
            StartCoroutine("Flash");
        }

        if (timeLeft <= 0)
        {
            StopCoroutine("LoseTime");
            StopCoroutine("Flash");
            //timeCanvas.SetActive(false);
            restartCanvas.GetComponent<Canvas>().enabled = true;
        }
    }

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    IEnumerator Flash()
    {
        time.color = Color.red;
        time.fontSize = 64;
        yield return new WaitForSeconds(0.5f);
        time.color = Color.white;
        time.fontSize = 58;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Flash");
    }
}

