using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    Rigidbody2D arrowId;
    Collider arrowCollider;
    ParticleSystem arrowImpact;
    SoundManager soundManager;
    [HideInInspector] public Dispenser dispenser;
    Rewind rewindPlayer;
    float distanceTraveled;
    float periode = 3f;

    // Start is called before the first frame update
    void Start()
    {
        arrowId = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<BoxCollider>();
        arrowId.gravityScale = 0;
        arrowImpact = GameObject.FindGameObjectWithTag("ArrowImpact").GetComponent<ParticleSystem>();
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlaySfx(transform, "arrowShot");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        soundManager.PlaySfx(transform, "arrowImpact");
        arrowImpact = Instantiate(arrowImpact, transform.position, Quaternion.Euler(0f,0f, transform.eulerAngles.z - 22.5f));
        arrowImpact.Play();
        if (other.gameObject.TryGetComponent<PlayerControl>(out PlayerControl player))
        {
            player.Death(); 
            player.rewindPlayer.dispenserCulprit = dispenser;
            player.rewindPlayer.shouldLoop = true;
            player.rewindPlayer.killedByArrow = true; 
            player.rewindPlayer.ResetRewind();
            distanceTraveled = arrowId.velocity.magnitude * (Time.timeSinceLevelLoad % periode);
        }
        else {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Rewind")) {
            rewindPlayer = other.GetComponent<Rewind>();
            rewindPlayer.ResetRewind();
            Destroy(gameObject);
        }
    }
    
}
