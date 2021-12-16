using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PhantomPlayer : MonoBehaviour
{
    float speed = 7f;
    Rigidbody2D phantomId;
    CameraControl cam;
    float movementx = 0f;
    float movementy = 0f;
    int hauntableLayer;
    bool isSucked = false;
    Vector2 massCenter;
    Vector2 distance;
    public ParticleSystem phantomDeath;
    [HideInInspector] public Vector3 startPos;
    public Rewind rewindPlayer;
    public SoundManager soundManager;
    Demon[] demons;
    bool demonsDefined = false;
    bool isHaunting = false;
    public GameObject ShowOnPhantomMode;
    public Volume postProcessing;
    public VolumeProfile playerProfile;
    public VolumeProfile phantomProfile;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        phantomId = GetComponent<Rigidbody2D>();
        hauntableLayer = LayerMask.GetMask("Hauntable", "Haunted");
        anim = GetComponent<Animator>();
    }

    void OnEnable() {
        Debug.Log("enabled");
        postProcessing.profile = phantomProfile;
        if (!demonsDefined) {
            demons = FindObjectsOfType<Demon>();
            demonsDefined = true;
        }
        ShowOnPhantomMode.SetActive(true);
        foreach(Demon demon in demons) {
            demon.Show();
        }
    }

    void OnDisable() {
        postProcessing.profile = playerProfile;
        if (!isHaunting) {
            HideAll();
        }
    }

    public void HideAll()
    {
        ShowOnPhantomMode.SetActive(false);
        foreach(Demon demon in demons) {
            demon.Hide();
        }

    }

    // Update is called once per frame
    void Update()
    {
        movementx = Input.GetAxis("Horizontal");
        movementy = Input.GetAxis("Vertical");
        
        if (movementx > 0) {
            anim.SetBool("facing_right", true);
        }
        else if (movementx < 0) {
            anim.SetBool("facing_right", false);
        }

        if (movementx == 0f && movementy == 0f) {
            anim.SetBool("idle", true);
            anim.SetBool("going_up", false);
            anim.SetBool("going_down", false);
            anim.SetBool("going_horizontal", false);
        }
        else {
            anim.SetBool("idle", false);
            if (movementy > 0.1f) {
                anim.SetBool("going_up", true);
                anim.SetBool("going_down", false);
                anim.SetBool("going_horizontal", false);
            }
            else if (movementy < -0.1f) {
                anim.SetBool("going_up", false);
                anim.SetBool("going_down", true);
                anim.SetBool("going_horizontal", false);
            }
            else {
                anim.SetBool("going_up", false);
                anim.SetBool("going_down", false);
                anim.SetBool("going_horizontal", true);
            }
        }
        

        if (!isSucked) {
            if (Input.GetKeyDown(KeyCode.E)) {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, 4f, Vector2.up, 0f,hauntableLayer);
                if (hit) {
                    soundManager.PlaySfx(transform, "haunted");
                    if (hit.collider.CompareTag("Dispenser")) {
                        hit.collider.gameObject.GetComponent<Dispenser>().Haunt();
                    }
                    else {
                        hit.collider.gameObject.GetComponent<Boulder>().Haunt();
                    }
                    isHaunting = true;
                    gameObject.SetActive(false);
                }
            }
            phantomId.velocity = new Vector2(speed * movementx, speed * movementy);
            if ((transform.position - startPos).sqrMagnitude > 2500 ) {        //pour éviter que le phantome se balade trop loin
                Death();
            }
        }
        else {
            distance = massCenter - phantomId.position;
            if (distance.magnitude > 0.5f) { 
            phantomId.AddForce(1f*distance.normalized);
            }
            else {
                Death();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("HellGate")) {
            phantomId.velocity = Vector2.zero;
            isSucked = true;
            massCenter = other.transform.position;
        }
        else if (other.CompareTag("Demon")) {
            Death();
        }
    }
    void Death()
    {
        soundManager.PlaySfx(transform, "phantomDeath");
        isSucked = false;
        phantomDeath.transform.position = transform.position;
        phantomDeath.Play();
        transform.position = startPos;
        //rewindPlayer.ResetRewind();
    }
}
