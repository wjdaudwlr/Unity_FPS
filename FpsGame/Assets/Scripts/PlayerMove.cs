using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    // 캐릭터 콘트롤러 변수
    CharacterController cc;

    // 중력 변수
    float gravity = -20f;
    // 수직 속력 변수
    float yVelocity = 0;

    // 점프력
    public float jumpPower = 10f;
    // 점프 상태 변수
    public bool isJumping = false;

    public int hp = 20;

    int maxHp = 20;

    // hp 슬라이더 변수
    public Slider hpSlider;

    // Hit 효과 오브젝트
    public GameObject hitEffect;

    Animator anim;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 1. 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 이동 블랜딩 트리를 호출하고 벡터의 크기 값을 넘겨준다.
        anim.SetFloat("MoveMotion", dir.magnitude);

        // 2-1. 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);

        // 2-2 만일, 점프 중이었고, 다시 바닥에 착지했다면...
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;

            yVelocity = 0;
        }

        // 2-3. 만일 키보드 스페이스바 키를 눌렀다면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
        
        // 2-3. 캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. 이동 속도에 맞춰 이동한다.
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 4. 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영한다.
        hpSlider.value = (float)hp / (float)maxHp;
    }


    public void DamageAction(int damage)
    {
        hp -= damage;

        if(hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }


    IEnumerator PlayHitEffect()
    {
        hitEffect.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        hitEffect.SetActive(false);
    }

}
