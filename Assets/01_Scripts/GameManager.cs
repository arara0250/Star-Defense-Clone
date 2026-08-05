using UnityEngine;

// 전역 참조용 간단한 싱글톤 GameManager
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("참조 연결")]
    [SerializeField] private PlayerHP   playerHP;
    [SerializeField] private WaveSystem waveSystem;
    
    // 외부 참조용 프로퍼티
    public PlayerHP PlayerHP => playerHP;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 생성 방지를 위해 자기 자신을 파괴
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 게임 시작 -> 첫 번째 웨이브 시작
        waveSystem.TryStartNextWave();
    }
}
