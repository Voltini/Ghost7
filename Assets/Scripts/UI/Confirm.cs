using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirm : MonoBehaviour
{

    public void Yes()
    {
        PlayerPrefs.SetInt("levelReached", 0);
        gameObject.SetActive(false);
    }

    public void No()
    {
        gameObject.SetActive(false);
    }

}
