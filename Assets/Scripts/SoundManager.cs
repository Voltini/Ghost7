using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource MusicSource;
    public List<AudioClip> musics;
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
    bool jinglePlaying = false;

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
            volume = 0.9f;
            StartCoroutine("VolumeDown", 4f);
            //MusicSource.pitch = 0.8f;
            break;
            
            case "phantomDeath":
            sfxClip = phantomDeath;
            volume = 1f;
            StartCoroutine("VolumeDown", 4f);
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

        if (type != "phantomDeath") {
            PlayClipAt(sfxClip, pos.position, randomPitch, type, volume);
        }
        else {
            if (!jinglePlaying) {
                jinglePlaying = true;
                PlayClipAt(sfxClip, pos.position, randomPitch, type, volume);
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

    IEnumerator VolumeDown(float time)
    {
        MusicSource.volume = 0.4f;
        yield return new WaitForSeconds(time);
        MusicSource.volume = 1f;
        jinglePlaying = false;
    }

    public void PlayerMode()
    {
        MusicSource.pitch = 1f;
    }

}
