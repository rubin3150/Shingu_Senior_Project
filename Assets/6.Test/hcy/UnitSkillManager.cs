using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillManager : MonoBehaviour
{
    [Tooltip("도끼 던지기의 스탯\n0은 도끼가 꽃히는 거리\n1은 기본 대미지\n2는 대미지 퍼센트")] public float[] throwAxeStat;

    [Tooltip("출혈의 스탯\n0은 지속 시간\n1은 추가 대미지 퍼센트\n2는 지속 대미지를 주는 시간\n3은 지속 대미지")] public float[] hurtStat;
}
