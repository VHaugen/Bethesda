using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSee : MonoBehaviour
{

    public Color transColor;
    private Color initColor;

    void Start()
    {   
        initColor = GetComponent<Renderer>().material.color;
    }
    public void SetTransparent()
    {

    }
public void SetNormal()
    {
        
    }


}
