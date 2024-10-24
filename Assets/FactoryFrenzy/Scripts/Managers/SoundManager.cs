using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public Sound[] musicSounds;
    public AudioSource musicSource;

    public enum SoundsName{
        Lobby,
        InGame,
    }

    private void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else
        {
            StopMusic();
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

    private void Start(){
        PlayMusic(SoundsName.Lobby);
    }

    public void StopMusic(){
        musicSource.Stop();
    }

    public void PlayMusic(SoundsName SoundName){
        Sound Sound = Array.Find(musicSounds, sound => sound.name == SoundName);

        if (Sound == null) return;

        musicSource.clip = Sound.audioClip;
        musicSource.Play();
    }
}

[System.Serializable]
public class Sound{
    public SoundManager.SoundsName name;
    public AudioClip audioClip;
    //public Volume volume;
}