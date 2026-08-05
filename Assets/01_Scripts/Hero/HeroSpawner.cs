using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    [Header("영웅 소환 세팅")]
    [SerializeField] private GameObject[]   heroPrefabs;
    [SerializeField] private int            heroSpawnGold;      // 영웅 소환 비용
    [SerializeField] private EnemySpawner   enemySpawner;       // Inspector 뷰에서 직접 연결 (영웅에게 적 정보 전달하기 위함)

    [Header("팝업 UI 세팅")]
    [SerializeField] private GameObject     actionPopupPrefab;  // "소환(20G)" / "승급" 공용 팝업 프리팹
    [SerializeField] private Transform      popupParent;        // 팝업 UI 들의 부모 (Canvas 하위)

    private PlayerGold      playerGold;
    private List<Hero>      heroList;
    private GameObject      currentPopup;  // 현재 열려있는 팝업 (동시에 하나만 유지)

    private void Awake()
    {
        heroList = new List<Hero>();
        playerGold = GameManager.Instance.PlayerGold;
    }


    // 영웅 소환 가능 블록을 클릭(터치) 시, 진입점
    public void TrySpawnHero(Transform blockTransform)
    {
        Block block = blockTransform.GetComponent<Block>();

        // 빈 블록 클릭 => "소환" 버튼 팝업
        if ( block.Hero == null )
        {
            ShowPopup(block.PopupPivot, $"소환({ heroSpawnGold }G)", () => SpawnHero(block));
        }

        // 이미 영웅이 소환된 블록 클릭 => "승급" 버튼 팝업
        else
        {
            ShowPopup(block.Hero.PopupPivot, "승급", () => TryMergeHero(block.Hero));
        }
    }

    // targetPivot 위치에 "text" 문구를 가진 팝업을 띄우고, 클릭 시 onConfirm 을 실행하는 메소드
    private void ShowPopup(Transform targetPivot, string text, Action onConfirm)
    {
        // 중복 팝업 방지
        if (currentPopup != null)
            Destroy(currentPopup);

        currentPopup = Instantiate(actionPopupPrefab, popupParent);
        currentPopup.GetComponent<UI_FollowTarget>().SetTarget(targetPivot);
        currentPopup.GetComponent<UI_PopupButton>().Setup(text, onConfirm);
    }

    // 영웅 신규 소환 메소드
    private void SpawnHero(Block block)
    {
        // 영웅 소환 가능 여부 검사 (골드 부족)
        if ( heroSpawnGold > playerGold.CurrentGold )
            return;

        SpawnHeroAtLevel(block, level: 1);   // 레벨 1 영웅 소환

        playerGold.SpendGold(heroSpawnGold);
    }

    // level 에 해당하는 프리팹으로 block 위치에 영웅을 소환 메소드 (신규 소환 / 승급 공용)
    private Hero SpawnHeroAtLevel(Block block, int level)
    {
        GameObject prefab = heroPrefabs[level - 1];   // 레벨 1 -> index 0, 레벨 2 -> index 1 ...

        var clone = Instantiate(prefab, block.transform.position, Quaternion.identity);
        Hero hero = clone.GetComponent<Hero>();

        hero.Setup(block, level);                            // 영웅에게 배치된 블록 및 레벨 정보 전달
        clone.GetComponent<HeroAttack>().Setup(enemySpawner);

        heroList.Add(hero);
        block.Hero = hero;

        return hero;
    }

    // 같은 레벨 영웅 x 2 => 승급
    private void TryMergeHero(Hero clickedHero)
    {
        // 클릭한 영웅과 "같은 레벨"이면서, "클릭한 영웅 자신은 아닌" 다른 영웅을 탐색
        Hero targetHero = heroList.Find(hero => hero != clickedHero && hero.Level == clickedHero.Level);

        // 합칠 대상이 없으면 아무 것도 하지 않음
        if (targetHero == null)
            return;

        int nextLevel = clickedHero.Level + 1;

        // 이미 최고 레벨(등록된 프리팹 개수를 초과)이라면 더 이상 합체하지 않음
        if (nextLevel > heroPrefabs.Length)
            return;

        Block mergeBlock = clickedHero.Block;   // 클릭한 영웅이 있던 블록 자리에 승급된 영웅을 새로 소환

        // 기존 두 영웅 제거 (각자 있던 블록도 비움)
        heroList.Remove(clickedHero);
        heroList.Remove(targetHero);

        clickedHero.Block.Hero = null;
        targetHero.Block.Hero = null;

        Destroy(clickedHero.gameObject);
        Destroy(targetHero.gameObject);

        // 승급된 레벨의 프리팹으로 새로 소환 (골드 소모 없음)
        SpawnHeroAtLevel(mergeBlock, nextLevel);
    }
}
