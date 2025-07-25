
![Vomic_noise (1)](https://github.com/user-attachments/assets/b2021fb2-5d06-4c00-a952-8591659b6f40)

AI 음성 기술(STT → LLM → TTS) 기반의 **보이스클로닝 공포게임**  
플레이어의 음성을 학습해 **몬스터가 동료를 가장**하며 혼란을 유도하는  
**심리 기반 2인 협동 방탈출 게임**입니다

## 0. 팀원 소개

|손서희</br>[@sh1257](https://github.com/sh1257)|이현정</br>[@KKANGCHONG](https://github.com/KKANGCHONG)|
|:---:|:---:|
|이화여자대학교 컴퓨터공학과 21학번|이화여자대학교 컴퓨터공학과 22학번|
|`클라이언트 개발`, `UI/UX 디자인`|`서버 환경 구축`, `AI 개발`|


## 1. 프로젝트 개요

- **장르**: 3D 공포 협동 방탈출 게임  
- **플레이 인원**: 2인 멀티플레이 (Photon)  
- **기술 스택**: Unity3D, Azure, FastAPI, Google STT, OpenAI GPT, PlayHT TTS  
- **핵심 요소**: 실시간 보이스 클로닝 / 심리적 긴장 유도 / 음성 기반 협력


## 2. 목표 및 문제 정의

기존 공포 게임은 점프 스케어나 추격에 의존하여 **예측 가능성**, **몰입도 저하** 등의 한계를 가짐.  
MANSION-VOMIC은 **신뢰와 의심이 교차하는 새로운 공포 경험**을 제공하고자 함.

- **핵심 기능**: 몬스터가 플레이어의 목소리를 학습 → LLM으로 대사 생성 → 클론된 목소리로 말함  
- **목표 사용자**: 새로운 긴장감과 몰입을 원하는 공포게임 애호가 및 멀티 협동 게이머


## 3. 기존 콘텐츠와의 차별점
![장단점표2](https://github.com/user-attachments/assets/6f9283dc-ff60-4079-aa21-6b5b3d3454cb)


## 4. 주요 기능

### 4.1. LLM 응답 생성 (GPT 기반)

- 플레이어 음성 → STT → 게임 상황 반영 프롬프트 → LLM → 응답 텍스트 생성
- GPT 대사는 대화 맥락과 세계관에 기반한 자연스러운 문장

### 4.2. 보이스클로닝 기반 TTS

- 생성된 대사 → PlayHT API + 클론된 voice_id → mp3/wav로 변환
- 몬스터가 **플레이어 목소리로 유혹/속임**

### 4.3. Unity 게임 구조

- **맵 구조**: B1~Attic 5개 층 / 각 층 방마다 랜덤 아이템 생성
- **아이템**: 총 11종, 일부 확률 생성 (ex. 마법봉 5%)
- **인벤토리**: 줍기/버리기 가능, 특정 조건 하에 사용 가능

### 4.4. 몬스터 AI

- FSM 기반 순찰 + 이벤트 반응형 추적
- 플레이어 발화/행동 감지 → AI 대사 유도 → 클론 음성 출력
- 동일 위치 반복 재생 방지, 쿨타임 및 조건 기반 대사 로직


## 5. 시스템 구조
![Frame 7 1](https://github.com/user-attachments/assets/84b62785-7a3b-42aa-b829-fb451e97cc3d)

## 6. 데모 영상
[🔗데모 영상 바로보기](https://youtu.be/uMuQ0SY0r-U)

## 7. 참조

### 7-1. 멀티 관련

- Main 브랜치: 1인 플레이, 몬스터의 즉각적인 대답
- Multiplay 브랜치: 2인 플레이, 몬스터AI를 통한 랜덤 발화

### 7-2. 에셋 관련

- 프로젝트에 해당 레포지토리에 포함되지 않은 에셋을 사용하고 있습니다.
- 따라서, 본 프로젝트를 다운받는다고 해도, 해당 에셋을 구매하여 따로 설치하지 않는 이상은 활용이 불가능합니다.
- 사용한 에셋은 다음과 같습니다: Mansion Set - 유니티 에셋스토어
