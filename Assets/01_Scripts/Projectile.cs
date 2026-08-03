using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ProjectileMovement  p_movement;     // 발사체 이동 제어 컴포넌트
    private Transform           _target;        // 발사체가 향해가는 공격 대상

    public void Setup(Transform target)
    {
        p_movement = GetComponent<ProjectileMovement>();

        // 영웅으로부터 전달받은 공격 대상 저장
        _target = target;
    }

    private void Update()
    {
        // 공격 대상이 존재할 때
        if ( _target != null )
        {
            // 발사체가 공격 대상을 향해 날아가도록 설정
            Vector3 direction = (_target.position - transform.position).normalized;
            p_movement.MoveTo(direction);
        }

        // 공격 대상이 사라졌을 때
        else
            Destroy(gameObject);    // 발사체 오브젝트도 삭제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 발사체가 적만 공격할 수 있도록 함
        if (!collision.CompareTag("Enemy"))
            return;

        // 발사체가 지정된 공격 대상만 공격할 수 있도록 함
        if (collision.transform != _target)
            return;

        // 적 공격
        // TODO : 적 체력 시스템 추가
        collision.GetComponent<Enemy>().OnDie();
        Destroy(gameObject);
    }
}
