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

    public struct animationData
    {
        public float playerTime;
        public string animation;

        public animationData(float playerTime, string animation)
        {
            this.playerTime = playerTime;
            this.animation = animation;
        }
    }
    public float deathTime;
    public LineRenderer line;
    public List<rewindData> rewindPositions;
    public List<animationData> animationList;
    public int counter = 0;
    public int animationCounter = 0;
    public int length;
    Rigidbody2D playerId;
    float timeFactor = 1f;
    public GameObject player;
    public GameObject phantom;
    public CameraControl cam;
    public bool shouldLoop = false;
    public List<Vector3> listPositions;
    Animator anim;


    void Start()
    {
        Debug.Log("nombre de points en mémoire : " + length);   //j'ai laisé ça provisoirement pour checker si ça risquait pas d'avoir un impact sur les performances
        playerId = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        time = timeFactor * (Time.timeSinceLevelLoad - deathTime);
        UpdatePosition();
        UpdateAnimation();
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
        animationCounter = 0;
        line.positionCount = 0;
        line.positionCount = length;
    }

    void UpdatePosition()
    {
        if (counter < length - 2)
        {
            while (time >= rewindPositions[counter + 1].playerTime && (counter < length - 2))
            {
                counter++;
                //l'idée de la boucle while là c'est d'éviter une désynchro si le framerate pendant la phase avant le décès est plus élevé qu'après le décès
                //ça parait pas super important mais ce sera peut-etre utile quand il y aura des animations
                //line.SetPosition(line.positionCount-1, rewindPositions[counter].playerPosition);
                //listPositions.RemoveAt(0);
                line.SetPositions(listPositions.GetRange(counter -1, line.positionCount - 1) .ToArray());
                line.positionCount --;
            }
            playerId.DOMove(rewindPositions[counter].playerPosition, Time.deltaTime);
            //DOMove c'est une fonction de DoTween qui est un asset (pas inclus de base dans Unity) qui permet d'avoir un déplacement lissé
        }
        else {
            if (shouldLoop) {
                line.positionCount = 0;     //ça c'est juste pour réinitialiser la liste sinon il reste des points pendant une frame et c'est bizarre
                line.positionCount = length;
                counter = 0;
                animationCounter = 0;
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
                animationList = new List<animationData>();
                gameObject.SetActive(false);
            }
        }
    }
    void UpdateAnimation()
    {
        if (animationCounter < animationList.Count - 1) {
            if (time >= animationList[animationCounter+1].playerTime) {
                animationCounter ++;
                anim.Play(animationList[animationCounter].animation);
                /*switch (animationList[animationCounter].animation) {
                    case "walk" :
                    anim.Play
                    break;
                }*/
            }
        }
    }

}