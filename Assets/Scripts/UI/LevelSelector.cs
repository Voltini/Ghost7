using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    //public SceneFader fader;
    public Button[] levelButtons;
    int LevelReached;
    public Color lockedColor;
    
    void Start() {
        if (!PlayerPrefs.HasKey("levelReached")) {
            PlayerPrefs.SetInt("levelReached", 1);
        }
        UpdateData();
    }

    public void Select (string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void UpdateData()
    {
        Debug.Log(PlayerPrefs.GetInt("levelReached"));
        LevelReached = PlayerPrefs.GetInt("levelReached", 1);
        for (int i = LevelReached; i < levelButtons.Length; i++) {
            levelButtons[i].interactable = false;
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = lockedColor;
        }
    }
}

