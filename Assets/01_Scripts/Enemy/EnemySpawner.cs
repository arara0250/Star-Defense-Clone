using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("프리팹 리소스")]
    [SerializeField] private GameObject     enemyHpBarPrefab;

    [Header("적 스폰 세팅")]
    [SerializeField] private float          spawnDuration;      // 적 스폰 주기
    [SerializeField] private Transform      hpBarParent;        // 적 체력바 UI 오브젝트들의 부모
    [SerializeField] private Transform[]    wayPoints;          // Waypoint 오브젝트 Inspector 뷰에서 추가

    private PlayerGold  playerGold;                 // 플레이어 골드 정보 참조
    private Wave        currentWave;                // 현재 진행중인 웨이브
    private bool        isSpawnFinished = true;     // 현재 진행중인 웨이브의 적(일반+보스)이 모두 스폰되었는지
                                                    // 초기값 : true => 스폰 대기 상태

    private List<Enemy> enemyList;                  // 현재 맵에 활성화된 적 정보를 관리하기 위한 리스트

    // 외부 참조용 프로퍼티
    public List<Enemy>  EnemyList => enemyList;
    public bool         IsSpawnFinished => isSpawnFinished;

    // 적이 제거되었음을 WaveSystem 에 알리기 위한 이벤트
    public event Action OnEnemyDestroyed;


    private void Awake()
    {
        enemyList = new List<Enemy>();
        playerGold = GameManager.Instance.PlayerGold;
    }

    public void StartWave(Wave wave)
    {
        // 전달받은 웨이브 정보 캐싱
        currentWave = wave;
        isSpawnFinished = false;

        // 현재 웨이브 시작
        StartCoroutine(nameof(SpawnEnemy));
    }

    private IEnumerator SpawnEnemy()
    {
        float elapsedTime = 0f;     // 현재 웨이브가 시작되고 소요된 시간

        // 현재 웨이브에 설정된 시간(waveTime) 동안에만 일반 적 소환
        while ( elapsedTime < currentWave.waveTime )
        {
            SpawnNormalMonster();

            yield return new WaitForSeconds(spawnDuration);     // 스폰 주기만큼 대기 후, 다음 적 생성
            elapsedTime += spawnDuration;
        }

        SpawnBossMonster();
        isSpawnFinished = true;     // 현재 웨이브의 적 생성 완료
    }

    private void SpawnNormalMonster()
    {
        // 현재 웨이브에서 등장하는 적이 여러 종류라면, 랜덤으로 등장
        int         enemyIndex          = UnityEngine.Random.Range(0, currentWave.enemyPrefabs.Length);
        GameObject  normalMonsterPrefab = currentWave.enemyPrefabs[enemyIndex];

        SpawnMonsterObject(normalMonsterPrefab);
    }

    private void SpawnBossMonster()
    {
        SpawnMonsterObject(currentWave.bossPrefab);
    }

    private void SpawnMonsterObject(GameObject monsterPrefab)
    {
        GameObject  clone = Instantiate(monsterPrefab);
        Enemy       enemy = clone.GetComponent<Enemy>();

        enemy.Setup(this, wayPoints);   // Inspector 뷰로 추가한 Waypoint 정보를 전달
        enemyList.Add(enemy);           // 직전에 생성된 적의 정보를 리스트에 저장

        SpawnEnemyHpBar(clone);
    }

    // EnemySpawner 에서 enemyList 를 관리하기 때문에, 적의 사망 처리도 EnemySpawner 에서 담당
    public void DestroyEnemy(Enemy enemy, int goldReward)
    {
        playerGold.CurrentGold += goldReward;   // 적 처치 골드 획득

        enemyList.Remove(enemy);        // 리스트에서 삭제
        Destroy(enemy.gameObject);      // 오브젝트도 삭제

        OnEnemyDestroyed?.Invoke();     // 적이 사망할 때마다 WaveSystem 에게 알림 (다음 웨이브 시작할지 판단)
    }

    // 적 체력바 UI 오브젝트 생성 및 설정
    private void SpawnEnemyHpBar(GameObject enemy)
    {
        var hpBar = Instantiate(enemyHpBarPrefab);

        // 체력바 UI 오브젝트의 부모 설정 및 스케일 초기화
        hpBar.transform.SetParent(hpBarParent);
        hpBar.transform.localScale = Vector3.one;

        // 체력바가 따라갈 위치를 Enemy 의 HudPivot 으로 설정
        hpBar.GetComponent<UI_FollowTarget>().SetTarget(enemy.GetComponent<Enemy>().HpBarPivot);

        // 적 체력 정보 컴포넌트 전달
        hpBar.GetComponent<UI_EnemyHP>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
