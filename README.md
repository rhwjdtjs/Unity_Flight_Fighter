# Flight Fighter (Unity Arcade 1 vs 1 MultiplayGame)
> 유니티 기반 1 대 1 아케이드 멀티플레이 게임 (포톤 네트워크 기반)

**Abouts**
- 🏆 2022년도 2학기 AI 게임프로그래밍 강의 기말고사 작품 (1인 개발)  

**Quick Links**
- 🎮 플레이 시연 영상(Photon PUN): https://youtu.be/Ya8GWlwHJAQ?si=jv_7c-uHWJrbD0rv  
- 📘 Technical Doc(아래 접기 섹션): README 하단

---
## Introduction
- Flight Fighter는 전투기를 조종해 1:1로 교전하는 아케이드 공중전 멀티플레이 게임입니다.
- Photon PUN의 Room 기반 멀티플레이를 사용해, 플레이어는 룸(매치)에 입장하면 전투 씬으로 동기화되어 게임을 시작합니다.  

**Development**
- **기간**: 2022.11 ~ 2022.12
- **인원**: 1인 개발
- **환경**: Unity 2019.3.0f / Visual Studio 2021 / Windows 11
- **역할**: Gameplay(C#), 네트워크(매칭/룸/동기화), UI·HUD, 사운드/이펙트 연동  

**Motivation & Intent**
- 싱글 프로젝트 경험 이후 “멀티플레이를 직접 완성해보자”를 목표로, 단순 튜토리얼 복제가 아니라 전투기 아케이드 교전이라는 레퍼런스가 적은 장르를 선택했습니다.
- Photon의 기본 기능(룸/마스터클라)을 그대로 쓰는 데서 끝내지 않고, 오브젝트 소유권, RPC 기반 발사/피격/결과 처리까지 포함한 “게임 플레이가 성립하는 단위”를 직접 설계/구현했습니다. 

## Overview  
- 플레이어는 룸 입장 후 컷씬 씬으로 동기화되어 게임을 시작하며, 전투 중에는 기관총/미사일을 사용해 상대 HP를 0으로 만들면 결과 씬으로 이동합니다.

**Highlights**
- **Room Based Matchmaking: 인원/제한시간(maxTime) 조건 기반으로 랜덤 매칭 또는 룸 생성**
- **Scene Sync: 마스터 클라이언트가 인원 충족 시 컷씬 로드, 모든 클라이언트 씬 동기**
- **Combat Sync: RPC/Network Instantiate 기반의 발사(기관총/미사일) 및 피격/HP 동기화**

---

## Key Features
**1 vs 1 룸 기반 멀티플레이**
- Dropdown으로 최대 인원 / 제한 시간(maxTime) 을 정하고 JoinRandomOrCreateRoom 수행
- 룸 인원이 꽉 차면 마스터가 게임 시작 씬을 로드
**전투 시스템 (기관총 / 미사일)**
- 기관총: Fire1 유지 시 RPC로 발사, Bullet을 네트워크 Instantiate
- 미사일: Fire1 클릭 시 락온 상태에서만 RPC 발사
**락온(시야각/거리 기반)**
- OverlapSphere + 시야각(viewAngle) + Raycast로 “정면 시야에 들어온 적”만 락온 판정
**HP/결과 처리**
- 피격 시 HP 감소 후 RPC로 상대 UI에 표시될 targetHP 갱신
- HP 0이면 2초 후 ResultScene 이동
**HUD/타겟 정보 UI**
- 타겟 거리, 화면 투영(WorldToScreenPoint) 기반 타겟 아이콘/락온 아이콘 표시
- Player2용 UI 스크립트를 따로 둬서 1:1 대칭 구조 유지 

---

## Tech Stack  
![Unity](https://img.shields.io/badge/Unity-000000?style=flat&logo=unity&logoColor=white)
![C%23](https://img.shields.io/badge/C%23-512BD4?style=flat&logo=csharp&logoColor=white)
![Photon%20PUN2](https://img.shields.io/badge/Photon%20PUN2-00B8FF?style=flat&logo=photon&logoColor=white)
![Realtime](https://img.shields.io/badge/Photon%20Realtime-1E90FF?style=flat)
![Room%20Based](https://img.shields.io/badge/Room%20Based%20Multiplayer-444444?style=flat)
- **Engine**: Unity 2019.3.0f  
- **Language**: C# 
- **Network**: Photon Network 

---

## Media
### Gameplay (2 Players)
[![1 vs 1 demo](https://github.com/user-attachments/assets/44da3149-1118-4c2d-8e89-990c4d1728f2)](https://youtu.be/Ya8GWlwHJAQ?si=Q5mcs5hklmQoLiNp)

---


<details>
  <summary><b>📘 Technical Documentation (Flight Fighter 기술서) (펼치기)</b></summary>

## TOC

- 예정
- 
</details>

---
