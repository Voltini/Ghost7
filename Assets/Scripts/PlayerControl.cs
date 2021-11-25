using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public int speed = 5;
    float movementx = 0f;
    Rigidbody2D playerId;
    Collider playerCollider;
    public int playerHealth = 10;
    public ParticleSystem playerExplosion;

    // Start is called before the first frame update
    void Start()
    {
        playerId = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        movementx = Input.GetAxis("Horizontal");
            playerId.velocity = new Vector2(7*movementx,playerId.velocity.y);
        if (Input.GetKeyDown(KeyCode.Z)) {
            playerId.AddForce(new Vector2(0f, 30000f));
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (playerHealth == 0) {
            Death();
        }
    }  

    void Death()
    {
        playerExplosion.transform.position = playerId.position;
        playerExplosion.Play();
        gameObject.SetActive(false);
    }
}