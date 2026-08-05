# 프로젝트 소개
- 스파르타 게임 캠프 10기 과제 전형 **(스타 디펜스: 유즈맵 TD 모작)** 을 위한 프로젝트 입니다.

---
# 시연 영상

---
# 과제 구현 사항
#### 1. [필수] 전투 루프: 적 생성 및 이동, 영웅 공격, 적 체력 감소, 적 사망 처리 (구현 완료 ✅)
- **적 생성 및 웨이포인트를 따라 이동**
  > [Enemy.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Enemy/Enemy.cs)
<br> [EnemySpawner.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Enemy/EnemySpawner.cs)

- **영웅(타워) 의 적 공격**
  > [HeroAttack.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Hero/HeroAttack.cs)

- **적 체력 감소**
  > [EnemyHP.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Enemy/EnemyHP.cs)

- **적 사망 처리 (적 정보를 관리하는 리스트 `enemyList` 와의 통신을 위해 `EnemySpawner` 에서 처리)**
  > [EnemySpawner.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Enemy/EnemySpawner.cs)

---
#### 2. [필수] 웨이브: 승리/실패 판정 (지휘관 체력 기반) (구현 완료 ✅)
- **웨이브 시스템**
  - 각 웨이브마다 몬스터를 스폰하는 시간이 정해져 있음 (`waveTime`)
  - 각 웨이브마다 스폰하는 몬스터의 종류와 보스가 정해져 있음 (`enemyPrefeabs`, `bossPrefab`)
  - 웨이브가 시작되면, `waveTime` 동안 `enemyPrefabs` 의 일반 몬스터를 스폰하다가, `waveTime` 이 지나면 `bossPrefab` 의 보스 몬스터를 1마리 스폰함
  - `enemyPrefabs` 의 일반 몬스터 프리팹이 여러 개라면, 적을 스폰할 때마다 프리팹을 랜덤하게 선택함
  - 맵에 스폰된 모든 적 (보스 포함) 을 처치하면, 다음 웨이브로 진행됨
  - 길의 끝에 위치한 플레이어(지휘관) 의 체력을 기반으로 게임오버 / 승리 처리

    > [WaveSystem.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/WaveSystem.cs)
    <br> [PlayerHP.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Player/PlayerHP.cs)
    <br> [GameManager.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/GameManager.cs)

---
#### 3. [필수] 영웅 시스템: 소환 및 승급 (재화소모 포함) (구현 완료 ✅)
- **영웅 소환 및 승급 시스템**
  1. 빈 블록(타일) 에 새로운 Lv1 영웅 소환 (골드 소모)
  2. Lv1 영웅이 2개 이상 소환되어 있으면, 두 개의 Lv1 영웅을 하나의 Lv2 영웅으로 합치면서 승급 (골드 소모 X)
     
        |  Lv1 영웅  |  Lv2 영웅  |
        | ------------- | ------------- |
        | <img width="160" height="178" alt="스크린샷 2026-08-06 074036" src="https://github.com/user-attachments/assets/4c9db66c-9a43-4339-8723-90974e8625cc" /> | <img width="160" height="166" alt="image" src="https://github.com/user-attachments/assets/e8aa0266-2485-464e-8373-8fc4b03fa489" /> |

     > [Hero.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Hero/Hero.cs)
     <br> [HeroSpawner.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Hero/HeroSpawner.cs)
     <br> [Block.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Block.cs)
     <br> [PlayerGold.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/Player/PlayerGold.cs)
     <br> [UI_PopupButton.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/UI/UI_PopupButton.cs)

---
#### 4. [선택] 영웅 초월, 현상금, 탐사정, 강화, 수리, 타일 버프 (구현 못 함 ❌)

---
#### 5. 그 외
- **주요 UI 구현 (이벤트 방식 기반)**
  
  > [UI_WavePanel.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/UI/UI_WavePanel.cs)
  <br> [UI_GoldPanel.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/UI/UI_GoldPanel.cs)
  <br> [UI_EnemyHP.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/UI/UI_EnemyHP.cs)
  <br> [UI_PlayerHP.cs](https://github.com/arara0250/Star-Defense-Clone/blob/main/Assets/01_Scripts/UI/UI_PlayerHP.cs)
