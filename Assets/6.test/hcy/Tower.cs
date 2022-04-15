using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float maxTowerHp;
    public float towerHp;

    public Image towerHpImage;

    private void Start()
    {
        towerHp = maxTowerHp;
    }

    public void UpdateHpBar(float damage)
    {
        towerHp -= damage;
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        towerHpImage.fillAmount = towerHp / maxTowerHp;

        if (towerHp <= 0)
        {
            Dead();
        }
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
