using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int skillNum;
    
    public Image skillImage;

    public bool isActive;

    [SerializeField] private PlayerSet playerSet;

    private int _currentSkillNum;

    [SerializeField] private StageManager stageManager;
    
    [SerializeField] private GameObject goBase;
        
    [SerializeField] private Text txtSkillName;
    [SerializeField] private Text txtSkillDesc;
    [SerializeField] private Text txtSkillHowToUsed;

    private void Start()
    {
        _currentSkillNum = skillNum - playerSet.playerNum * 3;
        
        skillImage.sprite = playerSet.skillImage[skillNum];

        SetColor();
    }

    public void SetColor()
    {
        if (isActive == true)
        {
            skillImage.color = Color.white;
        }
        else
        {
            skillImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 125f / 255f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isActive == true)
        {
            ShowSkillToolTip(transform.position);
        }
    }

    public void ShowSkillToolTip(Vector3 pos)
    {
        goBase.SetActive(true);

        if (stageManager.inStage == true)
        {
            pos += new Vector3(-goBase.GetComponent<RectTransform>().rect.width * 0.5f, 0f, 0f);
        }
        else
        {
            pos += new Vector3(-goBase.GetComponent<RectTransform>().rect.width * 0.5f, -goBase.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        }
        
        goBase.transform.position = pos;
        
        txtSkillName.text = playerSet.skillName[skillNum];

        txtSkillDesc.text = "소비하는 달빛 에너지 : " + playerSet.skillMoonEnergy[skillNum] + "\n스킬 사거리 : " + playerSet.skillRange[skillNum] + "\n스킬 쿨타임 : " +
                            playerSet.skillCoolTime[skillNum] + "초" + "\n스킬 유지 시간 : " +
                            playerSet.maintainTime[skillNum] + "초";
        
        if (_currentSkillNum == 0)
        {
            txtSkillDesc.text += "\n스킬 가이드 : 사거리 내에 있는 모든 탱커\n유닛의 체력을 " + playerSet.addStat[skillNum] + "만큼 상승시킨다";
        }
        else if (_currentSkillNum == 1)
        {
            txtSkillDesc.text += "\n스킬 가이드 : 중앙 사거리 내에 있는 모든\n딜러 유닛의 공격력을 " + playerSet.addStat[skillNum] + 
                                 "만큼 상승시킨다\n가장자리 사거리 내에 있는 모든 딜러 유\n닛의 공격력을 " + playerSet.addStat1[skillNum] +
                                 "만큼 상승시키고 체력을\n" + playerSet.addStat[skillNum] + "만큼 상승시킨다";
        }
        else
        {
            txtSkillDesc.text += "\n스킬 가이드 : 사거리 내에 있는 모든 힐러\n유닛의 회복량을 " + playerSet.addStat[skillNum] + "만큼 상승시킨다";
        }
        
        if (stageManager.inStage == false)
        {
            txtSkillHowToUsed.text = "";
        } 
        else
        {
            if (_currentSkillNum == 0)
            {
                txtSkillHowToUsed.text = "좌클릭 or (Z) - 사용";
            }
            else if (_currentSkillNum == 1)
            {
                txtSkillHowToUsed.text = "좌클릭 or (X) - 사용";
            }
            else
            {
                txtSkillHowToUsed.text = "좌클릭 or (C) - 사용";
            }
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        goBase.SetActive(false);
    }

    public void UseSkill()
    {
        // if ()
    }
}
