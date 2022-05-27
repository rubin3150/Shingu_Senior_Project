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
    
    [Tooltip("유닛의 기본 공격 사거리")] public float attackRangeStat;
    
    [Tooltip("유닛의 추가 공격 사거리")] public float attackAddRangeStat;
    
    [Tooltip("유닛의 치명차 확률")] public int criRate;
    
    [Tooltip("유닛의 치명타 대미지")] public int criDamage;
    
    [Tooltip("유닛의 밀려나는 거리")] public float pushRange;

    [Tooltip("유닛의 밀려나는 거리 저항값")] public float pushResist;
    
    [Tooltip("유닛의 공격 딜레이")] public float attackDelayStat;
    
    [Tooltip("유닛의 이동 속도")] public float speedStat;
    
    [Tooltip("유닛의 체력")] public float hpStat;
    
    [Tooltip("유닛의 직업군")] public string type;

    [Tooltip("적 유닛을 처치하였을 때 얻을 수 있는 마나")] public float mpGet;
}
