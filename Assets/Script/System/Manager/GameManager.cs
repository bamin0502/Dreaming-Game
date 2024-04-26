using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤을 할당할 전역 변수
    public CinemachineVirtualCamera virtualCamera; // 카메라
    public int score = 0; // 게임 점수

    public Cursor cursor;

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
    
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AddScore(int score)
    {
        this.score += score;
        UiManager.instance.UpdateScore(this.score);
    }
    
    

}
