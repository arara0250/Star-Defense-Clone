using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private GameObject heroPrefab;

    public void TrySpawnHero(Transform blockTransform)
    {
        // 선택한 위치의 블럭에 영웅 소환
        Instantiate(heroPrefab, blockTransform.position, Quaternion.identity);
    }
}
