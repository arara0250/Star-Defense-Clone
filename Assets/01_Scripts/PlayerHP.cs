using System.ComponentModel;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;

    // 플레이어 체력 정보 외부 참조용 프로퍼티
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        // 플레이어 체력 초기 세팅 (최대 체력으로)
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        
        if (currentHP <= 0)
        {
            // TODO : 체력이 0 이하이면, 게임 오버 처리
        }
    }
}
