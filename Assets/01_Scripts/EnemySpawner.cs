using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject     enemyPrefab;
    [SerializeField] private float          spawnTime;      // 적 스폰 주기
    [SerializeField] private Transform[]    wayPoints;      // Waypoint 오브젝트 Inspector 뷰에서 추가

    private void Awake()
    {
        StartCoroutine(nameof(SpawnEnemy));
    }

    private IEnumerator SpawnEnemy()
    {
        while ( true )
        {
            GameObject clone = Instantiate(enemyPrefab);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemy.Setup(wayPoints);     // Inspector 뷰로 추가한 Waypoint 정보를 전달
            
            yield return new WaitForSeconds(spawnTime);     // 스폰 주기만큼 대기 후, 다음 적 생성
        }
    }
}
