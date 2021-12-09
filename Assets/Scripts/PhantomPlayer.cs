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
    public float runSpeedx;
    CameraControl cam;
    float movementx = 0f;
    float movementy = 0f;
    bool abilityToHaunt = false;
    GameObject toHaunt;

    // Start is called before the first frame update
    void Start()
    {
        phantomId = GetComponent<Rigidbody2D>();
        phantomCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        phantomPos = phantomId.transform.position;
        movementx = Input.GetAxis("Horizontal");
        movementy = Input.GetAxis("Vertical");
        phantomId.velocity = new Vector2(speed * movementx, speed * movementy);
        if (abilityToHaunt && Input.GetKey(KeyCode.E))
        {
            toHaunt.transform.SetParent(transform);
        }
        else
        {
            toHaunt.transform.SetParent(p: null);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Hauntable"))
        {
            abilityToHaunt = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Hauntable"))
        {
            abilityToHaunt = false;
            toHaunt = other.gameObject;
        }
    }
}
