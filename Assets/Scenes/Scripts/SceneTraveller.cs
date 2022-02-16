using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTraveller : MonoBehaviour { 
    public static SceneTraveller instance;

    public static int currentScene;
    void Awake()
    {
        if (instance == null)
            instance = this;


        //Set screen size for Standalone
//#if UNITY_STANDALONE
//         Screen.SetResolution(564, 960, false);
//         Screen.fullScreen = false;
//#endif
    }

    void Start()
    {
        //Screen.SetResolution(375, 667, false);
    }
    public void A_ExitButton() {
        Application.Quit();
    }

    public void A_LoadScene(int i) {
        SceneManager.LoadScene(i);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            A_ExitButton();
    }
}
