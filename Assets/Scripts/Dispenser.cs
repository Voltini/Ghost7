using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public float periode ;
    public Transform firepoint;
    public GameObject inventory;
    public bool isHaunted = false;
    public CameraControl cam;
    public PhantomPlayer phantom;
    public PlayerControl player;
    [HideInInspector] public bool wasHaunted = false;
    float lastFiredTime;
    float timeElapsed;  //time to wait before shooting
    bool hasShot;
    float time => Time.timeSinceLevelLoad - player.rewindPlayer.deathTime;
    


    void Start()
    {
        lastFiredTime = time;
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
        Debug.Log(time);
        GameObject newArrow = Instantiate(inventory, firepoint.position, transform.rotation) as GameObject;
        Rigidbody2D newArrowId = newArrow.GetComponent<Rigidbody2D>();
        newArrowId.velocity = 25f * (firepoint.position - transform.position).normalized;
        newArrow.GetComponent<Arrow>().dispenser = this;
        lastFiredTime = time;
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
        cam.SwitchTarget(phantom.gameObject);
        phantom.GetComponent<Rigidbody2D>().position = transform.position;
        phantom.gameObject.SetActive(true);
    }

    public void SaveState()
    {
        timeElapsed = time - lastFiredTime; 
        hasShot = Time.timeSinceLevelLoad > periode;
        Debug.Log("time elapsed : " + timeElapsed);
    }

    public void RestoreState()
    {
        if (!isHaunted) {
            StopAllCoroutines();
            StartCoroutine("RestartShooting",timeElapsed);
        }
    }

    IEnumerator RestartShooting(float time)
    {
        yield return new WaitForSeconds(time);
        if (hasShot) {
            Shoot();
        }
        StartCoroutine("Timer");
    }

}
