using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public LineRenderer line;
    public SpringJoint2D joint;
    BoxCollider2D ropeCollider;
    public ParticleSystem ropeBreakEffect;
    Vector2 pos1;
    Vector2 pos2;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
        ropeCollider = GetComponent<BoxCollider2D>();
        line.SetPosition(0, joint.connectedBody.position);
        pos1 = joint.connectedBody.position;
        pos2 = joint.attachedRigidbody.position;
        ropeCollider.size = new Vector2(line.startWidth, (pos1 - pos2).magnitude);
    }

    // Update is called once per frame
    void Update()
    {
        pos2 = joint.attachedRigidbody.position;
        line.SetPosition(1, pos2);
        ropeCollider.size = new Vector2(line.startWidth, (pos1 - pos2).magnitude);
        ropeCollider.transform.position = 0.5f* (pos1 + pos2);
        Vector3 vectorToTarget = 0.5f * (pos1 - pos2);
        ropeCollider.transform.rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget); //nsm les quaternions ils m'ont cassé psychologiquement
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Arrow")) {
            ropeBreakEffect.transform.position = other.transform.position;
            joint.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
