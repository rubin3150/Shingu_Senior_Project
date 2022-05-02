using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeManager : MonoBehaviour
{
    #region 변수 영역

    // SelectUnitBase 스크립트를 담을 변수
    [SerializeField] private SelectUnitBase selectUnitBase;

    [SerializeField] private PlayerSet playerSet;
    
    // 쿨타임 이미지를 담을 변수
    [SerializeField] private Image[] coolTimeImage;

    [SerializeField] private Skill[] skiils;
    
    // 현재 쿨타임을 담을 변수
    [SerializeField] private float[] currentCoolTIme;
    
    // 쿨타임인지 아닌지 체크할 변수
    public bool[] isCoolTime;

    #endregion
    
    /// <summary>
    /// 매 프레임마다 호출되는 함수
    /// </summary>
    private void Update()
    {
        // 함수 호출
        CoolTimeCalc();
    }
    
    /// <summary>
    /// 어떤 쿨타임을 시작할지 정하는 함수
    /// </summary>
    /// <param name="num"></param>
    public void CoolTime(int num, bool skill)
    {
        if (skill == false)
        {
            // 배열 인자값 - 1번째 변수에 배열 인자값 - 1번째 퀵슬롯 유닛에 있는 유닛 쿨타임을 넣어줌 (인자값 - 1은 자기자신을 의미함)
            currentCoolTIme[num - 1] = selectUnitBase.quickSlot[num - 1].unit.spawnCoolTime;
        }
        else
        {
            currentCoolTIme[num - 1] = playerSet.skillCoolTime[skiils[num - 6].skillNum];
        }

        // 배열 인자값 - 1번째 변수에 참이라는 값을 넣어줌 (쿨타임 실행 가능)
        isCoolTime[num - 1] = true;
    }
    
    

    /// <summary>
    /// 쿨타임을 실행 시키는 함수
    /// </summary>
    private void CoolTimeCalc()
    {
        // 배열 0 번째 변수 값이 참이라면 아래 코드 실행 (1 번째 퀵슬롯의 유닛을 소환했다면)
        if (isCoolTime[0] == true)
        {
            // 배열 0번째 현재 쿨타임 변수에 실제 시간을 빼줌
            currentCoolTIme[0] -= Time.deltaTime;
            
            // 배열 0번째 쿨타임 이미지를 바꿈
            coolTimeImage[0].fillAmount = currentCoolTIme[0] / selectUnitBase.quickSlot[0].unit.spawnCoolTime;

            // 배열 0번째 쿨타임 변수의 값이 0보다 작거나 같다면 아래 코드 실행 (유닛의 쿨타임만큼 시간이 되었다면)
            if (currentCoolTIme[0] <= 0)
            {
                // 배열 0번쨰 변수에 거짓이라는 값을 넣음 (쿨타임 실행 중단)
                isCoolTime[0] = false;
            }
        }

        // 배열 1번째 변수 값이 참이라면 아래 코드 실행 (2 번째 퀵슬롯 유닛을 소환했다면)
        if (isCoolTime[1] == true)
        {
            // 배열 1번째 현재 쿨타임 변수에 실제 시간을 뻬줌
            currentCoolTIme[1] -= Time.deltaTime;
            
            // 배열 1번째 쿨타임 이미지를 바꿈
            coolTimeImage[1].fillAmount = currentCoolTIme[1] / selectUnitBase.quickSlot[1].unit.spawnCoolTime;

            // 배열 1번째 쿨타임 변수의 값이 0보다 작거나 같다면 아래 코드 실행 (유닛의 쿨타임만큼 시간이 되었다면)
            if (currentCoolTIme[1] <= 0)
            {
                // 배열 1번째 변수에 거짓이라는 값을 넣음 (쿨타임 실행 중단)
                isCoolTime[1] = false;
            }
        }

        // 배열 2번째 변수 값이 참이라면 아래 코드 실행 (3 번째 퀵슬롯 유닛을 소환했다면)
        if (isCoolTime[2] == true)
        {
            // 배열 2번째 현재 쿨타임 변수에 실제 시간을 빼줌
            currentCoolTIme[2] -= Time.deltaTime;
            
            // 배열 2번째 쿨타임 이미지를 바꿈
            coolTimeImage[2].fillAmount = currentCoolTIme[2] / selectUnitBase.quickSlot[2].unit.spawnCoolTime;

            // 배열 2번쨰 쿨타임 변수의 값이 0보다 작거나 같다면 아래 코드 실행 (유닛의 쿨타임만큼 시간이 되었다면)
            if (currentCoolTIme[2] <= 0)
            {
                // 배열 2번째 변수에 거짓이라는 값을 넣음 (쿨타임 실행 중단)
                isCoolTime[2] = false;
            }
        }

        // 배열 3번째 변수 값이 참이라면 아래 코드 실행 (4 번째 퀵슬롯 유닛을 소환했다면)
        if (isCoolTime[3] == true)
        {
            // 배열 3번째 현재 쿨타임 변수에 실제 시간을 빼줌
            currentCoolTIme[3] -= Time.deltaTime;
            
            // 배열 3번째 쿨타임 이미지를 바꿈
            coolTimeImage[3].fillAmount = currentCoolTIme[3] / selectUnitBase.quickSlot[3].unit.spawnCoolTime;

            // 배열 3번째 쿨타임 변수의 값이 0보다 작거나 같다면 아래 코드 실행 (유닛의 쿨타임만큼 시간이 되었다면)
            if (currentCoolTIme[3] <= 0)
            {
                // 배열 3번째 변수에 거짓이라는 값을 넣음 (쿨타임 실행 중단)
                isCoolTime[3] = false;
            }
        }

        // 배열 4번쨰 변수 값이 참이라면 아래 코드 실행 (5 번쨰 퀵슬롯 유닛을 소환했다면)
        if (isCoolTime[4] == true)
        {
            // 배열 4 번째 쿨타임 변수에 실제 시간을 뺴줌
            currentCoolTIme[4] -= Time.deltaTime;
            
            // 배열 4번쨰 쿨타임 이미지를 바꿈
            coolTimeImage[4].fillAmount = currentCoolTIme[4] / selectUnitBase.quickSlot[4].unit.spawnCoolTime;

            // 배열 4번째 쿨타임 변수의 값이 0보다 작거나 같다면 아래 코드 실행 (유닛의 쿨타임만큼 시간이 되었다면)
            if (currentCoolTIme[4] <= 0)
            {
                // 배열 4번째 변수에 거짓이라는 값을 넣음 (쿨타임 실행 중단)
                isCoolTime[4] = false;
            }
        }

        if (isCoolTime[5] == true)
        {
            currentCoolTIme[5] -= Time.deltaTime;
            
            coolTimeImage[5].fillAmount = currentCoolTIme[5] / playerSet.skillCoolTime[skiils[0].skillNum];

            if (currentCoolTIme[5] <= 0)
            {
                isCoolTime[5] = false;
            }
        }

        if (isCoolTime[6] == true)
        {
            currentCoolTIme[6] -= Time.deltaTime;

            coolTimeImage[6].fillAmount = currentCoolTIme[6] / playerSet.skillCoolTime[skiils[1].skillNum];

            if (currentCoolTIme[6] <= 0)
            {
                isCoolTime[6] = false;
            }
        }

        if (isCoolTime[7] == true)
        {
            currentCoolTIme[7] -= Time.deltaTime;

            coolTimeImage[7].fillAmount = currentCoolTIme[7] / playerSet.skillCoolTime[skiils[2].skillNum];

            if (currentCoolTIme[7] <= 0)
            {
                isCoolTime[7] = false;
            }
        }
    }
}
