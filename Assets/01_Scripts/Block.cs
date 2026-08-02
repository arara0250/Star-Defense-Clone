using UnityEngine;

public class Block : MonoBehaviour
{
    // 현재 블록의 영웅 소환 여부를 확인하는 프로퍼티
    public bool IsSpawnHero { set; get; }

    private void Awake()
    {
        IsSpawnHero = false;    // 초기 세팅 = 소환 X
    }
}
