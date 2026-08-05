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

    private int     currentWaveIndex = -1;
    private bool    isAllWavesCleared;      // OnAllWavesCleared 중복 방지 플래그

    // 외부 참조용 프로퍼티
    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;

    // 다음 웨이브로 진행되었음을 UI_WavePanel 에 알리기 위한 이벤트
    public event Action OnWaveChanged;

    // 마지막 웨이브 클리어 시, 승리 처리용 이벤트
    public event Action OnAllWavesCleared;

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
        // 모든 웨이브 클리어 => 웨이브 진행 X
        if ( isAllWavesCleared )
            return;

        // 아직 이번 웨이브의 적 스폰이 끝나지 않음 => 웨이브 진행 X
        if ( !enemySpawner.IsSpawnFinished )
            return;

        // 아직 스폰된 적이 남아있음 => 웨이브 진행 X
        if (enemySpawner.EnemyList.Count > 0)
            return;

        // 아직 웨이브가 남아있다면 => 다음 웨이브로 진행
        if ( currentWaveIndex < waves.Length - 1 )
        {
            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);    // 현재 웨이브 정보를 매개변수로 전달

            OnWaveChanged?.Invoke();
        }

        // 마지막 웨이브까지 클리어
        else
        {
            isAllWavesCleared = true;
            OnAllWavesCleared?.Invoke();
        }
    }
}
