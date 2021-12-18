using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirm : MonoBehaviour
{

    public LevelSelector levelSelector;

    public void Yes()
    {
        PlayerPrefs.SetInt("levelReached", 0);
        levelSelector.UpdateData();
        gameObject.SetActive(false);
    }

    public void No()
    {
        gameObject.SetActive(false);
    }

}
