using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // 발사 위치
    public GameObject firePosition;

    // 투척 무기 오브젝트
    public GameObject bombFactory;

    // 투척 파워
    public float throwPower = 15f;

    // 피격 이펙트 오브젝트
    public GameObject bulletEffect;
    // 피격 이펙트 파티클 시스템
    ParticleSystem ps;

    public int weaponPower = 5;

    Animator anim;

    enum WeaponMode
    {
        Normal,
        Sniper
    }

    WeaponMode wMode;

    // 카메라 확대 확인용 변수
    bool ZoomMode;

    public Text wModeText;

    public GameObject[] eff_Flash;

    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();

        wMode = WeaponMode.Normal;
    }

    void Update()
    {
        // 게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 마우스 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:

                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    // 슈류탄 오브젝트의 Rigidbody 컴포넌트를 가져온다
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    // 카메라의 정면 방향으로 수류탄에 물리적인 힘을 가한다.
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                case WeaponMode.Sniper:
                    // 만일, 줌 모드 상태가 아니라면 카메라를 확대하고 줌 모드 상태로 변경한다.
                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                    }
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                    }
                    break;
            }
        }


        // 총
        if (Input.GetMouseButtonDown(0))
        {
            if(anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            // 레이를 생성한 후 발사될 위치와 진행 방향을 설정한다
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();

            // 레이를 발사한 후 만일 부딪힌 물체가 있으면 피격 이펙트를 표시한다
            if(Physics.Raycast(ray, out hitInfo))
            {
                // 만일 레이에 부딪힌 대상의 레이어가 'Enemy'라면 데미지 함수를 실행한다.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point; // point = 충돌한 위치의 좌표

                    // 피격 이펙트이 forwrad 방향을 레이가 부딪힌 지점의 법선 벡터(normal)와 일치시킨다.
                    bulletEffect.transform.forward = hitInfo.normal;

                    // 피격 이펙트를 플레이한다.
                    ps.Play();
                }
            }

            StartCoroutine(ShootEffectOn(0.05f));

        }

        // 만일 키보드의 숫자 1번 입력을 받으면, 무기 모드를 일반 모드로 변경한다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            Camera.main.fieldOfView = 60f;

            wModeText.text = "Normal Mode";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            wModeText.text = "Sniper Mode";
        }


    }

    IEnumerator ShootEffectOn(float duration)
    {
        // 숫자를 랜덤하게 뽑는다.
        int num = Random.Range(0, eff_Flash.Length -1);

        eff_Flash[num].SetActive(true);

        yield return new WaitForSeconds(duration);

        eff_Flash[num].SetActive(false);
    }


}
