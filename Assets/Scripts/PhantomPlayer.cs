using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhantomPlayer : MonoBehaviour
{
    float speed = 7f;
    Rigidbody2D phantomId;
    Collider2D phantomCollider;
    Vector3 phantomPos;
    CameraControl cam;
    float movementx = 0f;
    float movementy = 0f;
    int hauntableLayer;
    

    // Start is called before the first frame update
    void Start()
    {
        phantomId = GetComponent<Rigidbody2D>();
        phantomCollider = GetComponent<Collider2D>();
        hauntableLayer = LayerMask.GetMask("Hauntable", "Haunted");
    }

    // Update is called once per frame
    void Update()
    {
        phantomPos = phantomId.transform.position;
        movementx = Input.GetAxis("Horizontal");
        movementy = Input.GetAxis("Vertical");
        phantomId.velocity = new Vector2(speed * movementx, speed * movementy);
        if (Input.GetKey(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 4f, Vector2.up, Mathf.Infinity,hauntableLayer);
            if (hit) {
                Debug.Log(hit.collider.tag);
                hit.collider.gameObject.GetComponent<Boulder>().Haunt();
                gameObject.SetActive(false);
            }
        }
    }
}
