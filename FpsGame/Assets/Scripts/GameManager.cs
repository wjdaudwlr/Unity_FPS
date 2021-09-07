using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    // 현재의 게임 상태 변수
    public GameState gState;

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;

    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    public static GameManager gm;

    // PlayerMove 클래스 변수
    PlayerMove player;

    public GameObject gameOption;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    void Start()
    {
        gState = GameState.Ready;

        gameText = gameLabel.GetComponent<Text>();

        gameText.text = "Ready...";

        gameText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if(player.hp <= 0)
        {
            gameLabel.SetActive(true);

            gameText.text = "Game Over";

            gameText.color = new Color32(255, 0, 0, 255);

            Transform buttons = gameText.transform.GetChild(0);

            buttons.gameObject.SetActive(true);

            gState = GameState.GameOver;


        }    
    }

    IEnumerator ReadyToStart()
    {
        // 2초 대기
        yield return new WaitForSeconds(2f);

        // 상태 텍스트의 내용을 Go로 한다
        gameText.text = "Go!";

        // 0.5 초 대기
        yield return new WaitForSeconds(0.5f);

        gameLabel.SetActive(false);

        gState = GameState.Run;
    }

    // 옵션 화면 켜기
    public void OpenOptionWindow() 
    {
        gameOption.SetActive(true);

        Time.timeScale = 0f;

        gState = GameState.Pause;
    }

    // 계속하기 옵션
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);

        Time.timeScale = 1f;

        gState = GameState.Run;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(1);
    }

    public void QutiGame()
    {
        Application.Quit();
    }

}
