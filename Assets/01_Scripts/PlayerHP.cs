using System;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;

    private bool isDie = false;     // 게임 오버 중복 방지 플래그

    // 이벤트 변수
    public event Action<float, float>   OnHPChanged;    // 체력바 UI 갱신용
    public event Action                 OnDeath;        // 게임오버 처리용

    // 플레이어 체력 정보 외부 참조용 프로퍼티
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        // 플레이어 체력 초기 세팅 (최대 체력으로)
        currentHP = maxHP;
    }

    private void Start()
    {
        // 시작 시점의 플레이어 체력 상태를 한 번 업데이트
        OnHPChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(float damage)
    {
        // 플레이어가 이미 사망한 경우를 위한 예외 처리
        if (isDie)
            return;

        currentHP = MathF.Max(0, currentHP - damage);
        OnHPChanged?.Invoke(currentHP, maxHP);
        
        if (currentHP <= 0)
        {
            // TODO : 체력이 0 이하이면, 게임 오버 처리
            isDie = true;
            OnDeath?.Invoke();
        }
    }
}
