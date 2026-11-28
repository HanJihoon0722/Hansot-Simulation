# 코드 구조 (Architecture)

> 상태: v0.2. 치킨마요 PoC 코드에 맞춰 고쳤다.
> 새 인터페이스나 SO 종류를 추가할 때는 이 문서를 먼저 고치고 사용자에게 알린다.

---

## 1. 원칙

- 지금 필요한 추상화만 만든다. 치킨마요 한 그릇에 필요 없는 구조는 넣지 않는다.
- 데이터(SO)와 동작(MonoBehaviour)을 나눈다. 새 재료와 새 메뉴는 코드 수정 없이 SO 추가로 늘어나는 것이 목표다.
- 수치는 SO나 인스펙터 값으로 뺀다.
- 입력은 Input System 액션으로만 받는다. 한 액션에는 한 종류의 행동만 맡긴다.
- 싱글톤과 정적(static) 상태를 쓰지 않는다. 다른 오브젝트가 필요하면 인스펙터 참조로 연결한다. 정적 메서드는 상태가 없으면 써도 된다.
  - 이유. 플레이 진입 시 도메인 리로드를 꺼 두었다(아래 8절). 이 설정에서는 정적 필드가 플레이를 껐다 켜도 초기화되지 않아 이전 플레이의 값이 남는다.

---

## 2. 네임스페이스와 폴더

| 네임스페이스 | 폴더 | 내용 |
|---|---|---|
| `Dosirak.Interaction` | `Assets/01. Scripts/Interaction/` | 상호작용 인터페이스 |
| `Dosirak.Data` | `Assets/01. Scripts/Data/` | SO 정의 클래스 |
| `Dosirak.Cooking` | `Assets/01. Scripts/Cooking/` | 물건(Item), 기구(튀김기, 슬라이서, 디스펜서), 용기(보울, 봉투) |
| `Dosirak.Orders` | `Assets/01. Scripts/Orders/` | 손님, 만족도 평가 |
| `Dosirak.Player` | `Assets/01. Scripts/Player/` | 1인칭 이동, 시선 레이캐스트, 손에 들기 |
| `Dosirak.PoC` | `Assets/01. Scripts/PoC/` | PoC 전용 임시 코드 (IMGUI HUD) |
| `Dosirak.EditorTools` | `Assets/01. Scripts/Editor/` | 데이터, 프리팹, 씬 생성 메뉴 |

- SO 에셋은 `Assets/02. Data/` 아래에 둔다. PoC 데이터는 `Assets/02. Data/PoC/`.
- 입력 액션 에셋은 `Assets/02. Data/Input/PlayerControls.inputactions`.

---

## 3. 인터페이스

| 이름 | 역할 | 구현 |
|---|---|---|
| `IGrabbable` | 빈손으로 집었을 때 손에 들 물건을 내준다. 줄 수 없으면 null | `Item`(자기 자신), `Dispenser`(새로 꺼냄), `Fryer`(바구니에서 꺼냄), `Slicer`(완료된 것을 꺼냄) |
| `IItemReceiver` | 손에 든 물건을 받을 수 있는지 판정하고 받는다 | `Fryer`, `Slicer`, `BowlContainer`, `BagContainer`, `Customer` |
| `IInteractable` | 사용 키로 조작한다. 프롬프트 문구(`UseLabel`)를 가진다 | `Fryer`(바구니 내리기와 올리기) |

- 뼈대에 있던 `IHoldable`은 `IGrabbable`로 바꿨다. 손에 드는 물건은 `Item` 하나뿐이라 따로 인터페이스가 필요 없고, 필요한 것은 "집으면 무엇을 주는가"였다.
- "밥은 반찬 칸에 들어가지 않는다" 같은 규칙은 `IItemReceiver.CanReceive`에서 처리한다.

---

## 4. SO 데이터

| 이름 | 담는 값 |
|---|---|
| `ItemData` | 이름, 메시, 크기, 색, 온도 사용 여부, 조리 직후 뜨거운지, 온도 유지 시간과 식는 속도, 조리 프로필, 슬라이스 결과 |
| `CookProfileData` | 적당함 시작 시간, 탐 시작 시간, 익힘에 따른 색 |
| `MenuData` | 메뉴 이름, 담는 순서, 포장 구성(물건과 개수) |

- 뼈대의 `IngredientData`는 `ItemData`로 바꿨다. 수저, 물티슈, 뚜껑, 용기도 같은 구조로 다루기 때문이다.
- 냉장 보관 여부, 사각 도시락 칸 종류는 해당 기능을 만들 때 추가한다.

---

## 5. 런타임 상태

- `Item`이 익힘 시간(`CookSeconds`)과 온도(`Temperature`, 0~1)를 가진다.
- 슬라이스처럼 종류가 바뀌어도(`ChangeData`) 익힘과 온도는 유지된다.
- 온도는 조리 직후 1이고, `holdSeconds` 동안 유지된 뒤 `coolPerSecond`씩 떨어진다.
- 만족도는 `Customer`가 봉투 안의 완성 메뉴를 보고 계산한다. 익힘, 온도, 포장 정확도에 가중치를 곱해 별 1~5개로 바꾼다.

---

## 6. 입력 액션 [PoC]

| 액션 | 현재 키 | 행동 |
|---|---|---|
| Move | WASD | 이동 |
| Look | 마우스 이동 | 시선 |
| Grab | 마우스 왼쪽 | 손 동작. 집기, 넣기, 내려놓기 |
| Use | E | 기구 조작. 튀김기 바구니 내리기와 올리기 |
| ToggleCursor | Tab | 마우스 커서 잠금과 해제 |

- 키 배치는 PoC를 직접 해 보며 바꾼다.
- 화면 안내 문구의 키 이름은 액션 바인딩에서 읽어 온다. 키를 바꾸면 안내도 같이 바뀐다.

---

## 7. PoC 씬 재생성

- 메뉴 `Dosirak > PoC > Build Chicken Mayo Scene`을 실행하면 데이터, 프리팹, 씬(`Assets/04. Scenes/PoC/PoC_ChickenMayo.unity`)을 다시 만든다.
- 씬을 손으로 고쳤다면 재생성 시 덮어쓰이니 주의한다.

---

## 8. 에디터 설정

- Enter Play Mode Settings에서 도메인 리로드를 껐다 (`ProjectSettings/EditorSettings.asset`). 플레이 버튼을 누르면 리로드 없이 바로 시작한다.
- 씬 리로드는 켜 둔다. 플레이를 끄면 씬이 원래대로 돌아온다.
- 스크립트를 고친 뒤의 컴파일과 리로드는 그대로 일어난다. 이건 끌 수 없다.
