using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectUnitBase : MonoBehaviour
{
    // FadeIn 스크립트를 관리할 변수
    [SerializeField] private FadeIn fadeIn;
    
    // 유닛슬롯의 부모 오브젝트를 담을 변수
    [SerializeField] private GameObject unitSlotParent;
    // 유닛 세팅을 하는 씬의 알파값 이미지를 담을 변수
    [SerializeField] private GameObject unitBaseAlpha;
    
    // 왼쪽 화살표 이미지를 담을 변수
    [SerializeField] private Image leftArrow;
    [SerializeField] private Image rightArrow;
    
    // Player 스크립트를 담을 변수
    [SerializeField] private Player player;
    
    // 유닛 선택시 확인 메시지를 담을 텍스트 변수
    [SerializeField] private TextMeshProUGUI confirmationTxt;
    // 페이지 텍스트를 담을 변수
    [SerializeField] private TextMeshProUGUI pageText;
    // 플레이어 정보 텍스트를 담을 변수
    [SerializeField] private TextMeshProUGUI playerHpTxt;
    [SerializeField] private TextMeshProUGUI playerManaTxt;

    [SerializeField] private TextMeshProUGUI[] skillManaTxt;
    
    // 퀵슬롯의 스크립트를 관리할 변수 
    public UnitSlot[] quickSlot;

    // 킉슬롯에 오브젝트가 하나라도 있는지 없는지 체크하기 위한 변수
    public bool startFlag;

    // 현재 페이지 유닛슬롯의 스크립트를 관리할 변수
    private UnitSlot[] _unitSlots;
    
    // 현재 페이지를 담을 변수
    private int _page = 1;
    // 몇번째 유닛 슬롯인지 체크할 변수 
    private int _num;

    // 모든 페이지의 유닛들을 담을 변수
    [SerializeField] private Unit[] maxUnit;

    [SerializeField] private PlayerSet playerSet;

    [SerializeField] private Animator _animator;
    
    /// <summary>
    /// 씬 시작시 처음 호출 되는 함수
    /// </summary>
    private void Start()
    {
        // 유닛슬롯의 하위 오브젝트들을 모두 변수에 넣음
        _unitSlots = unitSlotParent.GetComponentsInChildren<UnitSlot>();

        for (int i = 0; i < skillManaTxt.Length; i++)
        {
            skillManaTxt[i].text = playerSet.skillMoonEnergy[i].ToString();
        }

        playerHpTxt.text = player.hpStat + "/" + player.hpStat;

        playerManaTxt.text = player.maxMana + "/" + player.maxMana;
    }

    /// <summary>
    /// 유닛슬롯 중 어느 슬롯에 유닛을 넣을 수 있는지 확인 하는 함수 (현재 페이지에 유닛을 넣을 수 있음)
    /// </summary>
    /// <param name="unit"></param>
    private void AcquireUnit(Unit unit)
    {
        // 변수 값이 유닛슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            // i번째 유닛슬롯이 비어있다면 아래 코드 실행
            if (_unitSlots[i].unit == null)
            {
                // i번쨰 유닛술롯의 스크립트에서 함수 호출
                _unitSlots[i].AddUnit(unit);
                _unitSlots[i].btn.enabled = true;
                
                // 해당 함수를 끝낸다
                return;
            }
        }
    }
    
    // -70 , 15

    /// <summary>
    /// 퀵슬롯의 유닛을 클릭하였을때 호출하는 함수
    /// </summary>
    /// <param name="unit"></param>
    public void ResetSelectUnit(Unit unit)
    {
        // 변수 값이 유닛슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            // i번쨰 유닛슬롯의 유닛이 비어있지 않다면 아래 코드 실행
            if (_unitSlots[i].unit != null)
            {
                // i번째 유닛슬롯의 유닛 이름과 클릭한 유닛의 이름이 같다면 아래 코드 실행
                if (_unitSlots[i].unit.unitName == unit.unitName)
                {
                    // i번째 유닛스롯의 스크립트에서 함수 호출
                    _unitSlots[i].SetColor(1);

                    // _unitSlots[i].btn.enabled = true;
                
                    // 해당 함수를 끝낸다
                    return;
                }
            }
            // i번쨰 유닛 슬롯의 유닛이 비어있다면 아래 코드 실행
            else
            {
                // 해당 함수를 끝낸다
                return;
            }
        }
    }

    /// <summary>
    /// 퀵슬롯 중 어느 슬롯에 유닛을 넣을 수 있는지 확인 하는 함수
    /// </summary>
    /// <param name="unit"></param>
    public void AddQuickSlot(Unit unit)
    {
        // 변수 값이 퀵슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
        for (int i = 0; i < quickSlot.Length; i++)
        {
            // i번째 퀵슬롯의 유닛이 비어있다면 아래 코드 실행
            if (quickSlot[i].unit == null)
            {
                // 퀵슬롯의 i번째 스크립트에서 함수 호출
                quickSlot[i].AddUnit(unit);
                
                // 해당 함수를 끝낸다
                return;
            }
            // i번째 퀵슬롯의 유닛이름과 클릭한 유닛의 이름이 같다면 아래 코드 실행
            if (quickSlot[i].unit.unitName == unit.unitName)
            {
                // 해당 함수를 끝낸다
                return;
            }
        }
    }

    /// <summary>
    /// 전투 시작 버튼을 눌렀을때 호출하는 함수
    /// </summary>
    public void CheckUnit()
    {
        // 변수 값이 참이라면 아래 코드 실행 (유닛이 하나라도 선택되었다면)
        if (startFlag == true)
        {
            Data.Instance.sfx.clip = Data.Instance.btnClip;
            Data.Instance.sfx.Play();
            
            // 유닛 세팅을 하는 씬의 알파값 이미지 오브젝트를 활성화 시킴
            unitBaseAlpha.SetActive(true);

            // 변수 값이 퀵슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
            for (int i = 0; i < quickSlot.Length; i++)
            {
                // i번째 퀵슬롯의 유닛이 비어있지 않다면 아래 코드 실행 (퀵슬롯의 모든 유닛이 들어가 있다면)
                if (quickSlot[i].unit != null)
                {
                    // 텍스트에 설정한 텍스트를 삽입함
                    confirmationTxt.text = "전투 스테이지를 시작하겠습니까?";
                }
                // i번째 퀵슬롯의 유닛이 있다면 아래 코드 실행 (퀵슬롯의 모든 유닛이 들어가 있지 않고 하나 이상의 유닛이 있다면)
                else
                {
                    // 텍스트에 설정한 텍스트를 삽입함
                    confirmationTxt.text = "유닛 5개가 선택되지 않았습니다.\n그래도 진행하시겠습니까?";
                    
                    // 해당 함수를 끝낸다
                    return;
                }
            }
            
        }
    }

    /// <summary>
    /// 퀵슬롯의 유닛 순서를 정렬하는 함수
    /// </summary>
    /// <param name="num"></param>
    public void QuickSlotSort(int num) // Num은 1부터 시작
    {
        // 인자 값이 5가 아니라면 아래 코드 실행 (누른 퀵슬롯의 유닛 순서가 5번째 퀵슬롯 유닛이 아니라면)
        if (num != 5)
        {
            // 변수에 인자 값을 넣음
            int a = num;

            // 변수 값이 퀵슬롯의 최대 길이 - 인자값을 계산해 계산한 값보다 크거나 같아질때까지 아래 코드 반복 실행 (누른 퀵슬롯을 기준으로 오른쪽의 퀵슬롯의 수만큼 반복)
            for (int k = 0; k < quickSlot.Length - num; k++)
            { 
                // a - 1번쨰 퀵슬롯의 유닛이 비어 있고 오른쪽 퀵슬롯에 유닛이 있다면 아래 코드 실행
                if (quickSlot[a - 1].unit == null && quickSlot[a].unit != null)
                {
                    // 현재 퀵슬롯의 유닛에 오른쪽 퀵슬롯에 있던 유닛의 정보를 넣어줌
                    quickSlot[a - 1].unit = quickSlot[a].unit;

                    quickSlot[a - 1].btn.enabled = true;
                    // 오른쪽 퀵술롯의 스크립트에서 함수 호출
                    quickSlot[a].Clear();

                    // 현재 퀵슬롯의 스크립트에서 함수 호출
                    quickSlot[a - 1].AddUnit(quickSlot[a - 1].unit);

                    // 변수에 1을 더해줌
                    a++;
                }
            }
        }

        // 변수 값이 퀵슬롯의 최대 갯수보다 크거나 같아질때까지 아래 코드 반복 실행
        for (int i = 0; i < quickSlot.Length; i++)
        {
            // i번째 퀵슬롯의 유닛이 비어있다면 아래 코드 실행 (퀵슬롯의 모든 유닛이 비어 있다면)
            if (quickSlot[i].unit == null)
            {
                // 변수에 거짓이라는 값을 넣음 (전투 시작 버튼을 누를 수 없음)
                startFlag = false;
                
                // i번째 퀵슬롯의 스크립트에서 함수 호출
                quickSlot[i].StartBtnColorSet(155f);
            }
            // i번째 퀵슬롯의 유닛이 있다면 아래 코드 실행 (퀵슬롯에 유닛이 하나라도 있다면)
            else
            {
                // 해당 함수를 끝낸다
                return;
            }
        }
    }

    /// <summary>
    /// 최대 페이지까지 유닛을 넣을 수 있음
    /// </summary>
    /// <param name="unit"></param>
    public void AddMaxUnit(Unit unit)
    {
        for (int i = 0; i < maxUnit.Length; i++)
        {
            // i번째 유닛슬롯이 비어있다면 아래 코드 실행
            if (maxUnit[i] == null)
            {
                // i번쨰 유닛술롯의 스크립트에서 함수 호출
                maxUnit[i] = unit;
                
                
                // 해당 함수를 끝낸다
                break;
            }
        }

        if (_page > 1 && maxUnit[(_page - 1) * _unitSlots.Length] != null) 
        {
            AcquireUnit(unit);
        }
        else if (_page == 1)
        {
            AcquireUnit(unit);
        }
            
    }
    
    private void PageSetting()
    {
        // 초기화 작업
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            _unitSlots[i].Clear();
            _unitSlots[i].btn.enabled = false;
        }

        _num = 0;

        int startUnitNumber = (_page - 1) * _unitSlots.Length; //24는 한페이지의 슬롯 개수 // 2페이지면 24가됨

        for (int i = startUnitNumber; i < maxUnit.Length; i++)
        {
            if (i == _page * _unitSlots.Length)
            {
                break;
            }

            if (maxUnit[i] != null)
            {
                _unitSlots[_num].unit = maxUnit[i];
                _unitSlots[_num].AddUnit(_unitSlots[_num].unit);
                _num++;
            }

        }

        CheckQuickSlotUnit();
        
    }

    private void CheckQuickSlotUnit()
    {
        for (int i = 0; i < quickSlot.Length; i++)
        {
            if (quickSlot[i].unit != null)
            {
                for (int k = 0; k < _unitSlots.Length; k++)
                {
                    if (_unitSlots[k].unit != null)
                    {
                        if (quickSlot[i].unit.unitName == _unitSlots[k].unit.unitName)
                        {
                            _unitSlots[k].SetColor(0.5f);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    public void RightPageSetting()
    {
        if (_page < maxUnit.Length / (maxUnit.Length / 2))
        {
            Data.Instance.sfx.clip = Data.Instance.btnClip;
            Data.Instance.sfx.Play();
            _page++;
            pageText.text = _page + "/" + maxUnit.Length / (maxUnit.Length / 2);
            rightArrow.color = new Color(100f / 255f, 100f / 255f, 100f / 255f, 155f / 255f);
            leftArrow.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            PageSetting();
        }
        
    }

    public void LeftPageSetting()
    {
        if (_page > 1)
        {
            Data.Instance.sfx.clip = Data.Instance.btnClip;
            Data.Instance.sfx.Play();
            _page--;
            pageText.text = _page + "/" + maxUnit.Length / (maxUnit.Length / 2);
            rightArrow.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            leftArrow.color = new Color(100f / 255f, 100f / 255f, 100f / 255f, 155f / 255f);
            PageSetting();
            
        }
    }

    /// <summary>
    /// 시작하기 버튼을 눌렀을때 호출하는 함수
    /// </summary>
    public void StartBattleBtnClick()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
        
        // 함수 호출
        fadeIn.Fade();
        // 함수 호출
        // _animator.SetTrigger("StartStage");
    }

    /// <summary>
    /// 취소하기 버튼을 눌렀을때 호출하는 함수
    /// </summary>
    public void CancelBtnClick()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
        
        // 유닛 세팅을 하는 씬의 알파값 이미지 오브젝트를 비활성화 시킴
        unitBaseAlpha.SetActive(false);
    }
}
