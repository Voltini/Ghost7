using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{

    public Image blackscreen;

    void Start()
    {
        StartCoroutine("FadeIn");
    }

    public void Play()
    {
        StartCoroutine("PlayC");
    }

    public void Quit()
    {
        StartCoroutine("QuitC");
    }

    IEnumerator FadeIn()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 200; i++)
        {
            image.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), 0.005f * i);
            yield return new WaitForSeconds(0.003f);
        }
    }

    IEnumerator PlayC()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++)
        {
            image.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, 0.01f * i);
            yield return new WaitForSeconds(0.001f);
        }
        int currentLevel = PlayerPrefs.GetInt("levelReached");
        if (currentLevel != 0)
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
        else
        {
            SceneManager.LoadScene("Intro Cutscene");
        }
        SceneManager.LoadScene("Level 1");
    }

    IEnumerator QuitC()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++)
        {
            image.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, 0.01f * i);
            yield return new WaitForSeconds(0.001f);
        }
        Application.Quit();
        Debug.Log("Quit !");
    }

    public void NextCutscene()
    {
        StartCoroutine("NextCutsceneC");
    }

    IEnumerator NextCutsceneC()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++)
        {
            image.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, 0.01f * i);
            yield return new WaitForSeconds(0.001f);
        }
        SceneManager.LoadScene("IntroPt2");
    }

    public void Ending()
    {
        StartCoroutine("EndingC");
    }

    IEnumerator EndingC()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++)
        {
            image.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, 0.01f * i);
            yield return new WaitForSeconds(0.001f);
        }
        SceneManager.LoadScene("Ending 2");
    }

}
