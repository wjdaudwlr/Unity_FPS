using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    public int attackPower = 10;

    public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        // 폭팔 효과 반경 내에서 레이어가 'Enemy'인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다.
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);

        for (int i = 0; i< cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        // 이펙트 프리팹을 생성
        GameObject eff = Instantiate(bombEffect);

        // 이펙트 프리팹의 위치는 수류탄 오브젝트 자신의 위치와 동일하다
        eff.transform.position = transform.position;    

        Destroy(gameObject);
    }
}
