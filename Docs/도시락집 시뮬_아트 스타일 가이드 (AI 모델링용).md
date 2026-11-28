# 도시락집 시뮬 — 아트 스타일 가이드 (AI 모델링용)

Nov 28, 2026 · @한지훈

> 상태: **초안 v0.1.** `[초안]` 표시가 붙은 수치는 레퍼런스에서 추정한 시작값입니다. 기준 소품 테스트(`References/Tests/`) 후 확정하고 표시를 지웁니다.

## 개요

스타일 한 줄 정의: 밝고 따뜻한 한국 동네 도시락집을, 재질감은 사실적이고 형태와 디테일은 깔끔하게 단순화한 **세미 리얼 PBR**로 표현한다. 음식이 화면의 주인공이다.

- 목적: 모든 3D 에셋(직접 모델링, AI 생성, 외부 구매)을 같은 톤으로 맞추기 위한 기준 문서. 새 에셋을 만들기 전에 읽고, 완성 후 "검수 절차"로 확인한다.
- 대상: Unity 6000.5.6f1 / URP 17.5 / Steam PC / 1인칭 (눈높이 약 1.65m)
- 원칙: 기준에 안 맞는 에셋은 게임에 넣지 않는다. 수정해서 검수를 다시 통과하거나 버린다. "나중에 맞추기"로 들여오지 않는다.

## 레퍼런스

분위기와 조명은 **오니기리 시뮬레이터**, 음식을 돋보이게 하는 방식은 **라멘 시뮬레이터**, 공간 구성과 소품과 메뉴는 **실제 한국 도시락집**에서 가져온다.

```
References/
├─ Games/OnigiriSim/   # 조명, 색감, 깨끗한 실내 톤 (주 레퍼런스)
├─ Games/RamenSim/     # 음식 표현, 소품 밀도
├─ Real/Store/         # 실매장 공간 구성, 카운터, 좌석, 재질
├─ Real/Food/          # 실제 메뉴, 용기, 음식 색
├─ Real/Equipment/     # 주방 기구 형태
└─ Tests/              # 테스트 모델 렌더, 승인본은 Tests/Approved/
```

| 레퍼런스 | 가져올 점 | 버릴 점 | 대표 이미지 경로 |
| --- | --- | --- | --- |
| 오니기리 시뮬레이터 | 흰 벽과 형광등의 밝은 실내, 화면 전체의 옅은 웜톤 그레이딩, 깨끗한 스테인리스 | 일본식 간판과 거리 디자인, 과하게 노란 색보정 | `Games/OnigiriSim/onigiri_backkitchen_01.webp`, `onigiri_preptable_01.webp` |
| 라멘 시뮬레이터 | 음식만 채도와 디테일이 높아 바로 눈에 띄는 구성, 바트(스테인리스 통)에 담긴 재료 배치 | 어둡고 강한 대비, 녹과 기름때, 일본 옛 거리 분위기 | `Games/RamenSim/ramen_01.webp` |
| 실제 한국 도시락집 | 원목 + 흰 카운터 + 검정 프레임 조합, 메뉴판 벽, 픽업대, 칸막이 검정 도시락 용기, 실제 반찬 구성 | 실제 브랜드 로고와 상표, 매장 안내문 문구 | `Real/Store/store_counter_02.webp`, `Real/Food/food_lunchbox_01.webp` |

파일 이름 규칙: `{출처}_{대상}_{번호}.{확장자}` (예: `onigiri_counter_01.webp`, `food_katsudon_01.webp`). 테스트 렌더는 `test_{에셋명}_v{버전}.png`.

## 7가지 통일 규칙

모든 에셋은 아래 7가지를 지킨다. 하나라도 어기면 검수에서 떨어진다.

### 1. 팔레트

메인 색은 환경 6개 + 브랜드 1개 + 음식 7개다. 환경 색은 명도 ±10% 안에서만 벗어날 수 있고, 음식 색은 실제 음식 색을 따르되 채도를 낮추지 않는다. HEX는 Base Color(알베도) 기준이다. `[초안]`

| 이름 | HEX | 용도 |
| --- | --- | --- |
| 크림 화이트 | #EFE8D8 | 벽, 카운터 상판, 천장 |
| 밝은 원목 | #C8955E | 벽 패널, 테이블, 스툴, 메뉴판 프레임 |
| 짙은 원목 | #6B4A33 | 몰딩, 가구 다리, 문틀 |
| 스테인리스 | #B8BCBF | 주방 작업대, 바트, 밥솥, 주방 벽 |
| 무광 블랙 | #1C1B1A | 도시락 용기, 그릇, 쟁반, 테이블 프레임 |
| 웜 그레이 타일 | #6E6862 | 바닥 타일 |
| **브랜드 옐로** | #F2C230 | 간판, 앞치마, 포장 봉투, 안내판, UI 포인트 (화면당 작은 면적만) |
| 밥 화이트 | #F2EEE4 | 흰밥 (현미밥은 #CDB894) |
| 튀김 골드 | #C98A3E | 돈가스, 치킨, 고로케 |
| 볶음 오렌지 | #E08A3C | 김치볶음밥, 제육 양념 |
| 김치 레드 | #C9472A | 김치, 고추장 양념, 국물 |
| 노른자 옐로 | #F2B233 | 계란, 옥수수 |
| 채소 그린 | #5E8C3A | 나물, 오이, 파 |
| 고기 브라운 | #7C4B27 | 함박, 불고기, 구운 고기 |

