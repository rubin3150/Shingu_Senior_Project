using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/unit")]
public class Unit : ScriptableObject
{
    // 유닛의 이름
    public string unitName;
    
    // 유닛의 이미지
    public Sprite unitImage; 

    // 유닛의 프리팹
    public GameObject unitPrefab;

    public float moonEnergy;

    public float hpStat;

    public float attackStat;

    public float speedStat;

    public float spawnCoolTime;

    public float attackRangeStat;

    public float attackDelayStat;

    public float pushRange;

    public int criRate;

    public int criDamage;

    public string type;
}
