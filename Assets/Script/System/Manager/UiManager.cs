using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public Image gameoverUI;
    public TextMeshProUGUI gameoverText;
    public Slider healthSlider;
    public TMP_Text scoreText;
    public Image PauseGameUI;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Slider MapBossSlider;
    
    private void Start()
    {
        gameoverUI.gameObject.SetActive(false);
        
        healthSlider.maxValue = 100;

    }

    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

        if (instance == null)
        {
            instance = this;
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
        gameoverText.DOFade(1, 3);
    }

    public void SetBgmVolume(float volume)
    {
        SoundManager.instance.SetBgmVolume(volume);
        PlayerPrefs.SetFloat("bgmVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        SoundManager.instance.SetSfxVolume(volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void MuteAllSound(bool isMute)
    {
        SoundManager.instance.MuteAllSound(isMute);
    }
    
    public void BossSystemUI(int guage)
    {
        //시간이 지날수록 조금씩 차오르도록 설정
        MapBossSlider.DOValue(guage, 1);
    }
}