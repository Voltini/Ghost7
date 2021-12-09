using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WibbleWobble : MonoBehaviour
{

    float speed = 0.02f;
    Vector3 initialPos;
    Vector3 newPos;
    float range = 0.2f;
    float time;
    float initialAngle;
    float newAngle;
    float angleTime;
    float angularSpeed = 0.5f;
    float angleRange = 5f;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        initialAngle = transform.eulerAngles.z;
        StartCoroutine("Wobble");
        StartCoroutine("Rotate");
    }

    IEnumerator Wobble()
    {
        newPos = initialPos + new Vector3(Random.Range(-range, range), Random.Range(-range, range));
        time = (newPos - initialPos).magnitude / speed;
        transform.DOMove(newPos, time);
        yield return new WaitForSeconds(time);
        StartCoroutine("Wobble");
    }

    IEnumerator Rotate()
    {
        newAngle = initialAngle + Random.Range(-angleRange, angleRange);
        angleTime = Mathf.Abs((newAngle - initialAngle)) / angularSpeed;
        transform.DORotate(new Vector3(0f,0f, newAngle), angleTime);
        yield return new WaitForSeconds(angleTime);
        StartCoroutine("Rotate");
    }

}
