using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int skillNum;

    [SerializeField] private SkillManager skillManager;
    
    [SerializeField] private PlayerSet playerSet;
    
    private int _currentSkillNum;
    
    private void Start()
    {
        _currentSkillNum = skillNum + playerSet.playerNum * 3;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillManager.isActive[skillNum] == true)
        {
            skillManager.ShowSkillToolTip(transform.position, _currentSkillNum, skillNum);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillManager.HideSkillToolTip();
    }
}
