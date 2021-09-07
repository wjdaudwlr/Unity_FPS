using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{

    public float rotSpeed = 200f;

    float mx = 0;
    float my = 0;

    void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 사용자의 마우스 입력을 받아 물체를 회전시키고 싶다.
        // 1. 마우스 입력을 받는다.
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // 1-1. 회전 값 변수에 마우스 입력 값만큼 미리 누적시킨다.
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        // 1-2. 마우스 상하 이동 회전 변수(my)의 값을 -90~90도 사이로 제한한다.
        my = Mathf.Clamp(my, -90f, 90f);

        // 2. 마우스 입력 값을 이용해 회전 방향을 결정한다.
        transform.eulerAngles = new Vector3(-my, mx, 0);
        
        
    }
}
