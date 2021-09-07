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

    // 플레이어 발견 범위
    public float findDistance = 8f;

    // 플레이어 트랜스폼
    Transform player;

    // 공격 가능 범위
    public float attackDistance = 2f;

    // 이동 속도
    public float moveSpeed = 5f;

    // 캐릭터 콘트롤러 컴포넌트
    CharacterController cc;

    // 누적 시간
    float currentTime = 0;
    // 공격 딜레이 시간
    float attackDelay = 2f;

    // 에너미의 공격력
    public int attackPower = 3;

    // 초기 위치 저장용 변수
    Vector3 originPos;
    Quaternion originRot;
    // 이동 가능 범위
    public float moveDistance = 20f;

    Animator anim;

    // 내비게이션 에이전트 변수
    NavMeshAgent smith;

    void Start()
    {
        // 최초의 에너미 상태는 대기
        m_State = EnemyState.Idle;

        // 플레이어의 트랜스폼 컴퍼넌트 받아오기
        player = GameObject.Find("Player").transform;

        cc = GetComponent<CharacterController>();
        // 초기 위치와 회전 값 저장하기
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
        // 만일, 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환
        if(Vector3.Distance(transform.position,player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            Debug.Log("상태전환 move");

            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        // 만일 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면
        if(Vector3.Distance(transform.position,originPos) > moveDistance)
        {
            // 현재 상태를 복귀(Return)로 전환한다.
            m_State = EnemyState.Return;
            print("상태 전화: Move -> Return");
        }
        else if(Vector3.Distance(transform.position,player.position) > attackDistance)
        {
            // 이동 방향 설정
            //Vector3 dir = (player.position - transform.position).normalized;

            // 캐릭터 콘트롤러를 이용해 이동하기
            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다
            smith.isStopped = true;
            smith.ResetPath();

            // 내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다
            smith.stoppingDistance = attackDistance;

            // 내비게이션의 목적지를 플레이어의 위치로 설정한다
            smith.destination = player.position;
        }
        else
        {
            m_State = EnemyState.Attack;
            print("상태전환 Attack");

            currentTime = attackDelay;

            // 공격 대기 애니메이션 플레이
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        // 공격 범위에 있으면 공격
        if(Vector3.Distance(transform.position,player.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격한다.
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;
                // 공격 애니메이션 플레이
                anim.SetTrigger("StartAttack");
            }
        }
        else // 아님 말구(Move)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack - > Move");
            currentTime = 0;

            // 이동 애니메이션 플레이
            anim.SetTrigger("AttackToMove");
        }
    }

    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        // 만일, 초기 위치에서의 거리가 0.1f 이상이라면 초기 위치 쪽으로 이동한다.
        if(Vector3.Distance(transform.position,originPos) > 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;

            // 내비게이션의 목적지를 초기 지정된 위치롤 설정한다
            smith.destination = originPos;

            // 내비게이션으로 접근하는 최소 거리를 '0'으로 설정한다
            smith.stoppingDistance = 0;
        }
        else
        {
            // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다
            smith.isStopped = true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환 : Return -> Idle");

            anim.SetTrigger("MoveToIdle");
        }
    }
    
    public void HitEnemy(int hitPower)
    {

        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
            return;


        hp -= hitPower;

        // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다
        smith.isStopped = true;
        smith.ResetPath();

        if(hp > 0) // 체력이 0보다 크면 피격 상태로 전환한다.
        {
            m_State = EnemyState.Damaged;
            print("상태 전환 : Any state -> Damaged");

            // 피격 애니메이션을 플레이한다
            anim.SetTrigger("Damaged");

            Damaged();
        }
        else // 그렇지 않으면 죽음
        {
            m_State = EnemyState.Die;
            print("상태 전환 : Any state -> Die");

            // 죽음 애니메이션을 플레이한다.
            anim.SetTrigger("Die");

            Die();
        }
    }

    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다
        yield return new WaitForSeconds(1f);

        // 현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환 : Damaged → Move");
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
        print("소멸");
        Destroy(gameObject);
    }
}
