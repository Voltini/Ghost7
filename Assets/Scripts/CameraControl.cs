using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    float horizontalThreshold;
    float verticalThreshold;
    Vector3 camPos;
    public Rewind rewindPlayer;
    public GameObject phantomPlayer;
    public bool rewindTime = false;
    GameObject target;
    public Image blackscreen;
    public mode gameMode;
    public enum mode {Normal, Tutorial, Ending};

    void Start()
    {
        cam = GetComponent<Camera>();
        shake = Vector3.zero;
        if (gameMode is mode.Normal) {player.GetComponent<PlayerControl>().GetCamera(cam);}
        else if (gameMode is mode.Tutorial) {player.GetComponent<startPlayer>().GetCamera(cam);}
        else {player.GetComponent<endPlayer>().GetCamera(cam);}
        camSize = cam.orthographicSize;
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        camPos = this.transform.position;
        horizontalThreshold = 4;
        verticalThreshold = 2;
        target = player;
        StartCoroutine("FadeIn");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {      //restart level
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (isShaking)
        {
            Shake();
        }

            playerPos = target.transform.position;

        if (camPos.x - playerPos.x >= horizontalThreshold)
        {
            camPos = new Vector3(playerPos.x + horizontalThreshold, camPos.y, camPos.z);
        }
        else if (camPos.x - playerPos.x <= -horizontalThreshold)
        {
            camPos = new Vector3(playerPos.x - horizontalThreshold, camPos.y, camPos.z);
        }
        if (camPos.y - playerPos.y >= verticalThreshold)
        {
            camPos = new Vector3(camPos.x, playerPos.y + verticalThreshold, camPos.z);
        }
        else if (camPos.y - playerPos.y <= -verticalThreshold)
        {
            camPos = new Vector3(camPos.x, playerPos.y - verticalThreshold, camPos.z);
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
        shake = camSize * shakeMagitude * new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)) / 50;
    }

    public void SwitchTarget(GameObject newTarget)
    {
        target = newTarget;
        if (target.TryGetComponent<Dispenser>(out Dispenser dispenser)) {
            cam.transform.position = target.transform.position;
        }
    }

    public void CancelHaunting()
    {
        if (target.TryGetComponent<Boulder>(out Boulder boulder)) {
            boulder.StopHauting();
        }
        else if (target.TryGetComponent<Dispenser>(out Dispenser dispenser)) {
            Debug.Log(true);
            dispenser.StopHaunting();
        }
    }

    IEnumerator FadeIn()
    {
        var image = blackscreen.GetComponent<Image>();
        for (int i = 0; i <= 100; i++) {
            image.color = Color.Lerp(Color.black, new Color(0,0,0, 0), 0.01f*i);
            yield return new WaitForSeconds(0.001f);
        }
    }

}
