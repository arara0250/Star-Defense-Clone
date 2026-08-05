using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform popupPivot;      // "승급" 팝업 UI 가 표시될 위치

    // 외부 참조용 프로퍼티
    public Transform PopupPivot => popupPivot;

    public int      Level { get; private set; } = 1;    // 소환 시 기본 레벨 1
    public Block    Block { get; private set; }         // 이 영웅이 배치된 블록 (승급 시 블록을 비워주기 위함)

    public void Setup(Block block, int level = 1)
    {
        Block = block;
        Level = level;
    }
}
