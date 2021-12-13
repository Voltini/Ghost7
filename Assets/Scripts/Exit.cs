using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject pauseMenu;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player") {
            Debug.Log("win");
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
        yield return null;//new WaitForSeconds(0.5f);
        winScreen.SetActive(true);
    }
    
}
