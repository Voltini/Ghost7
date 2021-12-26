using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public ParticleSystem lavaParticles;
    public ParticleSystem lavaSplash;
    float density = 2;
    float splashDensity = 100f;
    ParticleSystem lavaSplashInstance;
    float lavaFrontierY;

    void Start()
    {
        lavaParticles = Instantiate(lavaParticles);
        lavaParticles.transform.position = transform.position + new Vector3(0f, transform.localScale.y*0.5f, 5f);
        var shape = lavaParticles.shape;
        shape.radius = transform.localScale.x * 0.5f;
        var em = lavaParticles.emission;
        em.rateOverTime = transform.localScale.x * density;
        lavaParticles.Play();
        lavaFrontierY = transform.position.y + transform.localScale.y*0.5f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.position.y > lavaFrontierY) {
            if (!other.gameObject.CompareTag("Rewind")) {
                var shape = lavaSplash.shape;
                if (other.TryGetComponent<CircleCollider2D>(out CircleCollider2D circle)) {
                    shape.radius = 7 * circle.radius;
                }
                else if (other.TryGetComponent<BoxCollider2D>(out BoxCollider2D box)) {
                    shape.radius = box.bounds.extents.x + box.bounds.extents.y;
                }
                var em = lavaSplash.emission;
                em.rateOverTime = shape.radius * splashDensity;
                lavaSplash.transform.position = new Vector3(other.transform.position.x, lavaFrontierY, 0f);
                Instantiate(lavaSplash).Play();
                if (other.gameObject.CompareTag("Player")) {
                    other.GetComponent<PlayerControl>().rewindPlayer.shouldSplash = true;
                    other.GetComponent<PlayerControl>().rewindPlayer.rewindLavaSplash = Instantiate(lavaSplash);
                }
                else if (other.gameObject.CompareTag("startPlayer")) {
                    other.GetComponent<startPlayer>().rewindPlayer.rewindLavaSplash = Instantiate(lavaSplash);
                }
            }
        }
    }
}
