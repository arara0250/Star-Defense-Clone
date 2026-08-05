using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Transform popupPivot;      // "소환" 팝업 UI 가 표시될 위치

    // 외부 참조용 프로퍼티
    public Transform PopupPivot => popupPivot;

    // 현재 블록의 영웅 소환 여부를 확인하기 위함 (없으면 null)
    public Hero Hero { get; set; }
}
