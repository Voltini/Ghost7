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
    public Rewind rewindPlayer;
    public RigidbodyConstraints2D constraints;
    public RigidbodyConstraints2D freezeConstraints;
    public CameraControl cam;
    float velocity;
    Vector2 initPos;
    public PhantomPlayer phantom;
    [HideInInspector] public bool wasHaunted = false;
    float radiusOpposite;
    bool rolling;
    Vector2 checkpointVelocity;


    void Start()
    {
        objectId = GetComponent<Rigidbody2D>();
        initPos = transform.position;
        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D component)) {
            radiusOpposite = 1f/GetComponent<CircleCollider2D>().radius;
        }
        else {
            radiusOpposite = 0f;
        }
        
    }

    void Update()
    {
        if (isHaunted) {
            Move();
            if (Input.GetKeyDown(KeyCode.E)) {
                StopHauting();
            }
        }
        else if (!wasHaunted) {
            velocity = objectId.velocity.magnitude;
            if (rolling) objectId.angularVelocity = - velocity * radiusOpposite*7f;
        }
    }

    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerControl>(out PlayerControl player))
        {   
            if (velocity > 1){
                player.Death();
                rewindPlayer.killedByBoulder = true;
                rewindPlayer.culprit = this;
            }
        }
        else if (other.gameObject.CompareTag("Platform")) {
            rolling = true;
        }
    }
    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Platform")) {
            rolling = false;
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
        cam.SwitchTarget(rewindPlayer.gameObject);
        initPos = transform.position;
        //phantom.GetComponent<PhantomPlayer>().HideAll();
    }

    public void SaveState() //pour sauvegarder l'état de la boule quand le joueur reprend possession du player
    {
        if (!wasHaunted) {
            initPos = transform.position;
            checkpointVelocity = objectId.velocity;
        }
    }

    public void OnPlayerDeath()
    {
        if (!wasHaunted) {
            transform.position = initPos;
            objectId.velocity = Vector2.zero;
            Debug.Log("position reset");
        }
    }
    
}