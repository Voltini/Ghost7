using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public LineRenderer line;
    public SpringJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (joint != null) {
            line.SetPosition(0, joint.connectedBody.position);
            line.SetPosition(1, joint.attachedRigidbody.position);
        }
        else {
            gameObject.SetActive(false);
        }
        //joint.
    }

}
