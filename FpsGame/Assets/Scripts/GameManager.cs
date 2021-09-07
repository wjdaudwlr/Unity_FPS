using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   

    // ���� ���� ���
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    // ������ ���� ���� ����
    public GameState gState;

    // ���� ���� UI ������Ʈ ����
    public GameObject gameLabel;

    // ���� ���� UI �ؽ�Ʈ ������Ʈ ����
    Text gameText;

    public static GameManager gm;

    // PlayerMove Ŭ���� ����
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
        // 2�� ���
        yield return new WaitForSeconds(2f);

        // ���� �ؽ�Ʈ�� ������ Go�� �Ѵ�
        gameText.text = "Go!";

        // 0.5 �� ���
        yield return new WaitForSeconds(0.5f);

        gameLabel.SetActive(false);

        gState = GameState.Run;
    }

    // �ɼ� ȭ�� �ѱ�
    public void OpenOptionWindow() 
    {
        gameOption.SetActive(true);

        Time.timeScale = 0f;

        gState = GameState.Pause;
    }

    // ����ϱ� �ɼ�
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
