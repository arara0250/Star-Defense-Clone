using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("프리팹 리소스")]
    [SerializeField] private GameObject     enemyPrefab;
    [SerializeField] private GameObject     enemyHpBarPrefab;

    [Header("적 스폰 세팅")]
    [SerializeField] private float          spawnTime;      // 적 스폰 주기
    [SerializeField] private Transform      hpBarParent;    // 적 체력바 UI 오브젝트들의 부모
    [SerializeField] private Transform[]    wayPoints;      // Waypoint 오브젝트 Inspector 뷰에서 추가


    private List<Enemy> enemyList;                  // 현재 맵에 활성화된 적 정보를 관리하기 위한 리스트
    public List<Enemy>  EnemyList => enemyList;     // 외부 참조용 프로퍼티


    private void Awake()
    {
        enemyList = new List<Enemy>();
        
        StartCoroutine(nameof(SpawnEnemy));
    }

    private IEnumerator SpawnEnemy()
    {
        while ( true )
        {
            GameObject clone = Instantiate(enemyPrefab);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemy.Setup(this, wayPoints);   // Inspector 뷰로 추가한 Waypoint 정보를 전달
            enemyList.Add(enemy);           // 직전에 생성된 적의 정보를 리스트에 저장

            SpawnEnemyHpBar(clone);
            
            yield return new WaitForSeconds(spawnTime);     // 스폰 주기만큼 대기 후, 다음 적 생성
        }
    }

    // EnemySpawner 에서 enemyList 를 관리하기 때문에, 적의 사망 처리도 EnemySpawner 에서 담당
    public void DestroyEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);        // 리스트에서 삭제
        Destroy(enemy.gameObject);      // 오브젝트도 삭제
    }

    // 적 체력바 UI 오브젝트 생성 및 설정
    private void SpawnEnemyHpBar(GameObject enemy)
    {
        var hpBar = Instantiate(enemyHpBarPrefab);

        // 체력바 UI 오브젝트의 부모 설정 및 스케일 초기화
        hpBar.transform.SetParent(hpBarParent);
        hpBar.transform.localScale = Vector3.one;

        // 체력바가 따라갈 위치를 Enemy 의 HudPivot 으로 설정
        hpBar.GetComponent<UI_FollowTarget>().SetTarget(enemy.GetComponent<Enemy>().HudPivot);

        // 적 체력 정보 컴포넌트 전달
        hpBar.GetComponent<UI_EnemyHP>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
