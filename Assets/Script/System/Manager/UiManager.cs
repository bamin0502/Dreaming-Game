using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance; // 싱글톤을 할당할 전역 변수

    public Image gameoverUI; // 게임 오버 UI

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
    
}
