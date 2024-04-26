using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤을 할당할 전역 변수
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
    
    private void Start()
    {
        UiManager.instance.PauseGameUI.gameObject.SetActive(false);
        SoundManager.Inst.PlayBgm(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            UiManager.instance.PauseGameUI.gameObject.SetActive(true);
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
        UiManager.instance.UpdateScore(this.score);
    }
    
    public void GameQuit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public void ContinueGame()
    {
        Time.timeScale = 1;
        UiManager.instance.PauseGameUI.gameObject.SetActive(false);
    }

}
