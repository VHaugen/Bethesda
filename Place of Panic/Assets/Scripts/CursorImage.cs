using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorImage : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D defautTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    // Use this for initialization
    void Start()
    {
        Cursor.SetCursor(defautTexture, hotSpot, cursorMode);
    }
    void OnMouseDown()
    {
        if (gameObject.tag == "StartPos" || gameObject.tag == "Bounds" || gameObject.tag == "EndPos")
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }
    void OnMouseExit()
    {
        //Cursor.SetCursor(defautTexture, hotSpot, cursorMode);
    }
}
