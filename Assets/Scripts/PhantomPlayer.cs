using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomPlayer : MonoBehaviour
{
    float speed = 1f;
    Rigidbody2D playerId;
    Collider2D playerCollider;
    Vector3 playerPos;
    public float runSpeed;
    CameraControl cam;

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {      //Courir
            speed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 1f;
        }
    }

    public void GetCamera(Camera camera)
    {
        cam = camera.GetComponent<CameraControl>();     //ça c'est juste pour pouvoir appeler des screenshake depuis le script du joueur
    }
}
