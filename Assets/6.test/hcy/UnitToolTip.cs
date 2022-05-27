using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitToolTip : MonoBehaviour
{
    [SerializeField] private GameObject goBase;
        
    [SerializeField] private TextMeshProUGUI txtUnitName;
    [SerializeField] private TextMeshProUGUI txtUnitDesc;
    [SerializeField] private TextMeshProUGUI txtUnitHowToUsed;

    public StageManager stageManager;
    
    public void ShowToolTip(Unit unit, bool isQuickSlot, Vector3 pos, string keyBord)
    {
        goBase.SetActive(true);

        if (stageManager.inStage == true)
        {
            pos += new Vector3(goBase.GetComponent<RectTransform>().rect.width * 0.5f, goBase.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        }
        else
        {
            pos += new Vector3(goBase.GetComponent<RectTransform>().rect.width * 0.5f, -goBase.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        }
        
        goBase.transform.position = pos;
        
        txtUnitName.text = unit.unitName + " (" + unit.type + ")";
        txtUnitDesc.text = "소비하는 달빛 에너지 : " + unit.moonEnergy + "\n유닛 체력 : " + unit.hpStat + "\n유닛 공격력 : " + unit.attackStat + "\n유닛 공격 사거리 : " + unit.attackAddRangeStat + "\n공격 딜레이 : " + unit.attackDelayStat + "초" + "\n유닛 스피드 : " + unit.speedStat + "\n유닛 소환 쿨타임 : " + unit.spawnCoolTime + "초" + "\n유닛 치명타 확률 : " + unit.criRate + "\n유닛 치명타 대미지 : " + unit.criDamage;

        if (isQuickSlot == true && stageManager.inStage == false)
        {
            txtUnitHowToUsed.text = "좌클릭 - 해제";
        }
        else if (isQuickSlot == true && stageManager.inStage == true)
        {
            txtUnitHowToUsed.text = "좌클릭 or (" + keyBord + ") - 소환";
        }
        else if (isQuickSlot == false)
        {
            txtUnitHowToUsed.text = "좌클릭 - 장착";
        }
    }

    public void HideToolTip()
    {
        goBase.SetActive(false);
    }
}
