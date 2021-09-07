using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // �߷� ����
    float gravity = -20f;
    // ���� �ӷ� ����
    float yVelocity = 0;

    // ������
    public float jumpPower = 10f;
    // ���� ���� ����
    public bool isJumping = false;

    public int hp = 20;

    int maxHp = 20;

    // hp �����̴� ����
    public Slider hpSlider;

    // Hit ȿ�� ������Ʈ
    public GameObject hitEffect;

    Animator anim;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        // ���� ���°� '���� ��' ������ ���� ������ �� �ְ� �Ѵ�.
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 1. ������� �Է��� �޴´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. �̵� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // �̵� ���� Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ��ش�.
        anim.SetFloat("MoveMotion", dir.magnitude);

        // 2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        // 2-2 ����, ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�...
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;

            yVelocity = 0;
        }

        // 2-3. ���� Ű���� �����̽��� Ű�� �����ٸ�
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
        
        // 2-3. ĳ���� ���� �ӵ��� �߷� ���� �����Ѵ�.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. �̵� �ӵ��� ���� �̵��Ѵ�.
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 4. ���� �÷��̾� hp(%)�� hp �����̴��� value�� �ݿ��Ѵ�.
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
