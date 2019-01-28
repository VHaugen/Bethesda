using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    public bool selected;
    SpriteRenderer colour;
    private Color startColor;
    public bool allowPickUp;

    // Use this for initialization
    void Start()
    {
        colour = GetComponent<SpriteRenderer>();
        startColor = colour.material.color;
        allowPickUp = true;
    }
    void OnMouseOver()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            selected = true;
        }

        colour.material.color = Color.blue;
    }
    void OnMouseExit()
    {
        Debug.Log("mouse has left");
        colour.material.color = startColor;
        Debug.Log("color is normal once more");
        
    }
    void Update()
    {
        if (selected)
        {
            Vector2 cursurPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursurPos.x, cursurPos.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            selected = false;
        }
        if (allowPickUp == false)
        {
            selected = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (col.gameObject.tag == "CollectionPlate")
            {
                selected = false;
                allowPickUp = false;
            }

        }
    }
}
