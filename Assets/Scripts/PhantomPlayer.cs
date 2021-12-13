using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhantomPlayer : MonoBehaviour
{
    float speed = 7f;
    Rigidbody2D phantomId;
    Collider2D phantomCollider;
    Vector3 phantomPos;
    CameraControl cam;
    float movementx = 0f;
    float movementy = 0f;
    int hauntableLayer;
    bool isSucked = false;
    Vector2 massCenter;
    Vector2 distance;
    public ParticleSystem phantomDeath;
    [HideInInspector] public Vector2 startPos;
    public Rewind rewindPlayer;
    public SoundManager soundManager;
    Demon[] demons;
    bool demonsDefined = false;

    // Start is called before the first frame update
    void Start()
    {
        phantomId = GetComponent<Rigidbody2D>();
        phantomCollider = GetComponent<Collider2D>();
        hauntableLayer = LayerMask.GetMask("Hauntable", "Haunted");
    }

    void OnEnable() {
        if (!demonsDefined) {
            demons = FindObjectsOfType<Demon>();
            demonsDefined = true;
        }
        foreach(Demon demon in demons) {
            demon.Show();
        }
    }

    void OnDisable() {
        foreach(Demon demon in demons) {
            demon.Hide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        phantomPos = phantomId.transform.position;
        movementx = Input.GetAxis("Horizontal");
        movementy = Input.GetAxis("Vertical");
        if (!isSucked) {
            if (Input.GetKey(KeyCode.E)) {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, 4f, Vector2.up, Mathf.Infinity,hauntableLayer);
                if (hit) {
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
            if (((Vector2)transform.position - startPos).sqrMagnitude > 2500 ) {        //pour éviter que le phantome se balade trop loin
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
        rewindPlayer.ResetRewind();
    }
}