금지 색: 순백 #FFFFFF와 순흑 #000000 (실제 재질에 없음), 형광색, 차가운 파랑과 보라 계열의 큰 면적 (음식 색을 죽임). 파랑은 생수병 같은 작은 소품에만 쓴다.

팔레트 파일: `Docs/palette.png` (기준 소품 확정 후 만든다)

### 2. 조명

오니기리 시뮬처럼 밝고 그림자가 부드러운 실내가 기준이다. `[초안]`

- 메인 라이트: 천장 LED 패널과 다운라이트(Spot/Area). 색온도 4000K, 실내 바닥 기준 약 500 lux. 홀 좌석의 펜던트 조명만 3000K로 따뜻하게.
- 앰비언트: 베이크된 GI + Adaptive Probe Volumes. 앰비언트 색은 크림 화이트 쪽으로 살짝 따뜻하게.
- 포인트 라이트: 메뉴판 백라이트(4000K), 쇼케이스 냉장고 내부(5000K, 약하게), 주방 후드 아래 작업 조명(4000K).
- 창문 햇빛: Directional 5500K. 시간 흐름이 있으면 시간대별 프리셋을 만든다 (GDD에서 결정, 아래 확인 필요 참고).
- 그림자: Soft. 실시간 그림자는 메인 Directional과 플레이어 주변 몇 개만, 나머지는 베이크.
- 프리셋 경로: `Assets/02. Data/Lighting/` (Lighting Settings 에셋)

### 3. 포스트프로세싱

오니기리 시뮬의 옅은 웜톤을 따르되, 음식 색이 변하지 않도록 약하게 건다. `[초안]`

- Tonemapping: Neutral (음식 색 왜곡이 적음. ACES는 대비가 세고 주황이 붉게 뜸)
- Color Adjustments: Post Exposure 0, Contrast +8, Saturation -5
- White Balance: Temperature +8, Tint 0
- Color Lookup (LUT): 사용 안 함 (필요해지면 `Assets/05. Art/LUT/`, 기여도 0.3 이하)
- Bloom: Threshold 1.1, Intensity 0.25, Scatter 0.6 (창문과 조명 기구에만 살짝)
- Vignette: Intensity 0.18, Smoothness 0.4
- 프로파일 경로: `Assets/02. Data/Volumes/VP_Global.asset`

### 4. 재질 (Roughness / Metallic)

Unity URP Lit 기준이다. URP는 Smoothness를 쓰므로 Smoothness = 1 - Roughness로 바꿔 넣는다. `[초안]`

| 재질 | Roughness | Metallic | 비고 |
| --- | --- | --- | --- |
| 스테인리스 (헤어라인) | 0.30–0.40 | 1 | 반사는 살아 있되 거울처럼 비치지 않게 |
| 스테인리스 (바트, 작업대) | 0.35–0.50 | 1 | 손 닿는 면만 Roughness를 약간 다르게 |
| 무광 블랙 플라스틱 (용기) | 0.55–0.70 | 0 | 도시락 용기, 쟁반 |
| 유광 플라스틱 | 0.20–0.35 | 0 | 밥솥 뚜껑, 가전 외장 |
| 투명 PET (뚜껑, 포장) | 0.05–0.15 | 0 | Transparent, 알파 0.25–0.4 |
| 원목 (코팅) | 0.45–0.60 | 0 | 테이블, 벽 패널 |
| 도장 벽 / 석고 | 0.80–0.95 | 0 | |
| 바닥 타일 | 0.55–0.70 | 0 | |
| 종이 (봉투, 메뉴판, 영수증) | 0.85–1.00 | 0 | |
| 흰밥 | 0.50–0.65 | 0 | 밥알 윤기는 노멀맵으로 |
| 튀김옷 | 0.70–0.85 | 0 | 기름기 있는 부분만 0.45까지 |
| 양념 / 소스 / 국물 | 0.15–0.35 | 0 | 윤기가 음식의 맛있어 보임을 결정 |
| 계란 흰자 / 메추리알 | 0.30–0.45 | 0 | |

