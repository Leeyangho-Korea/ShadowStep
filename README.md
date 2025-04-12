# ShadowStep

Unity로 개발 중인 쿼터뷰 액션 로그라이크 게임 프로젝트입니다.  
게임 시스템 및 구조적인 Unity 컴포넌트 설계를 연습하기 위한 개인 학습용 프로젝트입니다.

---

## 현재까지 구현된 기능

### 플레이어 이동
- 쿼터뷰 시점에서 WASD 입력을 통한 이동
- 이동 방향에 따라 캐릭터 회전 처리
- 단순 이동 애니메이션 연동 (Blend Tree는 사용하지 않음)

### 플레이어 전투 시스템
- 마우스 좌클릭을 통한 근접 공격 구현
- 공격 애니메이션은 상체 전용 Animator 레이어에서 재생
- 공격 상태는 PlayerState 컴포넌트에서 관리

### 애니메이션 레이어 시스템
- 공격, 피격, 디지(dizzy) 상태 등은 별도의 애니메이션 레이어로 처리
- 애니메이션 실행 시 레이어 weight를 조정해 자연스럽게 재생 및 종료
- 동일 애니메이션을 다시 처음부터 재생하는 구조 구현
- PlayerAnimation 스크립트에서 모든 애니메이션 로직 일원화

### 몬스터 피격 및 사망 처리
- 간단한 몬스터 프리팹과 충돌 판정 구성
- 플레이어 공격 시 데미지를 받고 피격 애니메이션 재생
- 체력이 0이 되면 사망 처리 및 오브젝트 제거

### 몬스터 AI
- 플레이어와의 거리 기반 상태 전이 (Idle, Chase, Attack, Return)
- NavMeshAgent를 사용한 추적 이동 및 원위치 복귀
- 일정 거리 이내 접근 시 자동 공격 (공격 간격 쿨타임 적용)
- 공격 시 PlayerState.TakeDamage()를 통해 피해 적용
- 사망 시 Death 애니메이션 재생 후 3초 후 비활성화 처리 (오브젝트 풀링 대비)
- 플레이어는 "Player" 태그를 통해 자동으로 감지 및 추적 대상 설정

### 컴포넌트 기반 구조 설계
- PlayerController: 이동 및 입력 처리
- PlayerAttack: 공격 판정 처리
- PlayerState: 체력, 상태 관리
- PlayerAnimation: Animator 제어 및 애니메이션 레이어 관리

---

## 향후 개발 계획
- 몬스터 풀링 생성 및 관리
- 체력 UI 구현
- 스킬 및 특수 공격 시스템
- 랜덤 던전 생성
- 이펙트 및 사운드 효과 추가

---

## 사용 기술

- Unity 2022.3.53f1
- C#
- Animator 레이어 제어 및 상태 전이
- GitHub를 통한 버전 관리

---

## 사용 에셋

- [Battle Wizard Poly Art](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/battle-wizard-poly-art-128097#content)
