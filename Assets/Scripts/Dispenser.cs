using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    Collider dispenserCollider;
    public float periode ;
    public Transform firepoint;
    public GameObject inventory;
    bool isHaunted = false;
    public CameraControl cam;
    public GameObject player;


    void Start()
    {
        dispenserCollider = GetComponent<Collider>();
        StartCoroutine("Timer");

    }

    void Update() 
    {
        if (isHaunted) {
            if (Input.GetKeyDown(KeyCode.E)) {
                isHaunted = false;
                cam.SwitchTarget(player);
                player.SetActive(true);
            }
            else {
                transform.rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z - 0.5f * Input.GetAxis("Horizontal"));
            }
        }
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

    public void Haunt()
    {
        isHaunted = true;
    }

}
