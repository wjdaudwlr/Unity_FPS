using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState m_State;
    // HP
    public int hp = 15;
    int maxHp = 15;

    public Slider hpSlider;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    // �÷��̾� Ʈ������
    Transform player;

    // ���� ���� ����
    public float attackDistance = 2f;

    // �̵� �ӵ�
    public float moveSpeed = 5f;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // ���� �ð�
    float currentTime = 0;
    // ���� ������ �ð�
    float attackDelay = 2f;

    // ���ʹ��� ���ݷ�
    public int attackPower = 3;

    // �ʱ� ��ġ ����� ����
    Vector3 originPos;
    Quaternion originRot;
    // �̵� ���� ����
    public float moveDistance = 20f;

    Animator anim;

    // ������̼� ������Ʈ ����
    NavMeshAgent smith;

    void Start()
    {
        // ������ ���ʹ� ���´� ���
        m_State = EnemyState.Idle;

        // �÷��̾��� Ʈ������ ���۳�Ʈ �޾ƿ���
        player = GameObject.Find("Player").transform;

        cc = GetComponent<CharacterController>();
        // �ʱ� ��ġ�� ȸ�� �� �����ϱ�
        originPos = transform.position;
        originRot = transform.rotation;

        anim = GetComponentInChildren<Animator>();
        smith = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        hpSlider.value = (float)hp / (float)maxHp;
    }

    void Idle()
    {
        // ����, �÷��̾���� �Ÿ��� �׼� ���� ���� �̳���� Move ���·� ��ȯ
        if(Vector3.Distance(transform.position,player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            Debug.Log("������ȯ move");

            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        // ���� ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�
        if(Vector3.Distance(transform.position,originPos) > moveDistance)
        {
            // ���� ���¸� ����(Return)�� ��ȯ�Ѵ�.
            m_State = EnemyState.Return;
            print("���� ��ȭ: Move -> Return");
        }
        else if(Vector3.Distance(transform.position,player.position) > attackDistance)
        {
            // �̵� ���� ����
            //Vector3 dir = (player.position - transform.position).normalized;

            // ĳ���� ��Ʈ�ѷ��� �̿��� �̵��ϱ�
            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�
            smith.isStopped = true;
            smith.ResetPath();

            // ������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�
            smith.stoppingDistance = attackDistance;

            // ������̼��� �������� �÷��̾��� ��ġ�� �����Ѵ�
            smith.destination = player.position;
        }
        else
        {
            m_State = EnemyState.Attack;
            print("������ȯ Attack");

            currentTime = attackDelay;

            // ���� ��� �ִϸ��̼� �÷���
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        // ���� ������ ������ ����
        if(Vector3.Distance(transform.position,player.position) < attackDistance)
        {
            // ������ �ð����� �÷��̾ �����Ѵ�.
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;
                // ���� �ִϸ��̼� �÷���
                anim.SetTrigger("StartAttack");
            }
        }
        else // �ƴ� ����(Move)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Attack - > Move");
            currentTime = 0;

            // �̵� �ִϸ��̼� �÷���
            anim.SetTrigger("AttackToMove");
        }
    }

    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        // ����, �ʱ� ��ġ������ �Ÿ��� 0.1f �̻��̶�� �ʱ� ��ġ ������ �̵��Ѵ�.
        if(Vector3.Distance(transform.position,originPos) > 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;

            // ������̼��� �������� �ʱ� ������ ��ġ�� �����Ѵ�
            smith.destination = originPos;

            // ������̼����� �����ϴ� �ּ� �Ÿ��� '0'���� �����Ѵ�
            smith.stoppingDistance = 0;
        }
        else
        {
            // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�
            smith.isStopped = true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");

            anim.SetTrigger("MoveToIdle");
        }
    }
    
    public void HitEnemy(int hitPower)
    {

        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
            return;


        hp -= hitPower;

        // ������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�
        smith.isStopped = true;
        smith.ResetPath();

        if(hp > 0) // ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�.
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ : Any state -> Damaged");

            // �ǰ� �ִϸ��̼��� �÷����Ѵ�
            anim.SetTrigger("Damaged");

            Damaged();
        }
        else // �׷��� ������ ����
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ : Any state -> Die");

            // ���� �ִϸ��̼��� �÷����Ѵ�.
            anim.SetTrigger("Die");

            Die();
        }
    }

    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���
        yield return new WaitForSeconds(1f);

        // ���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        print("���� ��ȯ : Damaged �� Move");
    }

    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("�Ҹ�");
        Destroy(gameObject);
    }
}