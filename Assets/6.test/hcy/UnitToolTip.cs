using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitToolTip : MonoBehaviour
{
    [SerializeField] private GameObject[] goBase;
        
    [SerializeField] private TextMeshProUGUI[] txtUnitInfo;
    
    public StageManager stageManager;

    private int num;
    
    public void ShowToolTip(Unit unit, Vector3 pos, bool isQuickSlot)
    {
        if (isQuickSlot == false)
        {
            if (unit.type == "Healer")
            {
                num = 0;
                txtUnitInfo[0].text = unit.unitName;
                txtUnitInfo[1].text = unit.healStat.ToString();
                txtUnitInfo[2].text = unit.hpStat.ToString();
                txtUnitInfo[3].text = unit.criRate + "%";
                txtUnitInfo[4].text = unit.criDamage + "%";
                txtUnitInfo[5].text = unit.speedStat.ToString();
                txtUnitInfo[6].text = unit.pushRange.ToString();
                txtUnitInfo[7].text = unit.pushResist.ToString();
                txtUnitInfo[8].text = unit.attackDelayStat.ToString();
                txtUnitInfo[9].text = unit.skillTxt;
            }
            else
            {
                num = 1;
                txtUnitInfo[10].text = unit.unitName;
                txtUnitInfo[11].text = unit.attackStat.ToString();
                txtUnitInfo[12].text = unit.hpStat.ToString();
                txtUnitInfo[13].text = unit.criRate + "%";
                txtUnitInfo[14].text = unit.criDamage + "%";
                txtUnitInfo[15].text = unit.speedStat.ToString();
                txtUnitInfo[16].text = unit.pushRange.ToString();
                txtUnitInfo[17].text = unit.pushResist.ToString();
                txtUnitInfo[18].text = unit.attackDelayStat.ToString();
                txtUnitInfo[19].text = unit.skillTxt;
            }
        
            goBase[num].SetActive(true);
        
            if (stageManager.inStage == true)
            {
                pos += new Vector3(goBase[num].GetComponent<RectTransform>().rect.width * 0.5f, goBase[num].GetComponent<RectTransform>().rect.height * 0.5f, 0f);
            }
            else
            {
                pos += new Vector3(-goBase[num].GetComponent<RectTransform>().rect.width * 0.5f, -goBase[num].GetComponent<RectTransform>().rect.height * 0.25f, 0f);
            }
        
            goBase[num].transform.position = pos;

        }
    }

    public void HideToolTip()
    {
        goBase[num].SetActive(false);
    }
}
