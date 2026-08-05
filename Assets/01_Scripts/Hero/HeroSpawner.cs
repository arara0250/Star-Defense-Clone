using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private GameObject     heroPrefab;
    [SerializeField] private EnemySpawner   enemySpawner;     // Inspector 뷰에서 직접 연결 (영웅에게 적 정보 전달하기 위함)

    public void TrySpawnHero(Transform blockTransform)
    {
        // 블록의 영웅 소환 여부 검사 (중복 방지)
        Block block = blockTransform.GetComponent<Block>();
        if ( block.IsSpawnHero )
            return;

        // 선택한 위치의 블록에 영웅 소환
        var clone = Instantiate(heroPrefab, blockTransform.position, Quaternion.identity);
        clone.GetComponent<HeroAttack>().Setup(enemySpawner);
        block.IsSpawnHero = true;
    }
}
