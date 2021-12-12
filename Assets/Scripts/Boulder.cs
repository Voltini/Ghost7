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


    void Start()
    {
        objectId = GetComponent<Rigidbody2D>();
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

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(typeof(PlayerControl), out Component player))
        {   
            if (velocity > 10){
                other.gameObject.GetComponent<PlayerControl>().Death();
            }
        }
    }

    public void Haunt()
    {
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
    }
}