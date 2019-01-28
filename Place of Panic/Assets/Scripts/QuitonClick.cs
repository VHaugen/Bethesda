using System.Collections;
using UnityEngine;

public class QuitonClick : MonoBehaviour {

    public void OnApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }
}
