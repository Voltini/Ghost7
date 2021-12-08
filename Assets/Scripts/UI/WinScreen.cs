using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [HideInInspector] public bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public SoundManager soundManager;
    float prevTimeScale;
    int nbLevels = 5;

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit !");
    }

    public void NextLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        string[] strScene = scene.name.Split(' ');
        int currentLevelNumber = int.Parse(strScene[1]);
        int nextLevelNumber = currentLevelNumber + 1;
        if (nextLevelNumber <= nbLevels) {
            SceneManager.LoadScene("Level " + nextLevelNumber);
        }
    }
}
