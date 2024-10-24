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

    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(){

    }
}

[System.Serializable]
public class Sound{
    public SoundManager.SoundsName name;
    public AudioClip audioClip;
    //public Volume volume;
}