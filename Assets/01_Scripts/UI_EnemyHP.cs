using UnityEngine;
using UnityEngine.UI;

// 적의 체력을 UI 에 그려주는 컴포넌트
public class UI_EnemyHP : MonoBehaviour
{
    private EnemyHP _enemyHP;
    private Slider  hpBar;

    public void Setup(EnemyHP enemyHP)
    {
        hpBar      = GetComponent<Slider>();
        _enemyHP   = enemyHP;
    }

    private void Update()
    {
        hpBar.value = _enemyHP.CurrentHP / _enemyHP.MaxHP;     // 0.0f ~ 1.0f
    }
}
