using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
    //public SceneFader fader;
    public Button[] levelButtons;
    int LevelReached;
    public Color lockedColor;
    public Image blackscreen;
    
    void Start() {
        if (!PlayerPrefs.HasKey("levelReached")) {
            PlayerPrefs.SetInt("levelReached", 0);
        }
        UpdateData();
    }

    public void Select (string levelName)
    {
        StartCoroutine("SelectC", levelName);
    }
    public void UpdateData()
    {
        LevelReached = PlayerPrefs.GetInt("levelReached", 1);
        for (int i = LevelReached; i < levelButtons.Length; i++) {
            levelButtons[i].interactable = false;
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = lockedColor;
        }
    }

    IEnumerator SelectC(string levelName) {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++) {
            image.color = Color.Lerp(new Color(0,0,0, 0), Color.black, 0.01f*i);
            yield return new WaitForSeconds(0.001f);
        }
        SceneManager.LoadScene(levelName);
    }
}