사용감 (결정: **약간의 사용감**): 깨끗한 바탕에, 손이 자주 닿는 곳(손잡이, 작업대 앞쪽 가장자리, 카운터)에만 옅은 긁힘과 Roughness 차이를 넣는다. 녹, 기름때, 찌든 얼룩은 넣지 않는다.

### 5. 텍셀 밀도

- 기준: 512 px/m `[초안]`
- 예외: 음식과 손에 드는 소품(도시락 용기, 주걱, 국자) 1024 px/m. 천장, 바닥 너머 외부, 먼 배경 256 px/m. 큰 면(벽, 바닥)은 타일링 텍스처와 트림 시트로 512 px/m를 유지한다.
- 확인 방법: Blender에서 `blender-retopology-uv`의 `texel_density.py`로 측정한다. 기준 대비 ±20% 안이면 통과.

### 6. 음식이 주인공

- 음식: 채도는 실제 음식 그대로 (낮추지 않음), 텍셀 밀도 1024 px/m, 윤기(낮은 Roughness)와 노멀맵 디테일을 가장 많이 쓴다. 화면에서 가장 눈에 띄어야 한다.
- 주방 기구: 형태는 실제 비율, 디테일은 손으로 조작하는 부분(손잡이, 버튼, 뚜껑)에만 집중한다. 채도는 낮게, 스테인리스와 블랙 위주.
- 배경: 채도와 대비를 가장 낮게 하고 디테일을 줄인다. 브랜드 옐로 외에 강한 색을 쓰지 않는다.

### 7. 모서리 처리

완전히 날카로운 90° 모서리는 없다. 모든 모서리에 Bevel을 넣어 하이라이트 한 줄이 생기게 한다. `[초안]`

- 큰 가구 (작업대, 카운터, 냉장고): Bevel 5–8mm, 3 segments
- 중간 소품 (밥솥, 바트, 그릇): Bevel 2–3mm, 2–3 segments
- 작은 소품 (용기, 수저, 소스통): Bevel 0.5–1mm, 1–2 segments
- 금지: Bevel 0인 날카로운 모서리, 소품 크기에 비해 과하게 둥근 "말랑한" 모서리, 같은 등급 안에서 Bevel 폭이 제각각인 것

## AI 모델 생성 규칙

AI 결과물은 "초벌 형태"로만 쓰고, 반드시 아래 정리 체크리스트를 거쳐 직접 모델링한 에셋과 구분되지 않게 만든다.

- 사용 도구: 당분간 사용 안 함. Blender MCP로 직접 모델링한다. 도구를 정하면 이 칸과 아래 템플릿을 갱신한다.
- 입력 방식: 미정 (도구 결정 시 텍스트, 이미지, 둘 다 중에서 정함)

### 프롬프트 템플릿

```
{object}, {material description}, realistic proportions, clean semi-realistic PBR,
simplified details, soft bevelled edges, Korean lunchbox shop kitchen prop,
neutral lighting, no text, no logo, single object, centered
```

예시: `rice cooker, brushed stainless steel body with glossy black plastic lid, realistic proportions, clean semi-realistic PBR, simplified details, soft bevelled edges, Korean lunchbox shop kitchen prop, neutral lighting, no text, no logo, single object, centered`

### 네거티브 프롬프트

```
text, logo, brand name, watermark, cartoon, toon, low detail blob, baked shadows,
baked highlights, rust, heavy grime, multiple objects, base platform, ground plane
```

### 폴리곤·텍스처 예산

`[초안]` 1인칭에서 가까이 보는 순서대로 예산을 크게 준다.

| 등급 | 예시 | 삼각형 수 | 텍스처 크기 |
| --- | --- | --- | --- |
| A. 히어로 | 음식, 도시락 용기, 손에 드는 도구 | 1k–8k | 1024–2048 |
| B. 상호작용 소품 | 밥솥, 튀김기, 포스기, 바트 | 2k–10k | 1024 |
| C. 큰 가구 | 작업대, 카운터, 냉장고, 테이블 | 1k–8k | 트림 시트 / 타일링 (2048 공용) |
| D. 배경 | 천장 설비, 외부 건물, 거리 소품 | 100–2k | 512 또는 공용 아틀라스 |

### 생성 후 정리 체크리스트

- [ ] 리토폴로지 또는 Decimate로 예산 안에 맞춘다
- [ ] 크기를 실제 미터 단위로 맞춘다 (도시락 용기 약 20×15×5cm)
- [ ] 피벗을 바닥 중앙에 둔다
- [ ] UV를 다시 펴고 텍셀 밀도를 맞춘다
- [ ] 구워진 그림자·하이라이트를 텍스처에서 지운다
- [ ] 텍스처 색을 팔레트 범위로 보정한다
- [ ] Roughness·Metallic을 재질 표 범위로 맞춘다
- [ ] 날카로운 모서리에 Bevel을 넣는다
- [ ] 내부에 박힌 면·구멍·뒤집힌 노멀을 고친다
- [ ] 상호작용이 필요하면 콜라이더용 단순 메시를 만든다

