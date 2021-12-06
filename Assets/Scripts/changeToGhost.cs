using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeToGhost : MonoBehaviour
{
    public Transform character;
    public List<Transform> possibleCharacters;
    public bool phantomPlaying;
    bool playerDeath; 
    // Start is called before the first frame update
    void Start()
    {
        playerDeath = GameObject.Find("player").GetComponent<PlayerControl>().playerDeath; 
        if (character == null && possibleCharacters.Count >= 1)
        {
            character = possibleCharacters[0];
        }
        change();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDeath){
            if (!phantomPlaying)
            {
                phantomPlaying = true;
            }
            change();
        }
    }
    public void change(){
        character = possibleCharacters[1];
        character.GetComponent<PlayerControl>().enabled = true;
        if (phantomPlaying)
        {
            possibleCharacters[0].GetComponent<PlayerControl>().enabled = false;
        }
    }
}
