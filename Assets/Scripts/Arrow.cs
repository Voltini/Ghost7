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
        arrowCollider = GetComponent<BoxCollider>();
        arrowId.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        

        if (other.gameObject.TryGetComponent(typeof(PlayerControl), out Component player))
        {
            other.gameObject.GetComponent<PlayerControl>().Death();  
        }
        gameObject.SetActive(false);
    }
}
