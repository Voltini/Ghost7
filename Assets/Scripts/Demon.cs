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
    bool facing_right;
    bool prev_facing_right;
    Animator anim;
    Vector2 offset;


    public enum mode {Line, Circle};

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (pathMode is mode.Line) {
            StartCoroutine("LinearMotion");
        }
        else {
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
        angle = (Mathf.PI/180f) * initialAngle;
        center = rb.position + radius * new Vector2(Mathf.Cos(180 + initialAngle), Mathf.Sin(180 + initialAngle));
        while (true) {
            angle += (speed * Time.deltaTime) % (2*Mathf.PI); 
            Debug.Log(angle);
            offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            rb.DOMove(center + offset, Time.deltaTime);
            facing_right = offset.y > 0;
            if (facing_right != prev_facing_right) {
                anim.SetBool("facing_right", facing_right);
                prev_facing_right = facing_right;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator LinearMotion()
    {
        float time = length/speed;
        while (true) {
            rb.DOMove(rb.position + length * direction.normalized, time, false).SetEase(Ease.Linear);       //motion to the right
            yield return new WaitForSeconds(time);
            anim.SetBool("facing_right", true);
            rb.DOMove(rb.position - length * direction.normalized, time, false).SetEase(Ease.Linear);       //motion to the left
            yield return new WaitForSeconds(time);
            anim.SetBool("facing_right", false);
        }
    }
}
