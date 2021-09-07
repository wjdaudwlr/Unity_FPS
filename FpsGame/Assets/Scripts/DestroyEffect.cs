using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // ���ŵ� �ð� ����
    public float destroyTime = 1.5f;

    // ��� �ð� ������ ����
    float currentTime = 0;

    void Update()
    {
        if(currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
    }
}
