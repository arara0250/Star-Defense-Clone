using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private GameObject heroPrefab;

    public void TrySpawnHero(Transform blockTransform)
    {
        // 블록의 영웅 소환 여부 검사 (중복 방지)
        Block block = blockTransform.GetComponent<Block>();
        if ( block.IsSpawnHero )
            return;

        // 선택한 위치의 블록에 영웅 소환
        Instantiate(heroPrefab, blockTransform.position, Quaternion.identity);
        block.IsSpawnHero = true;
    }
}
