using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance; // 싱글톤을 할당할 전역 변수

    public Image gameoverUI; // 게임 오버 UI
    public Slider healthSlider; // 체력 슬라이더
    
    private void Start()
    {
        //gameoverUI.gameObject.SetActive(false);
        healthSlider.maxValue = 100;
    }
    
    private void Awake()
    {
        // 싱글톤 할당
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
}
