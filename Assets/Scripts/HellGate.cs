using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGate : MonoBehaviour
{
    float force = 100000f;
    float gateRadius = 1f;
    Vector2 tangentForce;

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Phantom")) {
            Vector2 distance = transform.position - other.transform.position;
            if (distance.sqrMagnitude >= gateRadius) {
                tangentForce = Vector2.Perpendicular(-distance).normalized;
                other.attachedRigidbody.AddForce(1000f*tangentForce + force * distance.normalized/distance.sqrMagnitude);
            }
            Debug.Log("ah");
        }
    }
}
