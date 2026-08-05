using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField] private int currentGold = 150;  // 플레이어의 현재 골드, 최초 골드 150

    // 외부 참조용 프로퍼티
    public int CurrentGold
    {
        get => currentGold;
        set => currentGold = Mathf.Max(0, value);   // 음수가 되지 않도록 처리
    }
}
