using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillManager : MonoBehaviour
{
    [Tooltip("도끼 던지기의 스탯\n0은 도끼가 꽃히는 거리\n1은 기본 대미지\n2는 대미지 퍼센트")] public float[] throwAxeStat;
    
    [Tooltip("팅커벨 힐스킬의 스탯\n0은 줄여주는 스킬 쿨타임\n1은 힐 이펙트가 사라지는 시간")] public float[] magicHealStat;
    
    [Tooltip("나나 샤우팅스킬의 스텟\n0은 증가할 넉백 저항 수치\n1은 샤우팅 이펙트가 사라지는 시간")] public float[] showeringStat;
    
    [Tooltip("달팽이의 박치기 스탯\n0은 기본 대미지\n1은 대미지 퍼센트\n2는 체력 회복량\n3은 힐 이펙트가 사라지는 시간")] public float[] buttStat;

    [Tooltip("출혈의 스탯\n0은 지속 시간\n1은 추가 대미지 퍼센트\n2는 지속 대미지를 주는 시간\n3은 지속 대미지")] public float[] hurtStat;
    
    [Tooltip("도발의 스탯\n0은 지속 시간")] public float[] tauntStat;
}
