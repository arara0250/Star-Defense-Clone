using System;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public float        waveTime;       // 현재 웨이브에서 일반 적을 생성하는 시간(초)
    public GameObject[] enemyPrefabs;   // 현재 웨이브에서 등장하는 적의 종류
    public GameObject   bossPrefab;     // 현재 웨이브의 마지막에 등장할 보스 프리팹
}

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Wave[]         waves;          // 현재 스테이지의 모든 웨이브
    [SerializeField] private EnemySpawner   enemySpawner;   // 적 스폰 메소드 호출을 위해 필요

    private int currentWaveIndex = -1;

    // 외부 참조용 프로퍼티
    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;

    // 다음 웨이브로 진행되었음을 UI_WavePanel 에 알리기 위한 이벤트
    public event Action OnWaveChanged;

    private void OnEnable()
    {
        // 이벤트 구독
        enemySpawner.OnEnemyDestroyed += TryStartNextWave;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        enemySpawner.OnEnemyDestroyed -= TryStartNextWave;
    }

    public void TryStartNextWave()
    {
        // 현재 웨이브 적(일반+보스) 스폰이 완료되었고 && 모든 적을 처치하였고 && 웨이브가 남아있으면 다음 웨이브 시작
        if ( enemySpawner.IsSpawnFinished 
          && enemySpawner.EnemyList.Count == 0 
          && currentWaveIndex < waves.Length - 1 )
        {
            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);    // 현재 웨이브 정보를 매개변수로 전달

            OnWaveChanged?.Invoke();
        }
    }
}
