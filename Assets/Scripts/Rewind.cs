using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour
{
    float time;
    struct rewindData {
        public float startTime;
        public float endTime;

        public rewindData(float startTime, float endTime) {
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
    float startTimeZ;
    float endTimeZ;
    bool keyPressedZ = false;
    int counterZ = 0;

    List<rewindData> keyZ;

    void Start() 
    {
        keyZ = new List<rewindData>();
        keyZ.Add(new rewindData(5f, 6f));
        startTimeZ = keyZ[0].startTime;
        endTimeZ = keyZ[0].endTime;
    }

    void Update() 
    {
        CheckData();
        if (keyPressedZ) {
            Debug.Log(Time.timeSinceLevelLoad);
        }
    }

    void CheckData()
    {
        time = Time.timeSinceLevelLoad;
        CheckKeyZ();        
    }

    void CheckKeyZ()
    {
        if (!keyPressedZ) {
            if (time >= startTimeZ) {
                keyPressedZ = true;
            }
        }
        if (keyPressedZ) {
            if (time >= endTimeZ) {
                keyPressedZ = false;
                if (counterZ < keyZ.Count - 1) {
                    counterZ++;
                    startTimeZ = keyZ[counterZ].startTime;
                    endTimeZ = keyZ[counterZ].endTime;
                }
            }
        }
    }
}