using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{

    public GameObject noMousePanel;
    public Volume volume;

    void Start() {
        if (!Input.mousePresent) {
            noMousePanel.SetActive(true);
        }
        UnityEngine.Rendering.VolumeProfile volumeProfile = volume.GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        UnityEngine.Rendering.Universal.LensDistortion lensDistortion;
        if(!volumeProfile.TryGet(out lensDistortion)) throw new System.NullReferenceException(nameof(lensDistortion));
        lensDistortion.scale.Override(1f);
    }

    public void Play()
    {
        int nextLevel = PlayerPrefs.GetInt("levelReached") + 1;
        UnityEngine.Rendering.VolumeProfile volumeProfile = volume.GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        UnityEngine.Rendering.Universal.LensDistortion lensDistortion;
        if(!volumeProfile.TryGet(out lensDistortion)) throw new System.NullReferenceException(nameof(lensDistortion));
        lensDistortion.scale.Override(1.05f);
        if (nextLevel != 0) { //1
            SceneManager.LoadScene("Level " + 4/*nextLevel*/);
        }
        else {
            SceneManager.LoadScene("Intro Cutscene");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
