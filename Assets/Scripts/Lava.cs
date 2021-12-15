using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public ParticleSystem lavaParticles;
    float density = 2;
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
}
