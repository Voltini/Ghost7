using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [HideInInspector] public bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public SoundManager soundManager;
    float prevTimeScale;
    public PlayerControl player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    void Pause()
    {
        //soundManager.StopTime();
        pauseMenuUI.SetActive(true);
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isGamePaused = true;
        player.enabled = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = prevTimeScale;
        isGamePaused = false;
        //soundManager.ResumeTime();
        player.enabled = true;
    }

    public void LoadMenu()
    {
        Resume();
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
        if ((PlayerPrefs.GetInt("levelReached") == currentLevelNumber) && Application.CanStreamedLevelBeLoaded("Cutscene " + currentLevelNumber)) {
            SceneManager.LoadScene("Cutscene " + currentLevelNumber);
        }
        else {
            SceneManager.LoadScene("Level " + nextLevelNumber);
        }
    }
}
