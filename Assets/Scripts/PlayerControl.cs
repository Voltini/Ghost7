using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int i = 0;
    Vector3 previousPosition = Vector3.positiveInfinity;
    Vector3 playerPos;
    public GameObject phantomPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerId = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        rewindPlayer.rewindPositions = new List<Rewind.rewindData>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = playerId.transform.position;
        if (playerPos != previousPosition) {       
            rewindPlayer.rewindPositions.Add(new Rewind.rewindData(Time.timeSinceLevelLoad,playerPos));
            //en gros on stocke les valeurs de position que lorsqu'elles sont différentes des précédentes et on utilise un time stamp 
            //pour s'assurer que le rewind a la meme vitesse que le joueur indépendamment du framerate
        }
        previousPosition = playerPos;

        line.positionCount = i+1;
        line.SetPosition(i, playerId.transform.position);
        i++;

        if (Input.GetKeyDown(KeyCode.LeftShift)) {      //Courir
            speed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {        
            speed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Z)) {      //D'ailleurs j'ai mis provisoirement R comme touche pour reload la scène
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.A)) {      //A pour mourir (pratique pour tester le rewind)
            Death();
        }

        movementx = Input.GetAxis("Horizontal");
        playerId.velocity = new Vector2(7*speed*movementx,playerId.velocity.y);

        if (isTouchingWall) {
            if (playerId.velocity.y < 0 && currentWall != lastWall) {
                playerId.gravityScale = wallSlideSpeed;
            }
            else {
                playerId.gravityScale = 1f;
            }
        }
    }  

    void Jump()
    {
        if (isGrounded) {
            playerId.velocity = new Vector2(playerId.velocity.x, 7f);
        }
        else if (isStickingToWallLeft || isStickingToWallRight) {
            WallJump();
        }
    }

    void WallJump()
    {
        if (currentWall != lastWall) {      //ça c'est pour éviter que le joueur puisse faire des wall jump infinement sur la meme mur et faire de l'escalade
            playerId.velocity = new Vector2(playerId.velocity.x, 7f);
        }
        lastWall = currentWall;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (isStickingToWallLeft || isStickingToWallRight) {
            currentWall = other.gameObject;
            isTouchingWall = true;
        }
        else {
            lastWall = null;        // si le joueur touche du sol la capacité de walljump se réinitialise
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Lava"){
            Death();
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (isTouchingWall) {
            playerId.gravityScale = 1f;
            isTouchingWall = false;
        }
    }

    public void GetCamera(Camera camera)
    {
        cam = camera.GetComponent<CameraControl>();     //ça c'est juste pour pouvoir appeler des screenshake depuis le script du joueur
    }

    public void Death()
    {
        playerExplosion.transform.position = playerId.position;
        playerExplosion.Play();
        this.
        cam.ActivateShake(0.2f,2f);
        phantomPlayer.SetActive(true);
        rewindPlayer.gameObject.SetActive(true);
        cam.rewindTime = true;      //pour que la caméra switch de cible (temporaire mais c'est pratique pour regarder ce qu'il se passe)
        gameObject.SetActive(false);    //décès du joueur
    }

    //PS : les lignes c'est provisoire aussi, juste pour vérifier que le rewind fonctionne bien
}