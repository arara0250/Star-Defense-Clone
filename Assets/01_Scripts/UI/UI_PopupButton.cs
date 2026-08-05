using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 블록or영웅 위에 표시되는 팝업형 버튼. ("소환" or "승급")
public class UI_PopupButton : MonoBehaviour
{
    [SerializeField] private Button             popupButton;
    [SerializeField] private TextMeshProUGUI    label;

    private Action OnConfirm;       // 버튼 클릭 시 수행할 동작 (외부로부터 전달)

    private void Awake()
    {
        popupButton.onClick.AddListener(HandleClick);
    }

    // 팝업 생성 직후 호출 (표시할 문구, 실행할 동작을 매개변수로 전달)
    public void Setup(string text, Action onConfirmAction)
    {
        label.text = text;
        OnConfirm = onConfirmAction;
    }

    private void HandleClick()
    {
        OnConfirm?.Invoke();
        Destroy(gameObject);    // 클릭되면 팝업 스스로 제거
    }
}
