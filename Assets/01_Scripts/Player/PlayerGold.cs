using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerGold : MonoBehaviour
{
    [SerializeField] private int currentGold = 150;  // 플레이어의 현재 골드, 최초 골드 150

    // 외부 참조용 프로퍼티
    public int CurrentGold => currentGold;

    // 플레이어 소지 골드가 변경되었음을 알리기 위한 이벤트
    public event Action OnGoldChanged;

    private void Start()
    {
        OnGoldChanged?.Invoke();
    }

    // 골드 획득
    public void EarnGold(int gold)
    {
        currentGold += gold;
        OnGoldChanged?.Invoke();
    }

    // 골드 소비
    public void SpendGold(int gold)
    {
        currentGold = Mathf.Max(0, currentGold - gold);
        OnGoldChanged?.Invoke();
    }
}
