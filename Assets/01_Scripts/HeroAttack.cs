using UnityEngine;
using System.Collections;

public enum AttackState { SearchClosestTarget = 0, AttackToTarget }

public class HeroAttack : MonoBehaviour
{
    [Header("발사체 세팅")]
    [SerializeField] private Transform  projectilePivot;        // 발사체 생성 위치
    [SerializeField] private GameObject projectilePrefab;       // 발사체 프리팹

    [Header("영웅 공격 세팅")]
    [SerializeField] private float      attackRate;             // 영웅의 공격 속도
    [SerializeField] private float      attackRange;            // 영웅의 공격 범위

    private EnemySpawner    _enemySpawner;      // 공격할 적 정보 참조용
    private Transform       currentTarget;      // 영웅의 현재 공격 대상
    private AttackState     currentState;       // 영웅의 현재 공격 상태

    private SpriteRenderer  spriteRenderer;
    private Animator        animator;
    private float           projectilePivotX;

    private void Awake()
    {
        spriteRenderer      = GetComponentInChildren<SpriteRenderer>();
        animator            = GetComponentInChildren<Animator>();
        projectilePivotX    = projectilePivot.localPosition.x;
    }

    public void Setup(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;

        // 공격 상태 초기 세팅
        ChangeState(AttackState.SearchClosestTarget);
    }

    public void ChangeState(AttackState newState)
    {
        // Step #1. 현재 동작 중인 상태 중단
        StopCoroutine(currentState.ToString());

        // Step #2. 새로운 상태로 변경
        currentState = newState;
        StartCoroutine(currentState.ToString());

        // Step #3. 공격 상태별 애니메이션 전환
        switch (currentState)
        {
            case AttackState.SearchClosestTarget:
                animator.SetBool("IsAttacking", false);
                break;
            case AttackState.AttackToTarget:
                animator.SetBool("IsAttacking", true);
                break;
        }
    }

    private void Update()
    {
        // 영웅 캐릭터가 공격 대상 방향을 바라보도록 설정
        if ( currentTarget != null )
            FlipToTarget();
    }

    // 영웅이 공격 대상을 바라보도록 스프라이트 및 projectilePivot 을 뒤집는 함수
    private void FlipToTarget()
    {
        bool isLeft = currentTarget.position.x < transform.position.x;

        spriteRenderer.flipX = isLeft;

        projectilePivot.localPosition = new Vector3(
            isLeft ? -projectilePivotX : projectilePivotX,
            projectilePivot.localPosition.y,
            projectilePivot.localPosition.z);
    }

    // 가장 가까이 위치한 적을 찾기 위한 코루틴 함수
    private IEnumerator SearchClosestTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        while ( true )
        {
            for ( int i = 0; i < _enemySpawner.EnemyList.Count; i++ )
            {
                // 현재 맵에 스폰된 모든 적과의 거리 검사
                float distance = Vector3.Distance(_enemySpawner.EnemyList[i].transform.position, transform.position);

                // 적이 공격 범위 내에 있으면서, 지금까지 검사한 적들 중 가장 가까울 때
                if ( distance <= attackRange && distance <= closestDistSqr )
                {
                    closestDistSqr = distance;
                    currentTarget = _enemySpawner.EnemyList[i].transform;
                }
            }

            // 선택된 타겟을 공격하는 상태로 변경
            if (currentTarget != null)
                ChangeState(AttackState.AttackToTarget);

            yield return null;
        }
    }

    // 선택된 타겟을 공격하기 위한 코루틴 함수
    private IEnumerator AttackToTarget()
    {
        while ( true )
        {
            // 공격 대상이 존재하는지 다시 한 번 검사
            if (currentTarget == null)
            {
                ChangeState(AttackState.SearchClosestTarget);
                break;
            }

            // 현재 공격 대상이 공격 범위 내에 위치하는지 확인
            float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
            if ( distance > attackRange )
            {
                currentTarget = null;
                ChangeState(AttackState.SearchClosestTarget);
                break;
            }

            // 영웅의 공격 속도(주기) 에 맞게 공격
            yield return new WaitForSeconds(attackRate);
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        var clone = Instantiate(projectilePrefab, projectilePivot.position, Quaternion.identity);

        // 생성한 발사체에 공격 대상 정보 전달
        clone.GetComponent<Projectile>().Setup(currentTarget);
    }
}
