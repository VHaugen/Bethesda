using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSee : MonoBehaviour
{
    public Transform playerTrans;
    public Color tempColorReturn;
    private Color tempColor;
    public List<GameObject> hitObjects; // public for debug


    private void Start()
    {
        hitObjects = new List<GameObject>();
        tempColor.a = 0.2f;
        StartCoroutine(DetPlayerObs());
    }

    IEnumerator DetPlayerObs()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            Vector3 dir = (playerTrans.position - Camera.main.transform.position).normalized;
            RaycastHit raycCastHit;

            if (Physics.Raycast(Camera.main.transform.position, dir, out raycCastHit, Mathf.Infinity))
            {
                if (raycCastHit.collider.gameObject.tag == "Wall")
                {

                    raycCastHit.collider.gameObject.tag = "Hit";
                    hitObjects.Add(raycCastHit.collider.gameObject);
                    tempColorReturn = raycCastHit.collider.gameObject.GetComponent<Renderer>().material.color;
                    tempColor = raycCastHit.collider.gameObject.GetComponent<Renderer>().material.color;
                    tempColor.a = 0.35f;
                    raycCastHit.collider.gameObject.GetComponent<Renderer>().material.color = tempColor;


                }
                if (raycCastHit.collider.gameObject.tag == "Hit")
                {
                    //break;
                }
                if (raycCastHit.collider.gameObject.tag != "Wall" && raycCastHit.collider.gameObject.tag != "Hit" && hitObjects.Count > 0)
                {
                    for (int i = 0; i < hitObjects.Count; i++)
                    {
                        hitObjects[i].GetComponent<Renderer>().material.color = tempColorReturn;
                        hitObjects[i].gameObject.tag = "Wall";
                    }
                    print(tempColorReturn);
                    hitObjects.Clear();
                    print("HELOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");

                }
                else
                {
                    print("not");
                }
            }

        }
    }
}