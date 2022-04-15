using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMove : MonoBehaviour
{
    public float moveSpeed;
    public float attack;
    public float attackRange;
    public float arrackDelay;

    public GameObject selectUnitBase;
    public SelectUnitBase selectUnitBaseScripts;
    
    public float nowHpStat;
    public Image maxHpStatImage;
    // public Text hpText;

    public bool isMove;

    public Unit unit;

    public LayerMask layerMask;

    public bool donMove;

    private RaycastHit2D _ray;

    private bool _isAttack;
    
    private void Update()
    {
        if (isMove == true)
        {
            if (donMove == false)
            {
                transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
            }
            
            CheckObject();
        }
    }

    private void Start()
    {
        selectUnitBase = GameObject.Find("Canvas").transform.Find("SelectUnitBase").gameObject;
        selectUnitBaseScripts = selectUnitBase.GetComponent<SelectUnitBase>();
        
        for (int i = 0; i < selectUnitBaseScripts.quickSlot.Length; i++)
        {
            if (selectUnitBaseScripts.quickSlot[i].unit != null)
            {
                if (transform.name == selectUnitBaseScripts.quickSlot[i].unit.unitPrefab.name + "(Clone)")
                {
                    unit = selectUnitBaseScripts.quickSlot[i].unit;
                    moveSpeed = unit.speedStat;
                    nowHpStat = unit.hpStat;
                    attack = unit.attackStat;
                    attackRange = unit.attackRangeStat;
                    arrackDelay = unit.attackDelayStat;
                    isMove = true;
                }
            }
            else
            {
                return;
            }
        }
    }
    
    private void CheckObject()
    {
        _ray = Physics2D.Raycast(transform.position, Vector2.right, 150f + attackRange, layerMask);
        
        if (_ray.collider != null)
        {
            donMove = true;

            if (!_isAttack)
            {
                Attack();
            }
                
            
        }
        else
        {
            _isAttack = false;
            donMove = false;
        }
    }
    
    public void UpdateHpBar()
    {
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        maxHpStatImage.fillAmount = nowHpStat / unit.hpStat;
        
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }

    private void Attack()
    {
        _isAttack = true;
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        if (_ray.transform.tag == "Tower")
        {
            _ray.transform.GetComponent<Tower>().UpdateHpBar(attack);
        }

        yield return new WaitForSeconds(arrackDelay);
        _isAttack = false;
        
    }
}
