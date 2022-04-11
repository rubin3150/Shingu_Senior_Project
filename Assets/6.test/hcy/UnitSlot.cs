using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
{
    public Unit unit; // 획득한 유닛
    public Image unitImage; // 유닛의 이미지
    
    public GameObject StartBtn;
    public Text startText;

    public GameObject parentTrans;

    [SerializeField] private SelectUnitBase _selectUnitBase;

    public void SetColor(float _alpha)
    {
        Color color = unitImage.color;
        color.a = _alpha;
        unitImage.color = color;
    }

    public void AddUnit(Unit _unit)
    {
        unit = _unit;
        unitImage.sprite = unit.unitImage;
        
        SetColor(1);
    }

    public void ClearUnit()
    {
        _selectUnitBase.startFlag = true;
        //unit = null;
        //unitImage.sprite = null;
        SetColor(0.5f);
        StartBtn.GetComponent<Image>().color = new Color(100f / 255f, 100f / 255f, 100f / 255f, 255f / 255f);
        startText.color = new Color(1, 1, 1, 1);
        _selectUnitBase.AddQuickSlot(unit);
    }

    public void StartBrnColorSet()
    {
        StartBtn.GetComponent<Image>().color = new Color(100f / 255f, 100f / 255f, 100f / 255f, 155f / 255f);
        startText.color = new Color(1, 1, 1, 0.5f);
    }

    public void BtnClick()
    {
        if (unit != null)
        {
            for (int i = 0; i < _selectUnitBase.quickSlot.Length; i++)
            {
                if (_selectUnitBase.quickSlot[i].unit == null)
                {
                    ClearUnit();
                }
            }
            // 몬스터 생성
            
        }
    }

    public void Clear()
    {
        unit = null;
        unitImage.sprite = null;
    }

    public void BtnRightClick(int Num) // int
    {
        if (_selectUnitBase.inStage == false)
        {
            if (unit != null)
            {
                _selectUnitBase.ResetSelectUnit(unit);
                Clear();
                _selectUnitBase.ResetBtn(Num);
            }
        }
        else
        {
            GameObject go = Instantiate(unit.unitPrefab, Vector3.zero, Quaternion.identity);

            go.transform.parent = parentTrans.transform;

            go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            
            go.GetComponent<RectTransform>().localPosition = new Vector3(-800, 0,0 );
            
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 400);
        }
    }
}
