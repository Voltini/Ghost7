using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource MusicSource;
    public List<AudioClip> musics;
    AudioClip lastPlayed;
    List<AudioClip> musicQueue;
    int queueIndex = 0;
    int musicQuantity;
    public static SoundManager Instance = null;
    private bool isTimeStopped = false;
    float LowPitchRange = 0.9f;
    float HighPitchRange = 1.1f;
    AudioClip sfxClip;
    List<clip> audioIds;

    [Header("Audio Clips")]
    public AudioClip jump;
    public AudioClip playerDeath;
    public AudioClip phantomDeath;
    public AudioClip haunted;
    public AudioClip arrowShot;
    public AudioClip arrowImpact;
    float volume;

    struct clip{
        public string type;
        public GameObject audio;

        public clip(string type, GameObject audio) {    //Constructor
            this.type = type;
            this.audio = audio;
        }
    }

    private void Awake()
    {
        MusicSource = GetComponent<AudioSource>();
        audioIds = new List<clip>();
        PlayMusic(musics[0]);
    }
    

    void PlayMusic(AudioClip music)
    {
       MusicSource.clip = music;
       MusicSource.Play();
    }

    public void StopTime()
    {
        MusicSource.Pause();
        if (audioIds.Count > 0) {
            foreach (clip audioId in audioIds) {
                if (audioId.audio != null) {
                    audioId.audio.GetComponent<AudioSource>().Pause();
                }
            }
        }
    }

    public void ResumeTime()
    {
        MusicSource.Play();
        if (audioIds.Count > 0) {
            foreach (clip audioId in audioIds) {
                if (audioId.audio != null) {
                    audioId.audio.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public void PlaySfx(Transform pos, string type)
    {
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        switch (type)
        {
            case "jump":
            sfxClip = jump;
            volume = 1f;
            break;

            case "playerDeath":
            sfxClip = playerDeath;
            volume = 0.7f;
            StartCoroutine("VolumeDown", sfxClip.length);
            break;
            
            case "phantomDeath":
            sfxClip = phantomDeath;
            volume = 1f;
            StartCoroutine("VolumeDown", sfxClip.length);
            break;

            case "haunted":
            sfxClip = haunted;
            volume = 1f;
            break;

            case "arrowImpact":
            sfxClip = arrowImpact;
            volume = 1f;
            break;

            case "arrowShot":
            sfxClip = arrowShot;
            volume = 1f;
            break;
        }
    
       PlayClipAt(sfxClip, pos.position, randomPitch, type, volume);

    }

    public void StopSfx(string type, float velocity)
    {
        foreach (clip audioId in audioIds) {
            if (audioId.type == type) {
                if (audioId.audio != null) {
                    StartCoroutine(SfxFadeOut(audioId, velocity));
                    break;
                }
            } 
        }
    }

    void PlayClipAt(AudioClip clip, Vector3 pos, float pitch, string type, float volume)       //create temporary audio sources for each sfx in order to be able to modify pitch
    {
        GameObject audioContainer = new GameObject("TempAudio"); // create the temporary object
        audioContainer.transform.position = pos; // set its position to localize sound
        AudioSource aSource = audioContainer.AddComponent<AudioSource>();
        aSource.pitch = pitch;
        aSource.clip = clip;
        aSource.volume = volume;
        clip audioId = new clip(type, audioContainer);
        audioIds.Add(audioId);
        aSource.Play(); // start the sound
        StartCoroutine(SfxTimer(clip.length, audioId));
    }

    IEnumerator SfxTimer(float time, clip audioId)  //2nd argument initially aSource
    {
        yield return new WaitForSeconds(time);
        audioIds.Remove(audioId);
        Destroy(audioId.audio);
    }

    IEnumerator SfxFadeOut(clip audioId, float velocity)
    {
        float time = 1f;
        float limit = 100;
        float volumeFactor = - (1 + velocity/10)/limit;
        AudioSource aSource = audioId.audio.GetComponent<AudioSource>();
        float inValue = aSource.volume;
        for (int i = 0; i < limit; i++){
            if (aSource != null) {
                aSource.volume = Mathf.Clamp01(Mathf.Exp(volumeFactor * i));
            }
            yield return new WaitForSeconds((float)time/limit);
        }
        audioIds.Remove(audioId);
        Destroy(audioId.audio);
    }

    IEnumerator VolumeDown(float time)
    {
        MusicSource.volume = 0.2f;
        yield return new WaitForSeconds(time);
        MusicSource.volume = 1f;
    }

}