## 에셋 스펙과 네이밍

| 카테고리 | 접두어 | 등급 | 예시 | 폴더 |
| --- | --- | --- | --- | --- |
| 음식 | SM_Food_ | A | `SM_Food_PorkCutlet` | `Assets/05. Art/Models/Food/` |
| 용기·포장 | SM_Pack_ | A | `SM_Pack_Lunchbox6Slot` | `Assets/05. Art/Models/Packaging/` |
| 주방 도구 | SM_Tool_ | A | `SM_Tool_RicePaddle` | `Assets/05. Art/Models/Tools/` |
| 주방 기기 | SM_Equip_ | B | `SM_Equip_RiceCooker` | `Assets/05. Art/Models/Equipment/` |
| 가구 | SM_Furn_ | C | `SM_Furn_WorkTable` | `Assets/05. Art/Models/Furniture/` |
| 건물·환경 | SM_Env_ | C/D | `SM_Env_WallPanel` | `Assets/05. Art/Models/Environment/` |
| 캐릭터 | SK_Char_ | — | `SK_Char_Customer01` | `Assets/05. Art/Models/Characters/` |
| 콜라이더 메시 | UCX_ | — | `UCX_SM_Equip_RiceCooker` | 원본 모델과 같은 폴더 |

파일 규칙:

- 모델: `{접두어}{이름}.fbx` (Blender에서 `blender-export-pipelines`의 Unity 프리셋으로 내보냄)
- 머티리얼: `M_{재질}_{변형}` (예: `M_Steel_Brushed`, `M_Plastic_BlackMatte`)
- 텍스처: `T_{에셋명}_{종류}.png`. 종류 접미사는 `_BC`(Base Color), `_N`(Normal), `_MS`(Metallic R + Smoothness A, URP Lit용), `_AO`(Occlusion), `_E`(Emission)
- AI 원본: `ArtSource/AI_Raw/{도구}_{에셋명}_{날짜}.{확장자}` (Assets 밖, Git LFS)
- Blender 원본: `ArtSource/Blend/{카테고리}/{에셋명}.blend` (Assets 밖, Git LFS)
- 공용 머티리얼: `M_Steel_Brushed`, `M_Steel_Tray`, `M_Plastic_BlackMatte`, `M_Plastic_Glossy`, `M_PET_Clear`, `M_Wood_Light`, `M_Wood_Dark`, `M_Paint_Cream`, `M_Tile_WarmGray`, `M_Paper_Kraft`, `M_Brand_Yellow`. 위치는 `Assets/07. Materials/Shared/`. 새 에셋은 공용 머티리얼을 먼저 쓰고, 꼭 필요할 때만 전용 머티리얼을 만든다.

## 검수 절차

기준 씬: `Assets/04. Scenes/Dev/LookDev.unity` — 크림 화이트 벽, 웜 그레이 타일 바닥, 스테인리스 작업대, 기준 조명 프리셋과 VP_Global, 기준 소품(첫 번째는 밥솥), 크기 비교용 1m 큐브와 컬러 체커

1. 새 에셋을 기준 소품 옆 작업대 위에 둔다 (바닥 중앙 피벗이라 그대로 올라감)
2. 고정 카메라 3개로 찍는다. 1인칭 눈높이 1.65m에서 작업대를 내려다보는 시점, 같은 높이 정면, 30cm 근접
3. 아래 항목을 확인한다
4. 하나라도 걸리면 수정하고 다시 찍는다
5. 통과한 스크린샷은 `References/Tests/Approved/test_{에셋명}_v{버전}.png`로 저장한다

확인 항목:

- [ ] 색이 팔레트 범위 안에 있는가
- [ ] 주변 에셋보다 혼자 너무 반짝이거나 흐리지 않은가
- [ ] 텍스처 선명도가 주변과 비슷한가
- [ ] 모서리 둥글기가 주변과 비슷한가
- [ ] 크기가 현실 비율과 맞는가
- [ ] 음식이라면 주변보다 눈에 띄는가, 배경이라면 눈에 덜 띄는가
- [ ] 흑백으로 바꿔도 형태가 알아보이는가
- [ ] 삼각형 수와 텍스처 크기가 예산 안인가
- [ ] 로고, 브랜드명, 실제 상표가 들어가지 않았는가

## 확인 필요

- 시간 흐름: 하루 영업 중 창밖 햇빛이 바뀌는지 (GDD에서 결정). 바뀐다면 아침, 점심, 저녁 조명 프리셋 3개를 만든다.
- `[초안]` 수치 전체: 기준 소품(밥솥) 테스트 렌더를 비교해서 확정한다.
- 브랜드 옐로 HEX: 제목과 로고를 정할 때 함께 확정한다.
