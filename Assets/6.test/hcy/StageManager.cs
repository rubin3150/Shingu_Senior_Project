using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    // 스테이지에 진입 했는지 아닌지 체크할 변수
    public bool inStage;

    public GameObject[] spawnUnitTxt;
    public GameObject[] moonEnergyText;

    public GameObject[] skillTxt;
    public GameObject[] skillMoonEnergyTxt;

    public SelectUnitBase selectUnitBase;

    [SerializeField] private PlayerSet playerSet;

    [SerializeField] private Skill skill;

    public float maxMoonEnergy;
    public float nowMoonEnergy;
    public Image maxMoonEnergyImage;
    public Text moonText;

    public float moonEnergySpeed;

    public void SetActiveUnitText()
    {
        for (int i = 0; i < skillTxt.Length; i++)
        {
            skillTxt[i].SetActive(true);
            skillMoonEnergyTxt[i].GetComponent<Text>().text = playerSet.skillMoonEnergy[skill.skillNum + i].ToString();
            skillMoonEnergyTxt[i].SetActive(true);
        }
        
        for (int i = 0; i < spawnUnitTxt.Length; i++)
        {
            if (selectUnitBase.quickSlot[i].unit != null)
            {
                spawnUnitTxt[i].SetActive(true);
                moonEnergyText[i].GetComponent<Text>().text = selectUnitBase.quickSlot[i].unit.moonEnergy.ToString();
                moonEnergyText[i].SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

    private void Update()
    {
        if (inStage == true)
        {
            UpdateMoonBar();
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                CheckSpawnUnit(0);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                CheckSpawnUnit(1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                CheckSpawnUnit(2);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                CheckSpawnUnit(3);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                CheckSpawnUnit(4);
            }
        }
    }

    /// <summary>
    /// 유닛을 소환할 수 있는 체크
    /// </summary>
    private void CheckSpawnUnit(int num)
    {
        if (selectUnitBase.quickSlot[num].unit != null)
        {
            selectUnitBase.quickSlot[num].SpawnUnit(num + 1);
        }
    }
    
    public void UpdateMoonBar()
    {
        if (nowMoonEnergy >= 100)
        {
            nowMoonEnergy = 100;
        }
        else
        {
            nowMoonEnergy += moonEnergySpeed * Time.deltaTime;
        }
        moonText.text = Mathf.Floor(nowMoonEnergy) + " / " + maxMoonEnergy;
        // 체력 게이지 값 설정.
        maxMoonEnergyImage.fillAmount = nowMoonEnergy / maxMoonEnergy;
        
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }
}
