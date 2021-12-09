using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    Collider dispenserCollider;
    public float periode ;
    //0 : gauche
    //1 : haut
    //2 : droite
    //3 : bas
    public int orientation;
    public Transform firepoint;
    public GameObject inventory;
    // Start is called before the first frame update


    void Start()
    {
        dispenserCollider = GetComponent<Collider>();
        StartCoroutine("Timer");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector2 getArrowForce(){
        switch(orientation){
            case 0 : return new Vector2(-800f, 0f);//Gauche
            case 1 : return new Vector2(0f, 800f);//Haut
            case 2 : return new Vector2(800f, 0f);//Droite
            case 3 : return new Vector2(0f, -800f);//Bas
            default : return new Vector2(0f, 0f);
        }
    }

    public void Shoot(){
        GameObject newArrow = Instantiate(inventory, firepoint) as GameObject;
        Rigidbody2D newArrowId = newArrow.GetComponent<Rigidbody2D>();
        newArrowId.position = firepoint.position;
        newArrowId.AddForce(getArrowForce());
        
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(periode);
        Shoot();
        StartCoroutine("Timer");
    }

}
