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
    public Volume postProcessing;
    public VolumeProfile playerProfile;
    public VolumeProfile phantomProfile;
    Animator anim;
    RaycastHit2D[] hits;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        phantomId = GetComponent<Rigidbody2D>();
        hauntableLayer = LayerMask.GetMask("Hauntable", "Haunted");
        anim = GetComponent<Animator>();
    }

    void OnEnable() {
        postProcessing.profile = phantomProfile;
    }

    void OnDisable() {
        postProcessing.profile = playerProfile;
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
                hits = Physics2D.CircleCastAll(transform.position, 4f, Vector2.up, 0f,hauntableLayer);
                if (hits.Length > 0) {
                    if (hits.Length == 1) {
                        hit = hits[0];
                    }
                    else {
                        hit = GetClosestObject(hits);
                    }
                    soundManager.PlaySfx(transform, "haunted");
                    if (hit.collider.CompareTag("Dispenser")) {
                        hit.collider.gameObject.GetComponent<Dispenser>().Haunt();
                    }
                    else {
                        hit.collider.gameObject.GetComponent<Boulder>().Haunt();
                    }
                    gameObject.SetActive(false);
                }
            }
            phantomId.velocity = new Vector2(speed * movementx, speed * movementy);
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

    RaycastHit2D GetClosestObject(RaycastHit2D[] hits)
{
    RaycastHit2D tMin = new RaycastHit2D();
    float minDist = Mathf.Infinity;
    Vector3 currentPos = transform.position;
    foreach (RaycastHit2D t in hits)
    {
        float dist = Vector2.Distance(t.transform.position, currentPos);
        if (dist < minDist)
        {
            tMin = t;
            minDist = dist;
        }
    }
    return tMin;
}

    void Death()
    {
        soundManager.PlaySfx(transform, "phantomDeath");
        isSucked = false;
        phantomDeath.transform.position = transform.position;
        phantomDeath.Play();
        transform.position = startPos;
    }
}
