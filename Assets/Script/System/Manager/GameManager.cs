using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤을 할당할 전역 변수
    public CinemachineVirtualCamera virtualCamera; // 카메라
    public int score = 0; // 게임 점수

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

    // 점수를 증가시키는 함수
    public void AddScore(int value)
    {
        score += value;
    }
    

}
