using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GhostControl : MonoBehaviour
{

    PlayerControl player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if((!gameObject.activeSelf) && (!player.gameObject.activeSelf)){
            gameObject.SetActive(true);
        }
    }

}
