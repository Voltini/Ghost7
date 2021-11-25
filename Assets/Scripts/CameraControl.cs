using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start() {
        cam = GetComponent<Camera>();
        shake = Vector3.zero;
        playerId = player.GetComponent<PlayerControl>();
        camSize = cam.orthographicSize;
    }
    // Update is called once per frame
    void Update()
    {
        //PauseMenu pause = GameObject.FindObjectOfType<PauseMenu>();
        //isPaused = pause.isGamePaused;

        if (isShaking) {
            Shake();
        }

        if (player != null) {
            playerPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = playerPos + shake;
            //Debug.Log(shake);
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            ActivateShake(1f, 0.001f);
        }
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
        shake = Vector3.zero;
    }

    void Shake()
    {
        shake = camSize*shakeMagitude * new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f))/50;
    }

}
