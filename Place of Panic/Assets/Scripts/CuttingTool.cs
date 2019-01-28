using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CuttingTool : MonoBehaviour
{


    bool atStartPos = false;
    bool started = false;
    List<Collider2D> col2D = new List<Collider2D>();
    public GameObject line;


    public GameObject[] shitToRemove;

    void Start()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }

    void Update()
    {

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        //Instantiate(line, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);

        if (!started)
        {
            if (atStartPos && Input.GetButton("Fire1"))
            {
                started = true;
                GetComponent<TrailRenderer>().enabled = true;
                Destroy(GameObject.FindGameObjectWithTag("StartPos"));
            }

            return;
        }
        if (started && col2D.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StartPos")
        {
            atStartPos = true;
        }
        if (collision.tag == "Bounds")
            col2D.Add(collision);
        if (started && collision.tag == "EndPos")
        {
            foreach (var item in shitToRemove)
            {
                Destroy(item);
            }
            Destroy(gameObject);
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!started && collision.tag == "StartPos")
        {

            atStartPos = false;
        }
        if (collision.tag == "Bounds")
        {
            col2D.Remove(collision);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
