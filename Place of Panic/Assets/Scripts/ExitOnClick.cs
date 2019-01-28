using UnityEngine;
using UnityEngine.SceneManagement;
//Sebastian Olsson SU16b
public class ExitOnClick : MonoBehaviour {
    void Start()
    {


    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
        //Just to make sure its working
    }
}