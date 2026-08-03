using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;

    private bool            isDie = false;
    private Enemy           enemy;              // 적 사망 메소드 호출용
    private SpriteRenderer  spriteRenderer;     // 적 피격 효과 연출용

    private void Awake()
    {
        enemy           = GetComponent<Enemy>();
        spriteRenderer  = GetComponentInChildren<SpriteRenderer>();

        // 적 체력 초기 세팅 (최대 체력으로)
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        // 이미 사망한 적을 공격하였을 경우를 위한 예외 처리
        if (isDie)
            return;

        currentHP -= damage;

        // 중복 호출 방지
        StopCoroutine(nameof(FlashHitEffect));
        StartCoroutine(nameof(FlashHitEffect));

        // 체력이 0 이하이면, 적 사망 처리
        if ( currentHP <= 0 )
        {
            isDie = true;
            enemy.OnDie();
        }
    }

    // 적이 피격당했을 때, 빨간색으로 깜빡이는 효과 연출 메소드
    private IEnumerator FlashHitEffect()
    {
        // 기존 색상 저장
        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        // 다시 기존 색상으로
        spriteRenderer.color = originalColor;
    }
}
