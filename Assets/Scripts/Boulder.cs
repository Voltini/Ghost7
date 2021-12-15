using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Boulder : MonoBehaviour
{
    Collider boulderCollider;
    bool isHaunted;
    float movementx;
    float movementy;
    Rigidbody2D objectId;
    float speed = 7;
    public GameObject rewindPlayer;
    public RigidbodyConstraints2D constraints;
    public RigidbodyConstraints2D freezeConstraints;
    public CameraControl cam;
    float velocity;
    Vector2 initPos;
    public PhantomPlayer phantom;
    bool wasHaunted = false;


    void Start()
    {
        objectId = GetComponent<Rigidbody2D>();
        initPos = transform.position;
    }

    void Update()
    {
        if (isHaunted) {
            Move();
            if (Input.GetKeyDown(KeyCode.E)) {
                StopHauting();
            }
        }
        else {
            velocity = objectId.velocity.sqrMagnitude;
        }
    }

    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(typeof(PlayerControl), out Component player))
        {   
            if (velocity > 10){
                other.gameObject.GetComponent<PlayerControl>().Death();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (isHaunted) {
            if (other.CompareTag("HellGate")) {
                phantom.GetComponent<Rigidbody2D>().position = (Vector2)transform.position;
                phantom.gameObject.SetActive(true);
                cam.SwitchTarget(phantom.gameObject);
                transform.position = initPos;
                isHaunted = false;
                objectId.constraints = freezeConstraints;
            }
            else if (other.CompareTag("Demon")) {
                phantom.GetComponent<Rigidbody2D>().position = (Vector2)transform.position;
                phantom.gameObject.SetActive(true);
                cam.SwitchTarget(phantom.gameObject);
                transform.position = initPos;
                isHaunted = false;
                objectId.constraints = freezeConstraints;
            }
        }
        else if (!wasHaunted) {
            if (other.CompareTag("Rewind")) {
                other.GetComponent<Rewind>().ResetRewind();
                transform.position = initPos;
            }
        }
        
    }

    public void Haunt()
    {
        wasHaunted = true;
        isHaunted = true;
        objectId.gravityScale = 0f;
        objectId.constraints = constraints;
        cam.SwitchTarget(this.gameObject);
        gameObject.layer = LayerMask.NameToLayer("Hauntable");
    }

    void Move()
    {
        movementx = Input.GetAxis("Horizontal");
        movementy = Input.GetAxis("Vertical");
        objectId.velocity = new Vector2(speed * movementx, speed * movementy);
    }

    public void StopHauting()
    {
        isHaunted = false;
        objectId.constraints = freezeConstraints;
        cam.SwitchTarget(rewindPlayer);
        initPos = transform.position;
        phantom.GetComponent<PhantomPlayer>().HideAll();
    }

    public void OnPlayerDeath()
    {
        if (!wasHaunted) {
            transform.position = initPos;
        }
    }
    
}