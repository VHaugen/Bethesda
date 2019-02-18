using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour {

    public static int PlayerNum;
	
	// Update is called once per frame
	public void CharacterSelectFuntion (int sellectedNum) {
        PlayerNum = sellectedNum;
        Application.LoadLevel("MainScene");
	}
}
