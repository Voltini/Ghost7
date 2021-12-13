using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class endManager : MonoBehaviour
{

    public Image blackscreen;
    public GameObject thx;
    public GameObject txt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 200; i++) {
            image.color = Color.Lerp(Color.black, new Color(0,0,0, 0), 0.005f*i);
            yield return new WaitForSeconds(0.003f);
        }
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        thx.transform.DOMoveY(10, 20f);
        yield return new WaitForSeconds (5f);
        txt.transform.DOMoveY(10,30f);
        yield return new WaitForSeconds(20f);
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 200; i++) {
            image.color = Color.Lerp(new Color(0,0,0, 0), Color.black, 0.005f*i);
            yield return new WaitForSeconds(0.003f);
        }
        SceneManager.LoadScene("Main Menu");
    }
}
