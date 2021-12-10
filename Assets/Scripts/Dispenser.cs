using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    Collider dispenserCollider;
    public float periode ;
    public Transform firepoint;
    public GameObject inventory;
    // Start is called before the first frame update


    void Start()
    {
        dispenserCollider = GetComponent<Collider>();
        StartCoroutine("Timer");

    }

    public void Shoot(){
        GameObject newArrow = Instantiate(inventory, firepoint.position, transform.rotation) as GameObject;
        Rigidbody2D newArrowId = newArrow.GetComponent<Rigidbody2D>();
        newArrowId.position = firepoint.position;
        newArrowId.AddForce(800f * (firepoint.position - transform.position).normalized);
        
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(periode);
        Shoot();
        StartCoroutine("Timer");
    }

}
