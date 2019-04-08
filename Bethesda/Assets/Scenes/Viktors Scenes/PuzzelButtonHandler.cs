using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzelButtonHandler : MonoBehaviour
{

    private PuzzleHandler PuzzelHandler;
    public int triggers = 3;
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
            PuzzelHandler.PuzzelIsCleared();

        }

    }
}
