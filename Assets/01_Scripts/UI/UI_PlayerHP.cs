using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider     slider;
    [SerializeField] private PlayerHP   playerHP;       // Player 오브젝트
    [SerializeField] private Transform  hpBarPivot;     // Player 오브젝트 하위의 HpBarPivot 오브젝트

    private void Awake()
    {
        GetComponent<UI_FollowTarget>().SetTarget(hpBarPivot);
    }

    private void OnEnable()
    {
        playerHP.OnHPChanged += UpdateHPBar;
    }

    private void OnDisable()
    {
        playerHP.OnHPChanged -= UpdateHPBar;
    }

    private void UpdateHPBar(float currentHP, float maxHP)
    {
        slider.value = currentHP / maxHP;   // 0.0f ~ 1.0f
    }
}
