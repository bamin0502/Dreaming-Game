using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public Image gameoverUI;
    public Slider healthSlider;
    public TMP_Text scoreText;
    public Image PauseGameUI;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        gameoverUI.gameObject.SetActive(false);
        
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        
        bgmSlider.value = SoundManager.Inst.bgmSource.volume;
        sfxSlider.value = SoundManager.Inst.sfxSource.volume;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealth(int health)
    {
        healthSlider.value = health;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameoverUI.gameObject.SetActive(true);
        gameoverUI.DOFade(1, 3);
    }

    public void SetBgmVolume(float volume)
    {
        SoundManager.Inst.SetBgmVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        SoundManager.Inst.SetSfxVolume(volume);
    }

    public void MuteAllSound(bool isMute)
    {
        SoundManager.Inst.MuteAllSound(isMute);
    }
}