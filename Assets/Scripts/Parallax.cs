using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public Camera cam;
    Vector3 initPos;
    public float parallaxFactor;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initPos - parallaxFactor*cam.transform.position;
    }
}
