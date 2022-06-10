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
    public int[] addStat;
}
