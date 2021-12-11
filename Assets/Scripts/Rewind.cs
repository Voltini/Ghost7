using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rewind : MonoBehaviour
{
    float time;
    public struct rewindData
    {
        public float playerTime;
        public Vector3 playerPosition;

        public rewindData(float playerTime, Vector3 playerPosition)
        {
            this.playerTime = playerTime;
            this.playerPosition = playerPosition;
        }
    }
    public float deathTime;
    public LineRenderer line;
    int i = 0;
    public List<rewindData> rewindPositions;
    public int counter = 0;
    public int length;
    Rigidbody2D playerId;
    float timeFactor = 1f;
    public GameObject player;
    public GameObject phantom;
    public CameraControl cam;
    public bool shouldLoop = false;

    void Start()
    {
        Debug.Log("nombre de points en mémoire : " + length);   //j'ai laisé ça provisoirement pour checker si ça risquait pas d'avoir un impact sur les performances
        playerId = GetComponent<Rigidbody2D>();
        line.positionCount = 0;
    }

    void Update()
    {
        time = timeFactor * (Time.timeSinceLevelLoad - deathTime);
        if (counter < length - 2)
        {
            while (time >= rewindPositions[counter + 1].playerTime && (counter < length - 2))
            {
                counter++;
                //l'idée de la boucle while là c'est d'éviter une désynchro si le framerate pendant la phase avant le décès est plus élevé qu'après le décès
                //ça parait pas super important mais ce sera peut-etre utile quand il y aura des animations
            }
            playerId.DOMove(rewindPositions[counter].playerPosition, Time.deltaTime);
            line.positionCount ++;
            line.SetPosition(line.positionCount-1, rewindPositions[counter].playerPosition);
            //DOMove c'est une fonction de DoTween qui est un asset (pas inclus de base dans Unity) qui permet d'avoir un déplacement lissé
        }
        else {
            if (shouldLoop) {
                line.positionCount = 0;
                counter = 0;
                deathTime = Time.timeSinceLevelLoad;
                if (!phantom.activeSelf) {}
            }
            else {
                if (phantom.activeSelf) {
                    phantom.SetActive(false);
                }
                else {
                    cam.CancelHaunting();
                }
                line.positionCount = 0;
                player.transform.position = transform.position;
                player.GetComponent<PlayerControl>().reactivatedTime = Time.timeSinceLevelLoad;
                player.GetComponent<PlayerControl>().i = 0;
                player.SetActive(true);
                cam.SwitchTarget(player);
                counter = 0;
                rewindPositions = new List<rewindData>();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        counter = length;
        other.gameObject.layer = LayerMask.NameToLayer("Haunted");
        shouldLoop = false;
    }

    public void ResetRewind()
    {
        deathTime = Time.timeSinceLevelLoad;
        counter = 0;
    }

}