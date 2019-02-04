using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshmallowNinjaHitboxManager : MonoBehaviour {

    public PolygonCollider2D frame2;

    public PolygonCollider2D frame3;

    public PolygonCollider2D frame4;

    public PolygonCollider2D sideTilt;

    public PolygonCollider2D downTilt;

    public PolygonCollider2D upTilt;

    public PolygonCollider2D forwardAir;

    public PolygonCollider2D neutralAir;

    public PolygonCollider2D downAir1;

    public PolygonCollider2D downAir2;

    public PolygonCollider2D downAir3;

    public PolygonCollider2D downAirFinal;

    public PolygonCollider2D upAir;

    public PolygonCollider2D upSpecial;

    public PolygonCollider2D ariAri1;

    public PolygonCollider2D ariAri2;

    public PolygonCollider2D ariAri3;

    public PolygonCollider2D ariAri4;

    public PolygonCollider2D ariAri5;

    public PolygonCollider2D ariAri6;

    public PolygonCollider2D ariAri7;

    public PolygonCollider2D ariAri8;

    public PolygonCollider2D ariAri9;

    public PolygonCollider2D ariAriEnd1;

    public PolygonCollider2D ariAriEnd2;

    public PolygonCollider2D ariAriEnd3;

    private PolygonCollider2D[] colliders;

    private PolygonCollider2D localCollider;
	// Use this for initialization
	public enum hitBoxes
    {
        frame2Box,
        frame3Box,
        frame4Box,
        sideTiltBox,
        downTiltBox,
        upTiltBox,
        forwardAirBox,
        neutralAirBox,
        downAir1Box,
        downAir2Box,
        downAir3Box,
        downAirFinalBox,
        upAirBox,
        upSpecialBox,
        ariAri1Box,
        ariAri2Box,
        ariAri3Box,
        ariAri4Box,
        ariAri5Box,
        ariAri6Box,
        ariAri7Box,
        ariAri8Box,
        ariAri9Box,
        ariAriEnd1Box,
        ariAriEnd2Box,
        ariAriEnd3Box,
        clear
    }

    private void Start()
    {
        colliders = new PolygonCollider2D[] { frame2, frame3, frame4, sideTilt, downTilt, upTilt, forwardAir, neutralAir,
            downAir1, downAir2, downAir3, downAirFinal, upAir, upSpecial,
        ariAri1, ariAri2, ariAri3, ariAri4, ariAri5, ariAri6, ariAri7, ariAri8, ariAri9,
         ariAriEnd1, ariAriEnd2, ariAriEnd3};

        localCollider = gameObject.AddComponent<PolygonCollider2D>();
        localCollider.isTrigger = true;
        localCollider.pathCount = 0;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("it works");
    }

    public void setHitBox(hitBoxes val)
    {
        if (val != hitBoxes.clear)
        {
            localCollider.SetPath(0, colliders[(int)val].GetPath(0));
            return;
        }
        localCollider.pathCount = 0;
    }
}
