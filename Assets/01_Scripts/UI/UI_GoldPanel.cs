using TMPro;
using UnityEngine;

public class UI_GoldPanel : MonoBehaviour
{
    [SerializeField] private PlayerGold         playerGold;
    [SerializeField] private TextMeshProUGUI    goldText;

    private void OnEnable()
    {
        // 이벤트 구독
        playerGold.OnGoldChanged += UpdateGoldText;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        playerGold.OnGoldChanged -= UpdateGoldText;
    }

    private void UpdateGoldText()
    {
        goldText.text = $"{ playerGold.CurrentGold }";
    }
}
