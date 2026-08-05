using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("체력바 UI 세팅")]
    [SerializeField] private Transform  hpBarPivot;               // Inspector 뷰에서 추가
    public Transform                    HpBarPivot => hpBarPivot;   // 외부 참조용 프로퍼티

    [Header("적 정보 세팅")]
    [SerializeField] private float      timeOffset = 1.0f;      // 1칸 이동하는데 걸리는 시간
    [SerializeField] private float      attackRate;             // 적의 공격 속도
    [SerializeField] private float      attackDamage;           // 적의 공격력
    [SerializeField] private int        goldReward;             // 적 처치 시, 얻을 수 있는 골드

    private Transform[] _wayPoints;
    private int         wayPointCount;
    private int         currentIndex = 0;   // 현재 이동중인 웨이포인트의 인덱스

    private EnemySpawner    _enemySpawner;
    private PlayerHP        playerHP;       // 플레이어의 체력 정보 참조

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        _enemySpawner   = enemySpawner;
        playerHP        = GameManager.Instance.PlayerHP;

        // 적 이동 경로(Waypoint) 정보 초기 세팅
        wayPointCount = wayPoints.Length;
        _wayPoints = new Transform[wayPointCount];
        _wayPoints = wayPoints;

        // 적 위치를 첫번째 Waypoint(적 소환 포탈) 위치로 설정
        transform.position = _wayPoints[currentIndex].position;

        currentIndex++;

        // 적 이동 제어 코루틴 함수 호출
        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process()
    {
        while ( true )
        {
            // 적이 현재 위치에서 목표 Waypoint 까지 이동
            yield return StartCoroutine(MoveAToB(transform.position, _wayPoints[currentIndex].position));

            // 다음 Waypoint 설정
            if ( currentIndex < wayPointCount - 1 )
                currentIndex++;

            // 적이 Player 앞에 있는 마지막 Waypoint 에 도착하면 플레이어 공격
            else
            {
                playerHP.TakeDamage(attackDamage);
                yield return new WaitForSeconds(attackRate);
            }
        }
    }

    private IEnumerator MoveAToB(Vector3 start, Vector3 end)
    {
        float percent = 0f;
        float moveTime = Vector3.Distance(start, end) * timeOffset;

        while ( percent < 1f )
        {
            // 적이 Waypoint 까지 일정한 속도로 움직이도록 함
            percent += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }

    // 적의 사망 처리를 담당하는 EnemySpawner 에게 필요한 정보를 전달하는 메소드
    public void OnDie()
    {
        _enemySpawner.DestroyEnemy(this);
    }
}
