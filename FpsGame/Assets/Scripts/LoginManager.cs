using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // 사용자 아이디 변수
    public InputField id;

    public InputField password;

    // 검사 텍스트 변수
    public Text notify;

    private void Start()
    {
        notify.text = "";
    }

    // 아이디와 패스워드 저장 함수
    public void SaveUserData()
    {
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        if (!PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "아이디 생성이 완료되었습니다.";
        }
        else
        {
            notify.text = "이미 존재하는 아이디입니다.";
        }
    }

    public void CheckUserData()
    {
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        string pass = PlayerPrefs.GetString(id.text);

        if(password.text == pass)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            notify.text = "입력하신 아이디와 패스워드가 일치하지 않습니다";
        }
    }

    bool CheckInput(string id, string pwd)
    {
        if(id == "" || pwd == "")
        {
            notify.text = "아이디 또는 패스워드를 입력해주세요";
            return false;
        }
        else
        {
            return true;
        }
    }
}
