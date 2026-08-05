using UnityEngine;
using UnityEngine.SceneManagement;

// 전역 참조용 싱글톤 GameManager (간단ver)
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("참조 연결")]
    [SerializeField] private PlayerHP playerHP;
    [SerializeField] private PlayerGold playerGold;
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject victoryText;

    // 외부 참조용 프로퍼티
    public PlayerHP     PlayerHP => playerHP;
    public PlayerGold   PlayerGold => playerGold;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        playerHP.OnDeath                += HandleGameOver;
        waveSystem.OnAllWavesCleared    += HandleVictory;
    }

    private void OnDisable()
    {
        playerHP.OnDeath                -= HandleGameOver;
        waveSystem.OnAllWavesCleared    -= HandleVictory;
    }

    private void Start()
    {
        // 게임 시작 -> 첫 번째 웨이브 시작
        waveSystem.TryStartNextWave();
    }

    // 지휘관(Player) 체력이 0이 되었을 때 호출
    private void HandleGameOver()
    {
        Debug.Log("Game Over");

        retryButton.SetActive(true);

        Time.timeScale = 0f;   // 게임 진행(적 스폰, 이동, 공격 등) 을 전부 일시정지
    }

    // 마지막 웨이브까지 전부 클리어했을 때 호출
    private void HandleVictory()
    {
        Debug.Log("Victory");

        retryButton.SetActive(true);
        victoryText.SetActive(true);

        Time.timeScale = 0f;   // 게임 진행 전부 일시정지
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("GameScene");
    }
}
