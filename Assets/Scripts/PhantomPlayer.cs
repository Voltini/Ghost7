using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhantomPlayer : MonoBehaviour
{
    float speed = 7f;
    Rigidbody2D playerId;
    Collider2D playerCollider;
    Vector3 playerPos;
    public float runSpeedx;
    CameraControl cam;
    float movementx = 0f;
    float movementy = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerId = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = playerId.transform.position;
        movementx = Input.GetAxis("Horizontal");
        movementy = Input.GetAxis("Vertical");
        playerId.velocity = new Vector2(speed * movementx, speed * movementy);
    }

}
