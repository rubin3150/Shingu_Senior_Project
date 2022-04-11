using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectUnitBase : MonoBehaviour
{
    // 유닛 세팅을 하는 씬을 담을 변수 
    [SerializeField] GameObject selectUnitBase;
    
    // 유닛슬롯의 부모 오브젝트를 담을 변수
    [SerializeField] private GameObject unitSlotParent;
    
    // 퀵슬롯의 스크립트를 관리할 변수 
    public UnitSlot[] quickSlot;

    // 킉슬롯에 오브젝트가 하나라도 있는지 없는지 체크하기 위한 변수
    public bool startFlag;

    // 스테이지에 진입 했는지 아닌지 체크할 변수
    public bool inStage;

    // 유닛슬롯의 스크립트를 관리할 변수
    private UnitSlot[] _unitSlots;

    // 유닛 세팅을 하는 씬의 알파값 이미지를 담을 변수
    [SerializeField] private GameObject unitBaseAlpha;
    
    /// <summary>
    /// 씬 시작시 처음 호출 되는 함수
    /// </summary>
    private void Start()
    {
        // 유닛슬롯의 하위 오브젝트들을 모두 변수에 넣음
        _unitSlots = unitSlotParent.GetComponentsInChildren<UnitSlot>();
    }

    /// <summary>
    /// 유닛슬롯 중 어느 슬롯에 유닛을 넣을 수 있는지 확인 하는 함수
    /// </summary>
    /// <param name="_unit"></param>
    public void AcquireUnit(Unit _unit)
    {
        // 변수 값이 유닛슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            // i번째의 유닛 슬롯이 비어있다면 아래 코드 실행
            if (_unitSlots[i].unit == null)
            {
                // i번쨰 스크립트에서 함수 호출
                _unitSlots[i].AddUnit(_unit);
                
                // 조건이 충족되면 해당 함수를 끝낸다
                return;
            }
        }
    }

    /// <summary>
    /// 퀵슬롯의 유닛을 클릭하였을때 호출하는 함수
    /// </summary>
    /// <param name="_unit"></param>
    public void ResetSelectUnit(Unit _unit)
    {
        // 변수 값이 유닛슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            // 유닛슬롯의 i번째 유닛의 이름과 클릭한 유닛의 이름이 같다면 아래 코드 실행
            if (_unitSlots[i].unit.unitName == _unit.unitName)
            {
                // i번째 스크립트에서 함수 호출
                _unitSlots[i].SetColor(1);
                
                // 조건이 충족되면 해당 함수를 끝낸다
                return;
            }
        }
    }

    /// <summary>
    /// 퀵슬롯 중 어느 슬롯에 유닛을 넣을 수 있는지 확인 하는 함수
    /// </summary>
    /// <param name="_unit"></param>
    public void AddQuickSlot(Unit _unit)
    {
        // 변수 값이 퀵슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
        for (int i = 0; i < quickSlot.Length; i++)
        {
            // 퀵슬롯의 i번째 유닛이 비어있다면 아래 코드 실행
            if (quickSlot[i].unit == null)
            {
                // 퀵슬롯의 
                quickSlot[i].AddUnit(_unit);
                return;
            }
            if (quickSlot[i].unit.unitName == _unit.unitName)
            {
                return;
            }
        }
    }

    public void StartStage()
    {
        if (startFlag == true)
        {
            unitBaseAlpha.SetActive(true);
           // SelectScene.SetActive(false);
            inStage = true;
        }
    }

    public void ResetBtn(int Num) // Num은 1부터 시작
    {
        if (Num != 5)
        {
            int a = Num;
            // 아래 부분 부터 구현 시작
                for (int k = 0; k < quickSlot.Length - Num; k++)
                {
                  
                    if (quickSlot[a - 1].unit == null && quickSlot[a].unit != null)
                    {
                    
                            quickSlot[a - 1].unit = quickSlot[a].unit;
                            
                            quickSlot[a].Clear();
                            quickSlot[a - 1].AddUnit(quickSlot[a - 1].unit);

                            a++;
                    }
                }
            
        }

        for (int i = 0; i < quickSlot.Length; i++)
        {
            if (quickSlot[i].unit == null)
            {
                startFlag = false;
                quickSlot[i].StartBrnColorSet();
            }
            else
            {
                return;
            }
        }
    }
}
