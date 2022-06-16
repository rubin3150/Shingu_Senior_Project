using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSet : MonoBehaviour
{
    // 몇번쨰 플레이어인지 알기 위한 변수
    public int playerNum;
    
    // 플레이어의 이미지를 담을 변수 
    public Sprite[] playerImages;

    public string[] skillName;
    
    // 플레이어의 최대 Hp를 담을 변수
    public float[] maxHpStat;

    public float[] manaStat;
    
    // 플레이어의 스피드를 담을 변수
    public float[] speedStat;

    // 플레이어의 스킬이미지를 담을 변수 
    public Sprite[] skillImage;

    // 플레이어의 스킬 쿨타임을 담을 변수 
    public float[] skillCoolTime;

    public float[] skillMoonEnergy;
    
    // 플레이어의 스킬이 유지되는 시간을 담을 변수 
    public float[] maintainTime;

    // 플레이어의 스킬에 따라 증가하는 첫번째 스텟을 담을 변수 
    [Tooltip("플레이어의 스킬 스탯\n0은 힐러의 회복량\n1은 근거리 딜러의 공격력\n2는 원거리 딜러의 치명타 확률\n3은 탱커의 넉백 저항\n4는 모든 유닛의 이동 속도\n5는 스킬 쿨타임\n6은 3번째 스킬의 사거리\n7은 랜덤 넉백의 최소값\n8은 랜덤 넉백의 최대값\n9는 고정 대미지 값")] public int[] addStat;
}
