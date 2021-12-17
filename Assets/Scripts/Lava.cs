using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public ParticleSystem lavaParticles;
    public ParticleSystem lavaSplash;
    float density = 2;
    float splashDensity = 100f;
    // Start is called before the first frame update
    void Start()
    {
        lavaParticles = Instantiate(lavaParticles);
        lavaParticles.transform.position = transform.position + new Vector3(0f, transform.localScale.y*0.5f, 5f);
        var shape = lavaParticles.shape;
        shape.radius = transform.localScale.x * 0.5f;
        var em = lavaParticles.emission;
        em.rateOverTime = transform.localScale.x * density;
        lavaParticles.Play();
    }

    void OnTriggerEnter2D(Collider2D other) {
        var shape = lavaSplash.shape;
        if (other.TryGetComponent<CircleCollider2D>(out CircleCollider2D circle)) {
            shape.radius = 7 * circle.radius;
        }
        else if (other.TryGetComponent<BoxCollider2D>(out BoxCollider2D box)) {
            shape.radius = box.bounds.extents.x + box.bounds.extents.y;
        }
        var em = lavaSplash.emission;
        em.rateOverTime = shape.radius * splashDensity;
        lavaSplash.transform.position = new Vector3(other.transform.position.x, transform.position.y + transform.localScale.y*0.5f, 0f) ;
        lavaSplash.Play();
    }
}
