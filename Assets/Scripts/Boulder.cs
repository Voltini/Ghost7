using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Boulder : MonoBehaviour
{
    Collider boulderCollider;
    bool isHaunted;
    float movementx;
    float movementy;
    Rigidbody2D objectId;
    float speed = 7;
    public GameObject player;


    void Start()
    {
        objectId = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isHaunted) {
            Move();
            if (Input.GetKeyDown(KeyCode.Return)) {
                player.SetActive(true);
                isHaunted = false;
            }
        }
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.TryGetComponent(typeof(PlayerControl), out Component player))
        {   
            if (objectId.velocity.magnitude > 5){
                other.gameObject.GetComponent<PlayerControl>().Death();
            }
           
           
        }
    }

    public void Haunt()
    {
        isHaunted = true;
    }

    void Move()
    {
        movementx = Input.GetAxis("Horizontal");
        movementy = Input.GetAxis("Vertical");
        objectId.velocity = new Vector2(speed * movementx, speed * movementy);
    }
}