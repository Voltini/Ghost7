using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    Rigidbody2D arrowId;
    Collider arrowCollider;

    // Start is called before the first frame update
    void Start()
    {
        arrowId = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<Collider>();
        arrowId.gravityScale = 1;
        arrowId.mass = 2;
        transform.localScale = new Vector2(1.5f, 0.5f);
    }

    void awake(){
        arrowId.gravityScale = 1;
        arrowId.mass = 2;
        transform.localScale = new Vector2(1.5f, 0.5f);
        print("arrow spawned");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
