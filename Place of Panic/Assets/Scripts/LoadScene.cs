using UnityEngine;
using UnityEngine.SceneManagement;
//Sebastian Olsson SU16b
public class LoadScene : MonoBehaviour {
    public string scene;
    void Start()
    {


    }
    public void LoadOnClick()
    {
        SceneManager.LoadScene(scene);
    }
}