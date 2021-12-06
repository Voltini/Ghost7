using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    Camera cam;
    bool isPaused;
    bool isShaking;
    Vector3 playerPos;
    Vector3 shake;
    float shakeMagitude;
    float camSize;
    PlayerControl playerId;
    [SerializeField] float horizontalThreshold;
    [SerializeField] float verticalThreshold;
    Vector3 camPos;
    public Rewind rewindPlayer;
    public bool rewindTime = false;

    void Start() {
        cam = GetComponent<Camera>();
        shake = Vector3.zero;
        playerId = player.GetComponent<PlayerControl>();
        camSize = cam.orthographicSize;
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        playerId.GetCamera(cam);
        camPos = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {      //restart level
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (isShaking) {
            Shake();
        }

        if (!rewindTime) {
            playerPos = player.transform.position;
        }
        else {
            playerPos = rewindPlayer.transform.position;
        }

        if (camPos.x - playerPos.x >= horizontalThreshold) {
            camPos = new Vector3(playerPos.x + horizontalThreshold,camPos.y, camPos.z); 
        }
        else if (camPos.x - playerPos.x <= - horizontalThreshold) {
            camPos = new Vector3(playerPos.x - horizontalThreshold,camPos.y, camPos.z); 
        }
        if (camPos.y - playerPos.y >= verticalThreshold) {
            camPos = new Vector3(camPos.x,playerPos.y + verticalThreshold, camPos.z); 
        }
        else if (camPos.y - playerPos.y <= - verticalThreshold) {
            camPos = new Vector3(camPos.x,playerPos.y - verticalThreshold, camPos.z); 
        }
        cam.transform.position = camPos + shake;

    }

    public void ActivateShake(float time, float magnitude)
    {
        isShaking = true;
        shakeMagitude = magnitude;
        StartCoroutine(Wait(time));
    }

    public void StopShake()
    {
        isShaking = false;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        isShaking = false;
        shake = Vector2.zero;
    }

    void Shake()
    {
        shake = camSize*shakeMagitude * new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f))/50;
    }

}