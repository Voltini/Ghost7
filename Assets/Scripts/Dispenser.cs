using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{

    Rigidbody2D dispenserId;
    Collider dispenserCollider;
    int timer;
    public int periode ;
    //0 : gauche
    //1 : haut
    //2 : droite
    //3 : bas
    public int orientation;
    public Arrow inventory;
    // Start is called before the first frame update


    void Start()
    {
        dispenserId = GetComponent<Rigidbody2D>();
        dispenserCollider = GetComponent<Collider>();
        dispenserId.gravityScale = 0;
        timer = periode;

    }

    // Update is called once per frame
    void Update()
    {
        timer -=1;
        if(timer == 0){

            GameObject newArrow = new GameObject("arrow", typeof(Rigidbody2D), typeof(BoxCollider));
            newArrow.AddComponent<SpriteRenderer>();
            Rigidbody2D arrowId = newArrow.GetComponent<Rigidbody2D>();
            arrowId.position = dispenserId.position;
            arrowId.rotation = dispenserId.rotation;

            newArrow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dispenserDebug");
            arrowId.AddForce(getArrowForce());

            timer = periode;
        }
    }

    private Vector2 getArrowForce(){
        switch(orientation){
            case 0 : return new Vector2(-200f, 0f);//Gauche
            case 1 : return new Vector2(0f, 200f);//Haut
            case 2 : return new Vector2(200f, 0f);//Droite
            case 3 : return new Vector2(0f, -200f);//Bas
            default : return new Vector2(0f, 0f);
        }
    }

}
