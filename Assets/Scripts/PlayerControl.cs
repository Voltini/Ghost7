﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerControl : MonoBehaviour
{
    float speed = 1f;
    float movementx = 0f;
    Rigidbody2D playerId;
    Collider2D playerCollider;
    public ParticleSystem playerExplosion;
    public float runSpeed;
    public ContactFilter2D contactFilter;
    public ContactFilter2D contactWallRight;
    public ContactFilter2D contactWallLeft;
    bool isGrounded => playerId.IsTouching(contactFilter);
    bool isStickingToWallRight => playerId.IsTouching(contactWallRight);
    bool isStickingToWallLeft => playerId.IsTouching(contactWallLeft);
    float wallSlideSpeed = 0.2f;
    GameObject lastWall;
    GameObject currentWall;
    bool isTouchingWall = false;
    CameraControl cam;
    public bool playerDeath = false;

    public Rewind rewindPlayer;
    public LineRenderer line;
    public int i = 0;
    Vector3 previousPosition = Vector3.positiveInfinity;
    Vector3 playerPos;
    public GameObject phantomPlayer;
    public float reactivatedTime = 0f;
    public SoundManager soundManager;
    Animator anim;
    bool isJumping = false;
    string previousState = " ";
    string currentState;


    // Start is called before the first frame update
    void Start()
    {
        playerId = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rewindPlayer.rewindPositions = new List<Rewind.rewindData>();
        rewindPlayer.animationList = new List<Rewind.animationData>();
        cam.SwitchTarget(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = playerId.transform.position;
        if (playerPos != previousPosition)
        {
            rewindPlayer.rewindPositions.Add(new Rewind.rewindData(Time.timeSinceLevelLoad - reactivatedTime, playerPos));
            //en gros on stocke les valeurs de position que lorsqu'elles sont différentes des précédentes et on utilise un time stamp 
            //pour s'assurer que le rewind a la meme vitesse que le joueur indépendamment du framerate
            line.positionCount = i + 1;
            line.SetPosition(i, playerId.transform.position);
            i++;
        }
        previousPosition = playerPos;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {      //Courir
            speed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {      //D'ailleurs j'ai mis provisoirement R comme touche pour reload la scène
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {      //A pour mourir (pratique pour tester le rewind)
            Death();
        }

        movementx = Input.GetAxis("Horizontal");
        playerId.velocity = new Vector2(7 * speed * movementx, playerId.velocity.y);
        if (movementx > 0) {
            anim.SetBool("facing_right", true);
        }
        else if (movementx < 0) {
            anim.SetBool("facing_right", false);
        }

        if (isTouchingWall)
        {
            if (playerId.velocity.y < 0 && currentWall != lastWall)
            {
                playerId.gravityScale = wallSlideSpeed;
            }
            else
            {
                playerId.gravityScale = 1f;
            }
        }

        if (!isJumping) {
            if (Mathf.Abs(movementx) >= 0.1f) {anim.SetBool("isWalking", true); anim.SetBool("isIdle", false);}
            else if (Mathf.Abs(movementx) < 0.1f) {anim.SetBool("isWalking", false); anim.SetBool("isIdle", true);}
        }

    }

    void Jump()
    {
        if (isGrounded)
        {
            JumpAction();
        }
        else if (isStickingToWallLeft || isStickingToWallRight)
        {
            WallJump();
        }
    }

    void WallJump()
    {
        if (currentWall != lastWall)
        {      //ça c'est pour éviter que le joueur puisse faire des wall jump infinement sur la meme mur et faire de l'escalade
            JumpAction();
        }
        lastWall = currentWall;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Wall") || other.collider.CompareTag("Ground")) {
            if (isStickingToWallLeft || isStickingToWallRight)
            {
                currentWall = other.gameObject;
                isTouchingWall = true;
                anim.SetBool("isWallSliding", true);
                anim.SetBool("isJumping", false);
            }
            else
            {
                lastWall = null;        // si le joueur touche du sol la capacité de walljump se réinitialise
                if (isGrounded) {
                    isJumping = false;
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isWalking", true);
                }
            }
        }
        else {
            lastWall = null;        // si le joueur touche du sol la capacité de walljump se réinitialise
            if (isGrounded) {
                isJumping = false;
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", true); 
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Lava")
        {
            rewindPlayer.shouldLoop = true;
            Death();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Ground")) {
            playerId.gravityScale = 1f;
            isTouchingWall = false;
            isJumping = true;
            anim.SetBool("isWallSliding", false);
            anim.SetBool("isJumping", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", false);
        }
    }

    public void GetCamera(Camera camera)
    {
        cam = camera.GetComponent<CameraControl>();     //ça c'est juste pour pouvoir appeler des screenshake depuis le script du joueur
    }

    public void Death()
    {
        soundManager.PlaySfx(transform, "playerDeath");
        playerExplosion.transform.position = playerId.position;
        playerExplosion.Play();
        cam.ActivateShake(0.2f, 2f);
        phantomPlayer.transform.position = transform.position;
        phantomPlayer.SetActive(true);
        phantomPlayer.GetComponent<PhantomPlayer>().startPos = transform.position;
        rewindPlayer.deathTime = Time.timeSinceLevelLoad;
        rewindPlayer.length = rewindPlayer.rewindPositions.Count;
        rewindPlayer.animationCounter = 0;
        Vector3[] array = new Vector3[line.positionCount];
        line.GetPositions(array);
        rewindPlayer.listPositions = array.ToList();
        rewindPlayer.gameObject.SetActive(true);
        rewindPlayer.line.positionCount = line.positionCount;
        cam.SwitchTarget(phantomPlayer);      //pour que la caméra switch de cible (temporaire mais c'est pratique pour regarder ce qu'il se passe)
        gameObject.SetActive(false);    //décès du joueur
    }

    void JumpAction()
    {
        if (!isJumping) {
            isJumping = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", false);
        }
        playerId.velocity = new Vector2(playerId.velocity.x, 7f);
        soundManager.PlaySfx(transform,"jump");
        anim.SetBool("isJumping", true);
    }

    public void Checkpoint()
    {
        i = 0;
        reactivatedTime = Time.timeSinceLevelLoad;
        rewindPlayer.rewindPositions = new List<Rewind.rewindData>();
    }

    void LateUpdate() {
        currentState = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (currentState != previousState) {
            rewindPlayer.animationList.Add(new Rewind.animationData(Time.timeSinceLevelLoad - reactivatedTime, currentState));
            previousState = currentState;
        }
    }

    //PS : les lignes c'est provisoire aussi, juste pour vérifier que le rewind fonctionne bien
}