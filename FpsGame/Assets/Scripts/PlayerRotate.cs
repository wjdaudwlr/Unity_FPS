using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 200f;

    float mx = 0;

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

        // 1-1. ȸ�� �� ������ ���콺 �Է� ����ŭ �̸� ������Ų��.
        mx += mouse_X * rotSpeed * Time.deltaTime;

        // 1-2. ���콺 ���� �̵� ȸ�� ����(my)�� ���� -90~90�� ���̷� �����Ѵ�.

        // 2. ���콺 �Է� ���� �̿��� ȸ�� ������ �����Ѵ�.
        transform.eulerAngles = new Vector3(0, mx, 0);


    }
}
