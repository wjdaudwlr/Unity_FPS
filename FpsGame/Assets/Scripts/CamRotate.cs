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
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // ������� ���콺 �Է��� �޾� ��ü�� ȸ����Ű�� �ʹ�.
        // 1. ���콺 �Է��� �޴´�.
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        // 1-1. ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ������Ų��.
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        // 1-2. ���콺 ���� �̵� ȸ�� ����(my)�� ���� -90~90�� ���̷� �����Ѵ�.
        my = Mathf.Clamp(my, -90f, 90f);

        // 2. ���콺 �Է� ���� �̿��� ȸ�� ������ �����Ѵ�.
        transform.eulerAngles = new Vector3(-my, mx, 0);
        
        
    }
}
