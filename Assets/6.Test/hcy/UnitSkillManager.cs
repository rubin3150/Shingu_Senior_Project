using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillManager : MonoBehaviour
{
    [Tooltip("도끼 던지기의 스탯\n0은 도끼가 꽃히는 거리\n1은 기본 대미지\n2는 대미지 퍼센트")] public float[] throwAxeStat;
    
    [Tooltip("팅커벨 힐스킬의 스탯\n0은 줄여주는 스킬 쿨타임\n1은 힐 이펙트가 사라지는 시간")] public float[] magicHealStat;
    
    [Tooltip("나나 샤우팅스킬의 스텟\n0은 증가할 넉백 저항 수치\n1은 샤우팅 이펙트가 사라지는 시간")] public float[] showeringStat;
    
    [Tooltip("후크 선장의 칼휘두르기의 스텟\n0은 기본 대미지\n1은 대미지 퍼센트\n2적이 즉사할 확률")] public float[] swingSwordStat;
    
    [Tooltip("벨 꽃잎 날리기의 스텟\n0은 기본 대미지\n1은 대미지 퍼센트")] public float[] windRoseStat;
    
    [Tooltip("백설 사과 박스의 스텟\n0은 지속 힐을 주는 시간\n1은 회복하는 체력\n2는 증가할 넉백 수치\n3은 감소할 넉백 저항 수치\n4는 지속 시간")] public float[] appleBoxStat;

    [Tooltip("달팽이 박치기의 스탯\n0은 기본 대미지\n1은 대미지 퍼센트\n2는 체력 회복량\n3은 힐 이펙트가 사라지는 시간")] public float[] buttStat;

    [Tooltip("슬라임 마구찌르기의 스탯\n0은 기본 대미지\n1은 대미지 퍼센트\n2는 5회 공격할 확률\n3은 6회 공격할 확률\n4는 7회 공격할 확률\n5는 8회 공격할 확률\n6은 9회 공격할 확률\n7은 10회 공격할 확률")]
    public float[] slimeStat;
    
    [Tooltip("유령 일반공격 강화의 스탯\n0은 기본 대미지\n1은 대미지 퍼센트\n2는 지속 시간")]
    public float[] upgradeAttackStat;

    [Tooltip("출혈의 스탯\n0은 지속 시간\n1은 추가 대미지 퍼센트\n2는 지속 대미지를 주는 시간\n3은 지속 대미지")] public float[] hurtStat;
    
    [Tooltip("도발의 스탯\n0은 지속 시간")] public float[] tauntStat;
    
    [Tooltip("실명의 스탯\n0은 지속 시간\n1은 받는 대미지 증가량")] public float[] blindStat;
    
    [Tooltip("기절의 스탯\n0은 지속 시간\n1은 감소할 넉백 저항 수치")] public float[] stunStat;
}
