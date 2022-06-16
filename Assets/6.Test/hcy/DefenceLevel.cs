using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DefenceLevel : MonoBehaviour
{
    [Tooltip("유령이 레벨별 올라가는 hp 스텟")]public float[] ghostLevelHp;
    [Tooltip("달팽이가 레벨별 올라가는 hp 스텟")]public float[] snailLevelHp;
    [Tooltip("레드 큐브 슬라임이 레벨별 올라가는 hp 스텟")]public float[] redSlimeLevelHp;
    [Tooltip("그린 큐브 슬라임이 레벨별 올라가는 hp 스텟")]public float[] greenSlimeLevelHp;
    [Tooltip("블루 큐브 슬라임이 레벨별 올라가는 hp 스텟")]public float[] blueSlimeLevelHp;
    [Tooltip("유령이 레벨별 감소되는 스폰 쿨타임")] public float[] ghostLevelCoolTime;
    [Tooltip("달팽이가 레벨별 감소되는 스폰 쿨타임")] public float[] snailLevelCoolTime;
    [Tooltip("슬라임들이 레벨별 감소되는 스폰 쿨타임")] public float[] slimeLevelCoolTime;

    public bool[] isLevel;

    [SerializeField] private TextMeshProUGUI showtxt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            A(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            A(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            A(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            A(3);
        }
    }

    private void A(int k)
    {
        for (int i = 0; i < isLevel.Length; i++)
        {
            isLevel[i] = false;
        }

        isLevel[k] = true;
        showtxt.text = "현재 스테이지 레벨 : " + (k + 1);
    }
}
