using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // �߻� ��ġ
    public GameObject firePosition;

    // ��ô ���� ������Ʈ
    public GameObject bombFactory;

    // ��ô �Ŀ�
    public float throwPower = 15f;

    // �ǰ� ����Ʈ ������Ʈ
    public GameObject bulletEffect;
    // �ǰ� ����Ʈ ��ƼŬ �ý���
    ParticleSystem ps;

    public int weaponPower = 5;

    Animator anim;

    enum WeaponMode
    {
        Normal,
        Sniper
    }

    WeaponMode wMode;

    // ī�޶� Ȯ�� Ȯ�ο� ����
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
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� �Ѵ�.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // ���콺 ��Ŭ��
        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:

                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    // ����ź ������Ʈ�� Rigidbody ������Ʈ�� �����´�
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    // ī�޶��� ���� �������� ����ź�� �������� ���� ���Ѵ�.
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
                case WeaponMode.Sniper:
                    // ����, �� ��� ���°� �ƴ϶�� ī�޶� Ȯ���ϰ� �� ��� ���·� �����Ѵ�.
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


        // ��
        if (Input.GetMouseButtonDown(0))
        {
            if(anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            // ���̸� ������ �� �߻�� ��ġ�� ���� ������ �����Ѵ�
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // ���̰� �ε��� ����� ������ ������ ������ �����Ѵ�.
            RaycastHit hitInfo = new RaycastHit();

            // ���̸� �߻��� �� ���� �ε��� ��ü�� ������ �ǰ� ����Ʈ�� ǥ���Ѵ�
            if(Physics.Raycast(ray, out hitInfo))
            {
                // ���� ���̿� �ε��� ����� ���̾ 'Enemy'��� ������ �Լ��� �����Ѵ�.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point; // point = �浹�� ��ġ�� ��ǥ

                    // �ǰ� ����Ʈ�� forwrad ������ ���̰� �ε��� ������ ���� ����(normal)�� ��ġ��Ų��.
                    bulletEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ�� �÷����Ѵ�.
                    ps.Play();
                }
            }

            StartCoroutine(ShootEffectOn(0.05f));

        }

        // ���� Ű������ ���� 1�� �Է��� ������, ���� ��带 �Ϲ� ���� �����Ѵ�.
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
        // ���ڸ� �����ϰ� �̴´�.
        int num = Random.Range(0, eff_Flash.Length -1);

        eff_Flash[num].SetActive(true);

        yield return new WaitForSeconds(duration);

        eff_Flash[num].SetActive(false);
    }


}
