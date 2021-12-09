using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        /*int nextLevel = PlayerPrefs.GetInt("levelReached") + 1;
        if (nextLevel != 0) { //1
            SceneManager.LoadScene("Level " + nextLevel);
        }
        else {
            SceneManager.LoadScene("Intro Cutscene");
        }*/
        SceneManager.LoadScene("Level 1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
