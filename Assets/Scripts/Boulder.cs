using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Boulder : MonoBehaviour
{
    Rigidbody2D boulderId;
    Collider boulderCollider;

    void Start()
    {
        boulderId = GetComponent<Rigidbody2D>();
        boulderCollider = GetComponent<Collider>();
        boulderId.gravityScale = 10;
        boulderId.mass = 20;

    }

    void Update()
    {

    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.TryGetComponent(typeof(PlayerControl), out Component player))
        {   
            if (boulderId.velocity.magnitude > 5){
                other.gameObject.GetComponent<PlayerControl>().Death();
            }
           
           
        }
    }
}