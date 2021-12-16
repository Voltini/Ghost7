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
    [HideInInspector] public float deathTime;
    public LineRenderer line;
    public List<rewindData> rewindPositions;
    public List<animationData> animationList;
    [HideInInspector] public int counter = 0;
    [HideInInspector] public int animationCounter = 0;
    [HideInInspector] public int length;
    Rigidbody2D playerId;
    float timeFactor = 1f;
    public PlayerControl player;
    public GameObject phantom;
    public CameraControl cam;
    [HideInInspector] public bool shouldLoop = false;
    [HideInInspector] public List<Vector3> listPositions;
    Animator anim;
    [HideInInspector] public bool killedByBoulder = false;
    [HideInInspector] public Boulder culprit;
    [HideInInspector] public bool killedByArrow = false;
    [HideInInspector] public Dispenser dispenserCulprit;
    Dispenser[] dispensers;
    bool dispensersDefined = false;
    GameObject[] arrows;
    PlayerControl playerControl;


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
        if (other.CompareTag("Arrow")) {
            RewindDeath();
        }
        if (other.TryGetComponent<Boulder>(out Boulder boulder)) {
            if (!boulder.wasHaunted) {
                counter = length;
                other.gameObject.layer = LayerMask.NameToLayer("Haunted");
                shouldLoop = false;
            }
        }
    }

    public void RewindDeath()
    {
        rewindPositions = rewindPositions.GetRange(0, counter+1);
        animationList = animationList.GetRange(0,animationCounter+1);
        length = counter;
        ResetRewind();
    }

    public void ResetRewind()
    {
        deathTime = Time.timeSinceLevelLoad;
        counter = 0;
        animationCounter = 0;
        line.positionCount = 0;
        line.positionCount = length;
        StartCoroutine("TimelineSetBack");
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
            if (killedByBoulder) {
                if (!culprit.wasHaunted) {
                    shouldLoop = true;
                    culprit.OnPlayerDeath();
                }
                else {
                    shouldLoop = false;
                    killedByBoulder = false;
                }
            }
            else if (killedByArrow) {
                Debug.Log("killed by arrow");
                if (!dispenserCulprit.wasHaunted) {
                    //shouldLoop = true;
                    Debug.Log("was haunted");
                }
                else {
                    shouldLoop = false;
                    killedByBoulder = false;
                }
            }
            if (shouldLoop) {
                ResetRewind();
            }
            else {
                StopRewind();
            }
        }
    }
    void UpdateAnimation()
    {
        if (animationCounter < animationList.Count - 1) {
            if (time >= animationList[animationCounter+1].playerTime) {
                animationCounter ++;
                anim.Play(animationList[animationCounter].animation);
            }
        }
    }

    void StopRewind()
    {
        Debug.Log("rewind stopped");
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
        player.gameObject.SetActive(true);
        cam.SwitchTarget(player.gameObject);
        counter = 0;
        rewindPositions = new List<rewindData>();
        animationList = new List<animationData>();
        gameObject.SetActive(false);
    }

    IEnumerator TimelineSetBack()
    {
        arrows = GameObject.FindGameObjectsWithTag("Arrow"); 
        foreach(GameObject arrow in arrows) {
            Destroy(arrow);
        }
        foreach (Boulder boulder in player.boulders) {
            boulder.OnPlayerDeath();
        }
        foreach (Dispenser dispenser in player.dispensers) {
            dispenser.RestoreState();
        }
        yield return null;
    }

}