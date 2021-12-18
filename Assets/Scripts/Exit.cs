using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject pauseMenu;
    public Image blackscreen;
    int nbLevels = 7;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player") {
            other.GetComponent<PlayerControl>().levelWon = true;
            pauseMenu.gameObject.SetActive(false);
            Win();
            other.gameObject.SetActive(false);
        }
    }

    private void Win()
    {
        int LevelReached = PlayerPrefs.GetInt("levelReached", 1);
        Scene scene = SceneManager.GetActiveScene();
        string[] strScene = scene.name.Split(' ');
        int levelNumber = int.Parse(strScene[1]);
        if (levelNumber == LevelReached) {
            PlayerPrefs.SetInt("levelReached", levelNumber + 1);
        }
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter(){
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
    
}
