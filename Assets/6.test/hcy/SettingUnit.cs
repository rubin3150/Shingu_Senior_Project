using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUnit : MonoBehaviour
{
    // 현재 페이지
    private int _page = 1;
    private int maxPaheNumber;

    // 어떤 유닛이 선택되었는지 알기 위한 변수 
    private int _selectUnitNumber;

    private GameObject[] go_Slots;
    private Image[] image_Slot;
    private Text[] text_SlotName;
    private Text[] text_SlotDesc;

    private void Start()
    {
        _page = 1;
    }

    /// <summary>
    /// 
    /// </summary>
    private void PageSetting()
    {
        int startSloatNumber = (_page - 1) * go_Slots.Length;

        for (int i = startSloatNumber; i < maxPaheNumber; i++)
        {
            
        }
    }
}
