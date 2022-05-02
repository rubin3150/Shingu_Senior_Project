using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UnitMove : MonoBehaviour
{
    public float moveSpeed;
    public float attack;
    public float attackRange;
    public float arrackDelay;
    public int criRate;
    public int criDamage;
    public float maxHp;

    public GameObject selectUnitBase;
    public SelectUnitBase selectUnitBaseScripts;
    
    public float nowHpStat;
    public Image maxHpStatImage;
    // public Text hpText;

    public bool isMove;
    public bool donMove;

    public Unit unit;

    public LayerMask layerMask;

    public float pushRange;

    private RaycastHit2D _ray;

    private bool _isAttack;

    public GameObject hit_Effect;

    // 버프 중인지 아닌지 체크할 변수
    public bool isBuff;

    // 딜러형 버프인지 아닌지 체크할 변수 
    public bool isDealerBuff;
    
    public string unitType;

    private void Update()
    {
        if (isMove == true)
        {
            if (donMove == false)
            {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
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
                    maxHp = unit.hpStat;
                    attack = unit.attackStat;
                    attackRange = unit.attackRangeStat;
                    arrackDelay = unit.attackDelayStat;
                    pushRange = unit.pushRange;
                    unitType = unit.type;
                    criRate = unit.criRate;
                    criDamage = unit.criDamage;
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
        _ray = Physics2D.Raycast(transform.position, Vector2.right, attackRange, layerMask);
        Debug.DrawRay(transform.position + new Vector3(0, 5, 0), Vector2.right, Color.red, attackRange);
        
        if (_ray.collider != null)
        {
            donMove = true;
            Debug.Log("적 발견");

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
    
    public void UpdateHpBar(float damage)
    {
        
        nowHpStat -= damage;
        // transform.position -= new Vector3(pushRange, 0, 0);
        
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        maxHpStatImage.fillAmount = nowHpStat / maxHp;

        if (nowHpStat <= 0)
        {
            Destroy(gameObject);
        }
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        
        
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }

    private void Attack()
    {
        _isAttack = true;
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        GameObject go = Instantiate(hit_Effect, _ray.transform.position - new Vector3(3.5f, 0, 0), Quaternion.identity);
        if (_ray.transform.tag == "Tower")
        {
            _ray.transform.GetComponent<Tower>().UpdateHpBar(attack);
        }
        else if (_ray.transform.tag == "Enemy")
        {
            _ray.transform.GetComponent<Enemy>().UpdateHpBar(attack);
            // UpdateHpBar(_ray.transform.GetComponent<Enemy>().attack);
        }
        
        Destroy(go, 1.5f);

        yield return new WaitForSeconds(arrackDelay);
        _isAttack = false;
        
    }
}
