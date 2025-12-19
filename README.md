# Flight Fighter (Unity Arcade 1 vs 1 MultiplayGame)
> 유니티 기반 1 대 1 아케이드 멀티플레이 게임 (포톤 네트워크 기반)

**Abouts**
- 🏆 2022년도 2학기 AI 게임프로그래밍 강의 기말고사 작품 (1인 개발)  

**Quick Links**
- 🎮 플레이 시연 영상(Photon PUN): https://youtu.be/Ya8GWlwHJAQ?si=jv_7c-uHWJrbD0rv  
- 📘 [Technical Doc](#0-TOC)  
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

<a id="0-TOC"></a>
## TOC
  
- [네트워크 구조 개요 (Photon Room 기반)](#1-네트워크-구조-개요-photon-room-기반)
- [매칭/룸/씬 동기화 플로우](#2-매칭룸씬-동기화-플로우)
- [플레이어 스폰/Ownership 설계](#3-플레이어-스폰ownership-설계)
- [전투 동기화 (기관총/미사일) & 피격 판정](#4-전투-동기화-기관총미사일--피격-판정)
- [락온 판정 로직 (시야각/거리/가시성)](#5-락온-판정-로직-시야각거리-가시성)
- [UI/HUD 동기화 (타겟 거리/HP/카메라)](#6-uihud-동기화-타겟-거리hp카메라)
- [조작키 정리](#7-조작키-정리)

---

## 1. 네트워크 구조 개요 (Photon Room 기반)
본 프로젝트는 **Dedicated Server 권한형**이 아니라, Photon의 **Room 기반 멀티플레이**에서 제공하는 기능을 이용해 다음을 구현했습니다.

- **Photon Cloud**: 접속/매칭/룸 관리 (ConnectUsingSettings, JoinRandomOrCreateRoom)  
  - 관련 코드: `launcher.cs`

- **Gameplay Sync**: 클라이언트 간 **RPC + Network Instantiate + Ownership** 동기화  
  - 관련 코드: `WeaponManager.cs`, `MultyManager.cs`

---

## 2. 매칭/룸/씬 동기화 플로우

### 2.1 서버 접속 및 매칭
- 게임 시작 시 Photon 서버 접속 시도  
  - 관련 코드: `launcher.cs`

- UI에서 최대 인원/제한시간(maxTime) 선택 → 해당 조건으로 랜덤 매칭 또는 룸 생성  
  - 관련 코드: `launcher.cs`

### 2.2 씬 동기화
- `PhotonNetwork.AutomaticallySyncScene = true`  
  → 마스터가 `LoadLevel` 하면 전원이 같이 씬 이동  
  - 관련 코드: `launcher.cs`

- 룸 인원(=MaxPlayers) 충족 시 마스터가 `CutScene` 로드로 게임 시작  
  - 관련 코드: `launcher.cs`

---

## 3. 플레이어 스폰/Ownership 설계

### 3.1 스폰 전략
- `MultyManager`에서 `Player1/Player2`를 네트워크 `Instantiate` 후,  
  RPC로 ViewID를 전달하여 한쪽 오브젝트 소유권을 상대에게 `TransferOwnership`  
  - 관련 코드: `MultyManager.cs`

- 핵심 의도: 1 vs 1에서 각 클라이언트가 “자기 기체”를 소유하고,  
  입력/카메라/발사 등을 **자기 소유 오브젝트 기준**으로 처리하도록 구성

---

## 4. 전투 동기화 (기관총/미사일) & 피격 판정

### 4.1 기관총 (Bullet)
- `Fire1`을 누르고 있는 동안, RPC로 `FireMachineGun` 실행 → Bullet 네트워크 `Instantiate`  
  - 관련 코드: `WeaponManager.cs`

- Bullet 충돌 시 상대 Player 여부 체크 후 HP를 4 감소  
  - 관련 코드: `Bullet.cs`

### 4.2 미사일 (Missile)
- 현재 무기가 미사일이며, **락온 성공 시에만** RPC로 발사  
  - 관련 코드: `WeaponManager.cs`

- Missile 충돌 시 HP 30 감소 + 폭발 이펙트 네트워크 생성  
  - 관련 코드: `Missile.cs`

### 4.3 HP 동기화
- 피격한 로컬 플레이어는 hp 감소, RPC로 상대(targethp)도 감소시켜 UI에 반영  
  - 관련 코드: `HP.cs`

- HP 0이면 2초 후 ResultScene 이동  
  - 관련 코드: `HP.cs`

---

## 5. 락온 판정 로직 (시야각/거리/가시성)
`RockOn.CheckSight()`는 다음 흐름으로 락온을 판정합니다.

- `OverlapSphere`로 일정 거리 내 타겟 후보 탐색  
  - 관련 코드: `RockOn.cs` / `Rockon2.cs`

- 타겟 방향 벡터와 전방 벡터 각도 비교(viewAngle)  
  - 관련 코드: `RockOn.cs` / `Rockon2.cs`

- `Raycast`로 시야를 가리는 오브젝트가 없는지 확인  
  - 관련 코드: `RockOn.cs` / `Rockon2.cs`

- 이 구조 덕분에 미사일은 “그냥 누르면 나가는 무기”가 아니라,  
  **정면 시야에 들어온 적에게만 발사되는 락온 무기**로 역할이 분리됩니다.  
  - 관련 코드: `WeaponManager.cs`

---

## 6. UI/HUD 동기화 (타겟 거리/HP/카메라)

### 6.1 타겟 거리 및 아이콘 표시
- 타겟과 자신의 거리 계산 후 텍스트 표시  
  - 관련 코드: `TargetInfoUI.cs` / `TargetInfoUI2.cs`

- `WorldToScreenPoint`로 화면 좌표 투영 → 타겟 아이콘 위치 갱신  
  - 관련 코드: `TargetInfoUI.cs` / `TargetInfoUI2.cs`

### 6.2 카메라 전환
- Z / C 키로 카메라 전환 (Zoom/서브 카메라)  
  - 관련 코드: `PlayerController.cs`, `zCameraUI.cs`

### 6.3 결과 씬 → 로비 복귀
- ResultScene에서 Disconnect 후 Lobby 로드  
  - 관련 코드: `Resultscene.cs`

---

## 7. 조작키 정리
- **조준/기동**: 마우스 Y(피치), 마우스 X(요), A/D(롤: Horizontal)  
  - 관련 코드: `PlayerController.cs`

- **가속/감속**: W 가속, S 감속  
  - 관련 코드: `PlayerController.cs`, `speedController.cs`

- **발사**: Fire1(기본 좌클릭) — 기관총 연사 / 미사일 발사  
  - 관련 코드: `WeaponManager.cs`

- **무기 전환**: 1(기관총) / 2(미사일)  
  - 관련 코드: `WeaponnManager2.cs`

- **카메라**: Z / C 전환  
  - 관련 코드: `PlayerController.cs`

- **종료**: ESC  
  - 관련 코드: `PlayerController.cs`

</details>

---
