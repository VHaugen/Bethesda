﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterChange : MonoBehaviour
{

    var currentPlayer : int = 1;


     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentPlayer != 1)
        {
            print("1 pressed");
            currentPlayer = 1;
        }


        if (Input.GetKeyDown(KeyCode.Alpha2) && currentPlayer != 2 ){
            print("2 pressed");
            currentPlayer = 2;
        }

    }
}
