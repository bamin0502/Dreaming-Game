using System;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private Sound[] effectSounds;
    [SerializeField] private Sound[] bgmSounds;

    public AudioSource bgmAudioSource;
    [SerializeField] private AudioSource[] effectAudioSources;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        bgmAudioSource.ignoreListenerPause = true;
        foreach (AudioSource effectAudioSource in effectAudioSources)
        {
            effectAudioSource.ignoreListenerPause = true;
        }
    }

    private void Start()
    {
        bgmAudioSource.volume = PlayerPrefs.GetFloat("bgmVolume", 1f);
        foreach (AudioSource effectAudioSource in effectAudioSources)
        {
            effectAudioSource.volume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        }
        bgmAudioSource.loop = true;
        PlayBGM("BGM");
    }

    private void PlayBGM(string bgm)
    {
        Sound bgmSound = System.Array.Find(bgmSounds, sound => sound.name == bgm);
        if (bgmSound != null)
        {
            bgmAudioSource.clip = bgmSound.clip;
            bgmAudioSource.Play();
        }
        else
        {
            Debug.Log(bgm + " sound is not registered in SoundManager.");
        }
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        bgmAudioSource.volume = volume;
        
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        foreach (AudioSource effectAudioSource in effectAudioSources)
        {
            effectAudioSource.volume = volume;
        }
        
    }

    public void PlaySE(string name)
    {
        Sound effectSound = System.Array.Find(effectSounds, sound => sound.name == name);
        if (effectSound != null)
        {
            foreach (AudioSource effectAudioSource in effectAudioSources)
            {
                if (effectAudioSource.isPlaying) continue;
                effectAudioSource.clip = effectSound.clip;
                effectAudioSource.PlayOneShot(effectSound.clip);
                break;
            }
        }
        else
        {
            Debug.Log(name + " sound is not registered in SoundManager.");
        }
    }

    public void StopSE(string name)
    {
        Sound effectSound = System.Array.Find(effectSounds, sound => sound.name == name);
        if (effectSound != null)
        {
            foreach (AudioSource effectAudioSource in effectAudioSources)
            {
                if (!effectAudioSource.isPlaying || effectAudioSource.clip != effectSound.clip) continue;
                effectAudioSource.Stop();
                break;
            }
        }
        else
        {
            Debug.Log(name + " sound is not registered in SoundManager.");
        }
    }
    
    public void MuteAllSound(bool isMute)
    {
        bgmAudioSource.mute = isMute;
        foreach (AudioSource effectAudioSource in effectAudioSources)
        {
            effectAudioSource.mute = isMute;
        }

        if (isMute || Time.timeScale == 0)
        {
            AudioListener.pause = isMute;
        }
        
    }
}