using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UnitSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 유닛 스크립트를 담을 변수
    public Unit unit;
    
    // SelectUnitBase 스크립트를 담을 변수
    [SerializeField] private SelectUnitBase selectUnitBase;

    [SerializeField] private StageManager stageManager;

        // 유닛의 이미지를 담을 변수
    [SerializeField] private Image unitImage; 
    
    // 전투 시작 버튼 오브젝트를 담을 변수
    [SerializeField] private GameObject startBtn;
    
    // 유닛을 소환할 때 부모로 사용할 오브젝트 (캔버스)
    [SerializeField] private GameObject[] parentTrans;
    
    // 전투 시작 텍스트 오브젝트를 담을 변수
    [SerializeField] private TextMeshProUGUI startText;

    public UnitToolTip unitToolTip;

    public bool isQuickSlot;

    public CoolTimeManager coolTimeManager;

    [SerializeField] private SkillManager _skillManager;

    private int _ranValue;

    public string keyBord;

    public Button btn;
    
    /// <summary>
    /// 이미지의 알파 값을 조절하는 함수
    /// </summary>
    /// <param name="alpha"></param>
    public void SetColor(float alpha)
    {
        // 변수에 유닛의 이미지 색을 넣음
        Color color = unitImage.color;
        
        // 이미지의 알파값에 인자 값을 넣음
        color.a = alpha;
        
        // 바뀐 알파 값을 적용함
        unitImage.color = color;                             
    }

    /// <summary>
    /// 유닛의 정보를 넣는 함수
    /// </summary>
    /// <param name="_unit"></param>
    public void AddUnit(Unit _unit)
    {
        // 변수에 받은 유닛의 정보를 넣음
        unit = _unit;

        if (_unit.unitName == "나나")
        {
            unitImage.GetComponent<RectTransform>().localPosition = new Vector3(-70, 15, 0);
        }
        else
        {
            unitImage.GetComponent<RectTransform>().localPosition = new Vector3(5, -25, 0);
        }
        
        // 변수에 받은 유닛의 이미를 넣음
        unitImage.sprite = unit.unitImage;
        
        // 함수 호출
        SetColor(1);

        btn.enabled = true;
    }

    /// <summary>
    /// 퀵슬롯에 유닛을 장착하는 함수
    /// </summary>
    private void MountingUnit()
    {
        // 변수에 참이라는 값을 넣음 (전투 시작 버튼을 누를 수 있음)
        selectUnitBase.startFlag = true;
        
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
        
        // 함수 호출
        SetColor(0.5f);
        
        // 함수 호출
        StartBtnColorSet(255f);

        // 함수 호출
        selectUnitBase.AddQuickSlot(unit);
    }

    /// <summary>
    /// 전투 시작 버튼과 전투 시작 텍스트의 알파값을 조절할 함수
    /// </summary>
    public void StartBtnColorSet(float btnAlpha)
    {
        // 전투 시작 버튼 알파 값 조절
        startBtn.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, btnAlpha / 255f);
        
        startText.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, btnAlpha / 255f);
    }

    /// <summary>
    /// 유닛슬롯에서 유닛을 클릭할떄 호출하는 함수
    /// </summary>
    public void UnitBtnClick()
    {
        // 변수 값이 없지 않다면 아래 코드 실행 (유닛이 비어있지않다면)
        if (unit != null)
        {
            // 변수가 유닛 슬롯의 최대 길이보다 크거나 같아질때까지 아래 코드 반복 실행
            for (int i = 0; i < selectUnitBase.quickSlot.Length; i++)
            {
                // i번쨰 퀵슬롯의 유닛이 비어있다면 아래 코드 실행
                if (selectUnitBase.quickSlot[i].unit == null)
                {
                    // 함수 호출
                    MountingUnit();
                }
            }
        }
    }

    /// <summary>
    /// 유닛을 담는 칸의 정보를 초기화시키는 함수
    /// </summary>
    public void Clear()
    {
        SetColor(0);
        
        // 유닛의 정보를 비어있음으로 설정
        unit = null;

        // 유닛의 이미지를 비어있음으로 설정
        unitImage.sprite = null;

        btn.enabled = false;
    }

    /// <summary>
    /// 퀵슬롯에서 유닛을 클릭할떄 호출하는 함수
    /// </summary>
    /// <param name="Num"></param>
    public void QuickSlotBtnClick(int Num) // int
    {
        // 변수 값이 거짓이라면 아래 코드 실행 (전투 스테이지가 아니라면)
        if (stageManager.inStages == false)
        {
            // 유닛의 정보가 없지 않다면 아래 코드 실행
            if (unit != null)
            {
                Data.Instance.sfx.clip = Data.Instance.btnClip;
                Data.Instance.sfx.Play();
                
                unitToolTip.HideToolTip();
                
                // 함수 호출
                selectUnitBase.ResetSelectUnit(unit);
                
                // 함수 호출
                Clear();

                // 함수 호출
                selectUnitBase.QuickSlotSort(Num);
            }
        }
        // 변수 값이 참이라면 아래 코드 실행 (전투 스테이지라면)
        else
        {
            Data.Instance.sfx.clip = Data.Instance.btnClip;
            Data.Instance.sfx.Play();
            SpawnUnit(Num);
        }
    }

    public void SpawnUnit(int num)
    {
        if (!coolTimeManager.isCoolTime[num - 1] && stageManager.isStop == false)
        {
            if (stageManager.nowMoonEnergy >= unit.moonEnergy)
            {
                coolTimeManager.CoolTime(num, false);
                
                stageManager.nowMoonEnergy -= unit.moonEnergy;

                _ranValue = Random.Range(0, 3);

                // 유닛 생성
                GameObject go = Instantiate(unit.unitPrefab, new Vector3(-46.25f, 0, 0), Quaternion.identity);
                
                _skillManager.AddUnit(go);

                if (_ranValue == 0)
                {
                    go.GetComponent<RectTransform>().SetParent(parentTrans[0].transform, false);
                }
                else if (_ranValue == 1)
                {
                    go.GetComponent<RectTransform>().SetParent(parentTrans[1].transform, false);
                }
                else
                {
                    go.GetComponent<RectTransform>().SetParent(parentTrans[2].transform, false);
                }

                go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                if (unit.unitName == "팅커벨")
                {
                    go.GetComponent<RectTransform>().localPosition = new Vector3(-46.25f, -4.75f + -2.5f * _ranValue, 0);
                }
                else
                {
                    go.GetComponent<RectTransform>().localPosition = new Vector3(-46.25f, -0.5f + -2.5f * _ranValue, 0);
                }
            }
            else
            {
                if (_skillManager.moonCheck == false)
                {
                    _skillManager.ShowMoonWarningUI("달빛이 부족합니다.");
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unit != null)
        {
            unitToolTip.ShowToolTip(unit, transform.position, isQuickSlot);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        unitToolTip.HideToolTip();
    }

    
}
