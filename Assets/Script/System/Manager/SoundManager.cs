using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager Inst; // 싱글톤을 할당할 전역 변수
    
    public AudioClip[] bgmClips; // 배경음악 클립
    public AudioClip[] sfxClips; // 효과음 클립
    public AudioSource bgmSource; // 배경음악 소스
    public AudioSource sfxSource; // 효과음 소스
    
    
    private void Awake()
    {
        // 싱글톤 할당
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //음향 조절 
    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    //모든 음향 음소거
    public void MuteAllSound(bool isMute)
    {
        bgmSource.mute = isMute;
        sfxSource.mute = isMute;
    }
}
