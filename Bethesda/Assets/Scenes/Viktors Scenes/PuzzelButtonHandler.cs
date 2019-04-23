using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzelButtonHandler : MonoBehaviour
{

    private PuzzleHandler PuzzelHandler;
    public int triggers = 3;
    public GameObject[] powerUpp;
    // Use this for initialization
    void Start()
    {

        PuzzelHandler = GameObject.Find("World").GetComponent<PuzzleHandler>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TriggerWasHit()
    {
        triggers--;
        if (triggers == 0)
        {
            Instantiate(RandPoweupp(), transform);
            PuzzelHandler.PuzzelIsCleared();
            
        }

    }
    public GameObject RandPoweupp()
    {

        int k = Random.Range(0, powerUpp.Length - 1);

        return powerUpp[k];
    }
}
