using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Demon : MonoBehaviour
{

    SpriteRenderer sprite;
    Rigidbody2D rb;
    public mode pathMode;
    [Header("Linear Parameters")]
    public Vector2 direction;
    public float length;
    public float speed;

    [Header("Circular Parameters")]
    public float initialAngle;
    public float radius;
    float angle;
    Vector2 center;


    public enum mode {Line, Circle};

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (pathMode is mode.Line) {
            rb.DOMove(rb.position + length * direction.normalized, length/speed, false).SetLoops(-1, LoopType.Yoyo);
        }
        else {
            angle = initialAngle;
            center = rb.position + radius * new Vector2(Mathf.Cos(180 + initialAngle), Mathf.Sin(180 + initialAngle));
            StartCoroutine("CircularMotion");
        }
    }

    public void Show()
    {
        sprite.enabled = true;
    }

    public void Hide()
    {
        if (sprite != null) {
            sprite.enabled = false;
        }
    }

    IEnumerator CircularMotion()
    {
        while (true) {
            angle += speed * Time.deltaTime; 
            var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            rb.position = center + offset;
            yield return new WaitForEndOfFrame();
        }
    }
}
