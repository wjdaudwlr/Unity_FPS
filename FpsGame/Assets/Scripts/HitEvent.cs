using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    // ���ʹ� ��ũ��Ʈ ������Ʈ�� ����ϱ� ���� ����
    public EnemyFSM efsm;

    // �÷��̾�� �������� ������ ���� �̺�Ʈ �Լ�
    public void PlayerHit()
    {
        efsm.AttackAction();
    }
}
