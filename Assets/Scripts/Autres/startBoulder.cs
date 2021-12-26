using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBoulder : MonoBehaviour
{
    Collider boulderCollider;
    bool isHaunted;
    float movementx;
    float movementy;
    Rigidbody2D objectId;
    float speed = 7;
    public startRewind rewindPlayer;
    public RigidbodyConstraints2D constraints;
    public RigidbodyConstraints2D freezeConstraints;
    public CameraControl cam;
    float velocity;
    Vector2 initPos;
    public startPhantom phantom;
    [HideInInspector] public bool wasHaunted = false;
    float radiusOpposite;
    bool rolling;
    Vector2 checkpointVelocity;
    public GameObject onHauntPanel;


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
                if (!TryGetComponent<SpringJoint2D>(out SpringJoint2D joint) || !joint.enabled) {
                    Vector2 direction = other.gameObject.transform.position - transform.position;
                    Quaternion angle = Quaternion.LookRotation(Vector3.forward, direction);
                    if (Mathf.DeltaAngle(Quaternion.LookRotation(Vector3.forward, objectId.velocity).eulerAngles.z,angle.eulerAngles.z) <= 45f) {
                        player.Death();
                    }
                }
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
        else if (other.CompareTag("Rewind")) {
            rewindPlayer.deathBylava = false;
            if (velocity > 1){
                if (!TryGetComponent<SpringJoint2D>(out SpringJoint2D joint) || !joint.enabled) {
                    Vector2 direction = other.gameObject.transform.position - transform.position;
                    Quaternion angle = Quaternion.LookRotation(Vector3.forward, direction);
                    if (Mathf.DeltaAngle(Quaternion.LookRotation(Vector3.forward, objectId.velocity).eulerAngles.z,angle.eulerAngles.z) <= 45f) {
                        rewindPlayer.RewindDeath();
                    }
                }
            }
            else {
                rewindPlayer.StopRewind();
                gameObject.layer = LayerMask.NameToLayer("Haunted");
            }
        }  
    }

    public void Haunt()
    {
        if (!wasHaunted) {
            onHauntPanel.SetActive(true);
        }
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
        cam.SwitchTarget(phantom.gameObject);
        phantom.transform.position = new Vector3(transform.position.x, transform.position.y, phantom.transform.position.z);
        phantom.gameObject.SetActive(true);
        initPos = transform.position;
        velocity = 0f;
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
            objectId.velocity = checkpointVelocity;
        }
    }
}
