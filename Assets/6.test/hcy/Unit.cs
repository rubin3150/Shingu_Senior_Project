using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/unit")]
public class Unit : ScriptableObject
{
    [Tooltip("유닛의 이름")] public string unitName;
    
    [Tooltip("유닛의 프로필 이미지")] public Sprite unitImage; 
    
    [Tooltip("유닛의 프리팹")] public GameObject unitPrefab;

    [Tooltip("유닛의 소모하는 달빛 에너지")] public float moonEnergy;

    [Tooltip("유닛의 소환 쿨타임")] public float spawnCoolTime;
    
    [Tooltip("유닛의 공격력")] public float attackStat;

    [Tooltip("유닛의 회복력")] public float healStat;

    [Tooltip("적과 어느 정도의 거리를 두고 이동을 멈추게 하는 거리")]
    public float donMoveDistance;
    
    [Tooltip("적 타워 또는 플레이어와 어느 정도의 거리를 두고 이동을 멈추게 하는 거리")]
    public float donTowerMoveDistance;
    
    [Tooltip("유닛의 기본 공격 사거리")] public float attackRangeStat;

    [Tooltip("유닛의 치명차 확률")] public int criRate;
    
    [Tooltip("유닛의 치명타 대미지")] public int criDamage;
    
    [Tooltip("유닛의 밀려나는 거리")] public float pushRange;

    [Tooltip("유닛의 밀려나는 거리 저항값")] public float pushResist;
    
    [Tooltip("유닛의 공격 딜레이")] public float attackDelayStat;
    
    [Tooltip("유닛의 이동 속도")] public float speedStat;
    
    [Tooltip("유닛의 체력")] public float hpStat;
    
    [Tooltip("유닛의 직업군")] public string type;

    [Tooltip("적 유닛을 처치하였을 때 얻을 수 있는 마나")] public float mpGet;

    [Tooltip("유닛의 공격 타입")] public string attackType;

    [Tooltip("유닛이 보스 타입인지 아닌지 구분할 변수")] public string bossType;

    [Tooltip("유닛의 스킬 쿨타임")] public float skillCoolTime;
    
    [Tooltip("유닛의 스킬 설명")] public string skillTxt;
    
    [Tooltip("유닛의 스킬 인덱스\n0은 도끼 던지기\n1은 팅커벨의 힐스킬\n2는 나나의 샤우팅\n3은 후크 선장의 칼 휘두르기\n4는 벨의 꽃잎 날리기\n5는 백설의 사과 박스\n6은 앨리스의 고슴도치 던지기\n7은 달팽이의 박치기\n8은 슬라임의 마구찌르기")] public int skillIndex;
    
    [Tooltip("유닛의 스킬 사용후 상태이상\n0은 출혈\n1은 도발\n2는 실명\n3은 기절")] public int skillEffect;
}
