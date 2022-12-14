using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    
    [HideInInspector] public bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public SoundManager soundManager;
    float prevTimeScale;
    public GameObject player;
    public Image blackscreen;
    public mode gameMode;
    public enum mode {Normal, Tutorial, Ending};

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
        soundManager.StopTime();
        pauseMenuUI.SetActive(true);
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isGamePaused = true;
        if (gameMode is mode.Normal) {player.GetComponent<PlayerControl>().enabled = false;}
        else if (gameMode is mode.Tutorial) {player.GetComponent<startPlayer>().enabled = false;}
        else {player.GetComponent<endPlayer>().enabled = false;}
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = prevTimeScale;
        isGamePaused = false;
        soundManager.ResumeTime();
        if (gameMode is mode.Normal) {player.GetComponent<PlayerControl>().enabled = true;}
        else if (gameMode is mode.Tutorial) {player.GetComponent<startPlayer>().enabled = true;}
        else {player.GetComponent<endPlayer>().enabled = true;}
    }

    public void LoadMenu()
    {
        StartCoroutine("LoadMenuC");
    }

    public void Quit()
    {
        StartCoroutine("QuitC");
    }

    public void NextLevel()
    {
        StartCoroutine("NextLevelC");        
    }

    IEnumerator NextLevelC()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++) {
            image.color = Color.Lerp(new Color(0,0,0, 0), Color.black, 0.01f*i);
            yield return new WaitForSeconds(0.001f);
        }
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

    IEnumerator LoadMenuC()
    {
        Resume();
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++) {
            image.color = Color.Lerp(new Color(0,0,0, 0), Color.black, 0.01f*i);
            yield return new WaitForSeconds(0.001f);
        }
        SceneManager.LoadScene("Main Menu");
    }

    IEnumerator QuitC()
    {
        Resume();
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++) {
            image.color = Color.Lerp(new Color(0,0,0, 0), Color.black, 0.01f*i);
            yield return new WaitForSeconds(0.001f);
        }
        Application.Quit();
        Debug.Log("Quit !");
    }
    
}
