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
    bool facing_up;
    bool prev_facing_up;
    Animator anim;
    Vector2 offset;
    string facing_direction;
    float deltax;
    float deltay;
    bool initialBool;
    


    public enum mode {Line, Circle};

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (pathMode is mode.Line) {
            deltax = Mathf.Abs(direction.x);
            deltay = Mathf.Abs(direction.y);
            if (deltax >= deltay) {
                facing_direction = "facing_right";
                if (deltax >= 0) {
                    initialBool = true;
                }
                else {
                    initialBool = false;
                }
            }
            else {
                facing_direction = "facing_up";
                if (deltay >= 0) {
                    initialBool = true;
                }
                else {
                    initialBool = false;
                }
            }
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
            Debug.Log("_____________________________________________________________________________________________________");
            angle += (speed * Time.deltaTime); 
            offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            deltax = Mathf.Abs(offset.x);
            deltay = Mathf.Abs(offset.y);
            rb.DOMove(center + offset, Time.deltaTime);     //on peut pas faire directement un mouvement circulaire du coup micro-mouvements tangents au cercle 
            Debug.Log(anim.GetBool("horizontal_movement"));
            if (deltay < deltax){
                anim.SetBool("horizontal_movement", false);
                if (offset.x > 0) {
                    anim.SetBool("facing_up", false);
                }
                else if (offset.x < 0) {
                    anim.SetBool("facing_up", true);
                }
            }
            else if (deltay > deltax) {
                anim.SetBool("horizontal_movement", true);
                if (offset.y > 0) {
                    anim.SetBool("facing_right", true);
                }
                else if (offset.y < 0) {
                    anim.SetBool("facing_right", false);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    

    IEnumerator LinearMotion()
    {
        float time = length/speed;
        while (true) {
            anim.SetBool(facing_direction, initialBool);
            rb.DOMove(rb.position + length * direction.normalized, time, false).SetEase(Ease.Linear);       //motion to the right
            yield return new WaitForSeconds(time);
            anim.SetBool(facing_direction, !initialBool);
            rb.DOMove(rb.position - length * direction.normalized, time, false).SetEase(Ease.Linear);       //motion to the left
            yield return new WaitForSeconds(time);
        }
    }
}
