using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float timeOffset = 1.0f;   // 1칸 이동하는데 걸리는 시간

    private Transform[] _wayPoints;
    private int         wayPointCount;
    private int         currentIndex = 0;   // 현재 이동중인 웨이포인트의 인덱스

    private EnemySpawner    _enemySpawner;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        _enemySpawner = enemySpawner;
        
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
            else
                OnDie();
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
