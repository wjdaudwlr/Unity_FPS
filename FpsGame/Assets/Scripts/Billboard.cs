using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        // �ڱ� �ڽ��� ������ ī�޶��� ����� ��ġ��Ų��.
        transform.forward = target.forward;
    }
}
