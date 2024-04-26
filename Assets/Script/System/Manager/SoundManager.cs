using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void MuteAllSound(bool isMute)
    {
        bgmSource.mute = isMute;
        sfxSource.mute = isMute;
    }

    public void PlayBgm(int index)
    {
        if (index < 0 || index >= bgmClips.Length)
            return;
        bgmSource.clip = bgmClips[index];
        bgmSource.Play();
        bgmSource.loop = true;
    }

    public void PlaySfx(int index)
    {
        if (index < 0 || index >= sfxClips.Length)
            return;
        sfxSource.PlayOneShot(sfxClips[index]);
    }
}