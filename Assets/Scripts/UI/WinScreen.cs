using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [HideInInspector] public bool isGamePaused = false;
    public GameObject pauseMenuUI;
    public SoundManager soundManager;
    float prevTimeScale;
    int nbLevels = 5;
    public Image blackscreen;

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
        if (nextLevelNumber <= nbLevels) {
            SceneManager.LoadScene("Level " + nextLevelNumber);
        }
    }

    IEnumerator LoadMenuC()
    {
    var image = blackscreen.GetComponent<Image>();
    for (int i = 0; i <= 100; i++) {
        image.color = Color.Lerp(new Color(0,0,0, 0), Color.black, 0.01f*i);
        yield return new WaitForSeconds(0.001f);
    }
    SceneManager.LoadScene("Main Menu");
    }

    IEnumerator QuitC()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++) {
            image.color = Color.Lerp(new Color(0,0,0, 0), Color.black, 0.01f*i);
            yield return new WaitForSeconds(0.001f);
        }
        Application.Quit();
        Debug.Log("Quit !");
    }
}
