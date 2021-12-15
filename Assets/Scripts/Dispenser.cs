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
    public PhantomPlayer phantom;
    [HideInInspector] public bool wasHaunted = false;
    float lastFiredTime;
    float timeElapsed;  //time to wait before shooting
    bool hasShot;
    


    void Start()
    {
        dispenserCollider = GetComponent<Collider>();
        StartCoroutine("Timer");

    }

    void Update() 
    {
        if (isHaunted) {
            if (Input.GetKeyDown(KeyCode.E)) {
                StopHaunting();
            }
            else {
                transform.rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z - 0.5f * Input.GetAxis("Horizontal"));
            }
        }
    }

    public void Shoot(){
        GameObject newArrow = Instantiate(inventory, firepoint.position, transform.rotation) as GameObject;
        Rigidbody2D newArrowId = newArrow.GetComponent<Rigidbody2D>();
        newArrowId.velocity = 25f * (firepoint.position - transform.position).normalized;
        newArrow.GetComponent<Arrow>().dispenser = this;
        lastFiredTime = Time.timeSinceLevelLoad;
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
        wasHaunted = true;
        cam.SwitchTarget(this.gameObject);
    }

    public void StopHaunting()
    {
        isHaunted = false;
        cam.SwitchTarget(player);
        player.SetActive(true);
        phantom.HideAll();
    }

    public void SaveState()
    {
        timeElapsed = Time.timeSinceLevelLoad - lastFiredTime; 
        hasShot = Time.timeSinceLevelLoad > 3f;
    }

    public void RestoreState()
    {
        StopAllCoroutines();
        StartCoroutine("RestartShooting",timeElapsed);
    }

    IEnumerator RestartShooting(float time)
    {
        Debug.Log(time);
        yield return new WaitForSeconds(time);
        if (hasShot) {
        Shoot();
        }
        StartCoroutine("Timer");
    }

}
