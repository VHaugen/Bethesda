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
        clear
    }

    private void Start()
    {
        colliders = new PolygonCollider2D[] { frame2, frame3, frame4, sideTilt, downTilt, upTilt, forwardAir, neutralAir, downAir1, downAir2, downAir3, downAirFinal, upAir };

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
