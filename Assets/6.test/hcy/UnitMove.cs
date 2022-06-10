using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UnitMove : MonoBehaviour
{
    public float moveSpeed;
    public float attack;
    public float attackRange;
    public float attackAddRangeStat;
    public float heal;
    public float pushResist;
    public float attackDelay;
    public float criRate;
    public int criDamage;
    public float maxHp;
    public string attackType;

    public GameObject selectUnitBase;
    public SelectUnitBase selectUnitBaseScripts;
    
    public float nowHpStat;
    public Image maxHpStatImage;

    private bool isMove;
    public bool donMove;

    public Unit unit;

    public LayerMask enemyMask;
    public LayerMask towerMask;
    public LayerMask unitMask;
    public LayerMask enemyTowerMask;

    public float pushRange;

    private RaycastHit2D _ray;
    
    private RaycastHit2D _currentRay;
    
    private bool _isAttack;

    public GameObject hit_Effect;

    // 버프 중인지 아닌지 체크할 변수
    public bool isBuff;

    public string unitType;

    public Animator animator;
    
    private float _currentDelay;

    [SerializeField] private BoxCollider2D boxCol;

    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;

    public Image unitImage;

    public SpriteRenderer[] unitImages;

    public bool isDead;
    
    [SerializeField] private GameObject damageText;

    [SerializeField] private Image hpBackImage;

    public bool isStop;
    
    private int k = 0;

    private Vector3 enemyPos;

    private void Update()
    {
        if (isDead == false)
        {
            if (isMove == true)
            {
                if (donMove == false)
                {
                    if (animator != null)
                    {
                        animator.SetBool("Move", true);
                        // animator.SetBool("Attack", false);
                    }
                
                    transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                }
            
                CheckObject();
                CheckAttack();
            
                if (_isAttack == true)
                {
                    // animator.SetBool("Attack", false);
                    _currentDelay += Time.deltaTime;

                    if (_currentDelay >= attackDelay)
                    {
                        _isAttack = false;
                        _currentDelay = 0;
                    }
                }
            }
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
                    heal = unit.healStat;
                    attackRange = unit.attackRangeStat;
                    attackAddRangeStat = unit.attackAddRangeStat;
                    attackDelay = unit.attackDelayStat;
                    pushRange = unit.pushRange;
                    pushResist = unit.pushResist;
                    unitType = unit.type;
                    criRate = unit.criRate;
                    criDamage = unit.criDamage;
                    attackType = unit.attackType;
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
        // 힐러가 아니라면 아래 코드 실행
        if (unit.type != "Healer")
        {
            RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange + attackAddRangeStat, enemyMask);

            // 앞에 몬스터가 없을 경우
            if (rays.Length == 0)
            {
                // 포탑 공격대상으로 선택
                _ray = Physics2D.BoxCast(transform.position, new Vector2(1f, 18), 0, Vector2.right,
                    attackRange + attackAddRangeStat, towerMask);
            }
            // 앞에 몬스터가 있음
            else
            {
                for (int i = 0; i < rays.Length; i++)
                {

                    if (rays[i].transform.GetComponent<Enemy>().bossType == "Boss")
                    {
                        _ray = rays[i];

                        // 보스 발견시 해당 포문 끝냄
                        break;
                    }
                    else
                    {
                        if (attackType != "약점")
                        {
                            _ray = Physics2D.BoxCast(transform.position, new Vector2(1f, 18), 0, Vector2.right,
                                attackRange + attackAddRangeStat, enemyMask);
                        }
                        // 공격 타입이 약점
                        else
                        {
                            if (i + 1 < rays.Length)
                            {
                                if (rays[i + 1].transform.GetComponent<Enemy>().bossType != "Boss")
                                {
                                    if (rays[k].transform.GetComponent<Enemy>().nowHpStat >=
                                        rays[i + 1].transform.GetComponent<Enemy>().nowHpStat)
                                    {
                                        k = i + 1;
                                    }
                                }
                            }
                            // 반복문 끝남
                            else
                            {
                                _ray = rays[k];
                                k = 0;
                            }
                        }
                    }

                }
            }
        }
        // 힐러라면 아래 코드 실행
        else
        {
            // 플레이어와 유닛이 있는 인덱스를 제외하고 검출하기
            
            
            
            
            // RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange + attackAddRangeStat, unitMask);
            //
            // // 앞에 유닛이 없을 경우
            // if (rays.Length == 1)
            // {
            //     // 포탑 공격대상으로 선택
            //     _ray = Physics2D.BoxCast(transform.position, new Vector2(1f, 18), 0, Vector2.right,
            //         attackRange + attackAddRangeStat, enemyTowerMask);
            // }
            // else
            // {
            //     for (int i = 0; i < rays.Length; i++)
            //     {
            //         // 가장 낮은 hp를 보유한 유닛 검출
            //         if (rays[i].transform.tag != "Player" && rays[i].transform.name != transform.name)
            //         {
            //             if (i + 1 < rays.Length)
            //             {
            //                 if (rays[i + 1].transform.tag != "Player" && rays[i + 1].transform.name != transform.name)
            //                 {
            //                     if (rays[k].transform.GetComponent<UnitMove>().nowHpStat >=
            //                         rays[i + 1].transform.GetComponent<UnitMove>().nowHpStat)
            //                     {
            //                         k = i + 1;
            //                     }
            //                 }
            //             }
            //             // 반복문 끝남
            //             else
            //             {
            //                 _ray = rays[k];
            //                 k = 0;
            //             }
            //         }
            //         else
            //         {
            //             k += 1;
            //         }
            //     }
            // }
            
        }
    }

    private void CheckAttack()
    {
        if (_ray.collider != null)
        {
            donMove = true;
            _currentRay = _ray;

            if (animator != null)
            {
                animator.SetBool("Move", false);
            }

            // Debug.Log("적 발견");

            if (!_isAttack)
            {
                if (unit.type != "Healer")
                {
                    Attack();
                }
                else
                {
                    Heal();
                }
            }

        }
        else
        {
            if (isStop == false)
            {
                // _isAttack = false;
                donMove = false;
            
                if (animator != null)
                {
                    animator.SetBool("Move", true);
                }
            }
            
        }
    }

    public void MoveAnim()
    {
        if (isStop == false)
        {
            donMove = false;
            
            if (animator != null)
            {
                animator.SetBool("Attack", false);
            }
        }
    }
    
    public void UpdateHpBar(float damage, bool isDamage)
    {
        if (isDamage == true && isMove == true)
        {
            if (unit.unitName == "팅커벨")
            {
                // 알파 값 이미지 조절 하기 
                for (int i = 0; i < unitImages.Length; i++)
                {
                    unitImages[i].color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
                }
                Invoke("ResetAlphaImage", 0.5f);
            }
            else
            {
                unitImage.color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
                Invoke("ResetAlphaImage", 0.5f);
            }
        }

        nowHpStat -= damage;

        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        maxHpStatImage.fillAmount = nowHpStat / maxHp;
        
        if (nowHpStat <= 0)
        {
            isDead = true;
            boxCol.enabled = false;
            if (animator != null)
            {
                animator.SetTrigger("Dead");
            }

            if (unit.unitName == "팅커벨")
            {
                StartCoroutine(FadeUnits());
            }
            else
            {
                StartCoroutine(FadeUnit());
            }
        }
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }

    private void ResetAlphaImage()
    {
        if (unit.unitName == "팅커벨")
        {
            // 알파 값 이미지 조절 하기 
            for (int i = 0; i < unitImages.Length; i++)
            {
                unitImages[i].color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            }
        }
        else
        {
            unitImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
    }

    private void ResetAttackAnim()
    {
        animator.SetBool("Attack", false);
    }
    
    private IEnumerator FadeUnits()
    {
        // 변수 초기화
        _unitAlpha = 0f;

        // 변수에 몬스터 이미지 컬러 값을 넣음
        Color alpha =  unitImages[0].color;

        // 알파 값이 255보다 작을 동안에 아래 코드 실행
        while (alpha.a < 1f)
        {
            // 실제 시간 / 설정한 딜레이 시간을 계산한 값을 변수에 넣음
            _unitAlpha += Time.deltaTime / _unitAlphaTime;

            // 알파 값 조절
            alpha.a = Mathf.Lerp(0, 1, _unitAlpha);

            for (int i = 0; i < unitImages.Length; i++)
            {
                unitImages[i].color = alpha;
            }
            hpBackImage.color = alpha;

            yield return null;
        }
        
        // 변수 초기화
        _unitAlpha = 0f;

        // 딜레이 타임 0.1f
        yield return new WaitForSeconds(0.1f);

        // 알파 값이 0보다 큰 동안에 아래 코드 실행
        while (alpha.a > 0f)
        {
            // 실제 시간 / 설정한 딜레이 시간을 계산한 값을 변수에 넣음
            _unitAlpha += Time.deltaTime / _unitAlphaTime;

            // 알파 값 조절
            alpha.a = Mathf.Lerp(1, 0, _unitAlpha);

            // 조절한 알파 값을 이미지의 컬러 값에 넣음
            for (int i = 0; i < unitImages.Length; i++)
            {
                unitImages[i].color = alpha;
            }
            hpBackImage.color = alpha;
            
            yield return null;
        }

        yield return null;
        Destroy(gameObject);
    }
    
    private IEnumerator FadeUnit()
    {
        // 변수 초기화
        _unitAlpha = 0f;

        // 변수에 몬스터 이미지 컬러 값을 넣음
        Color alpha =  unitImage.color;

        // 알파 값이 255보다 작을 동안에 아래 코드 실행
        while (alpha.a < 1f)
        {
            // 실제 시간 / 설정한 딜레이 시간을 계산한 값을 변수에 넣음
            _unitAlpha += Time.deltaTime / _unitAlphaTime;

            // 알파 값 조절
            alpha.a = Mathf.Lerp(0, 1, _unitAlpha);

            unitImage.color = alpha;
            hpBackImage.color = alpha;

            yield return null;
        }
        
        // 변수 초기화
        _unitAlpha = 0f;

        // 딜레이 타임 0.1f
        yield return new WaitForSeconds(0.1f);

        // 알파 값이 0보다 큰 동안에 아래 코드 실행
        while (alpha.a > 0f)
        {
            // 실제 시간 / 설정한 딜레이 시간을 계산한 값을 변수에 넣음
            _unitAlpha += Time.deltaTime / _unitAlphaTime;

            // 알파 값 조절
            alpha.a = Mathf.Lerp(1, 0, _unitAlpha);

            // 조절한 알파 값을 이미지의 컬러 값에 넣음
            unitImage.color = alpha;
            hpBackImage.color = alpha;
            
            yield return null;
        }

        yield return null;
        Destroy(gameObject);
    }

    private void Attack()
    { 
        _isAttack = true;
        if (animator != null)
        {
            animator.SetBool("Attack", true);
            Invoke("ResetAttackAnim", 0.5f);
            Invoke("AttackDelay", 0.25f);
        }
        else
        {
            AttackDelay();
        }
    }

    private void Heal()
    {
        _isAttack = true;
        animator.SetBool("Attack", false);
        
        if (_currentRay.transform.tag != "Enemy" && _currentRay.transform.tag != "Tower")
        {
            if (animator != null)
            {
                animator.SetBool("Attack", true);
                Invoke("ResetAttackAnim", 0.5f);
                Invoke("HealDelay", 0.25f);
            }
            else
            {
                HealDelay();
            }
        }
    }

    private void HealDelay()
    {
        Debug.Log(_currentRay.transform.name);
        UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
        _unitMove.nowHpStat += unit.healStat;

        if (_unitMove.nowHpStat > _unitMove.maxHp)
        {
            _unitMove.nowHpStat = _unitMove.maxHp;
        }
        
        if (_unitMove.isDead == false)
        {
            if (_unitMove.unit.unitName == "팅커벨")
            {
                ShowDamageTxtUnit(unit.healStat, new Vector3(0, 9.5f, 0));
            }
            else
            {
                ShowDamageTxtUnit(unit.healStat, new Vector3(0, 5.5f, 0));
            }
            
            _unitMove.UpdateHpBar(0, false);
        }
    }
    
    private void ShowDamageTxtUnit(float damage, Vector3 yPos)
    {
        GameObject damageGo = Instantiate(damageText);
        damageGo.transform.SetParent(transform);
        damageGo.transform.position = transform.position + yPos; // 일반 유닛 5.5 // 팅커벨 유닛 7.5
        
        damageGo.GetComponent<DamageText>().text.color = Color.green;
        
        damageGo.GetComponent<DamageText>().damage = damage;
        
        damageGo.GetComponent<DamageText>().isCri = false;
    }

    private void AttackDelay()
    {
        int r = Random.Range(1, 101);

        if (r <= criRate)
        {
            // Debug.Log("유닛이 치명타 ");
            float criticalDamage = attack * (criDamage * 0.01f);
            
            if (_currentRay.transform.tag == "Tower")
            {
                ShowDamageTxt(criticalDamage, true, new Vector3(0, 7.5f, 0));
                _ray.transform.GetComponent<Tower>().UpdateHpBar(criticalDamage);
            }
            else if (_currentRay.transform.tag == "Enemy")
            {
                Enemy _enemy = _currentRay.transform.GetComponent<Enemy>();
                
                ShowDamageTxt(criticalDamage, true, new Vector3(0, 4, 0));
                
                _enemy.UpdateHpBar(criticalDamage);

                if (pushRange - _enemy.pushResist >= 0)
                {
                    enemyPos = _currentRay.transform.position += new Vector3(pushRange - _enemy.pushResist, 0f, 0f);
                }

                if (enemyPos.x > 38.5f)
                {
                    _currentRay.transform.position = new Vector3(38.75f, 0, 0);
                }
                else
                {
                    _currentRay.transform.position = enemyPos;
                }

                if (_enemy.isStop == false)
                {
                    _enemy.StopMove();
                }
            }
        }
        else
        {
            if (_currentRay.transform.tag == "Tower")
            {
                ShowDamageTxt(attack, false, new Vector3(0, 7.5f, 0));
                _currentRay.transform.GetComponent<Tower>().UpdateHpBar(attack);
            }
            else if (_currentRay.transform.tag == "Enemy")
            {
                Enemy _enemy = _currentRay.transform.GetComponent<Enemy>();
                
                if (_enemy.unit.unitName == "달팽이")
                {
                    ShowDamageTxt(attack, false, new Vector3(0, 2, 0));
                }
                else if (_enemy.unit.unitName == "유령")
                {
                    ShowDamageTxt(attack, false, new Vector3(0, 4, 0));
                }
                else
                {
                    ShowDamageTxt(attack, false, new Vector3(0, 1, 0));
                }
                
                _enemy.UpdateHpBar(attack);
            }
        }
        
        GameObject go = Instantiate(hit_Effect, _currentRay.transform.position - new Vector3(3.5f, 0, 0), Quaternion.identity);
        go.GetComponent<EffekseerEmitter>().Play();
        Destroy(go, 1.5f);
    }

    private void ShowDamageTxt(float damage, bool cirDamage, Vector3 yPos)
    {
        GameObject damageGo = Instantiate(damageText);
        damageGo.transform.SetParent(_currentRay.transform);
        damageGo.transform.position = _currentRay.transform.position + yPos;
        damageGo.GetComponent<DamageText>().text.color = Color.red;
        damageGo.GetComponent<DamageText>().damage = damage;
        if (cirDamage == true)
        {
            // 알파 값 조절
            damageGo.GetComponent<DamageText>().isCri = true;
        }
        else
        {
            damageGo.GetComponent<DamageText>().isCri = false;
        }
    }
    
    public void StopMove()
    {
        isStop = true;
        donMove = true;
        Invoke("DonStopMove", 1f);
    }

    private void DonStopMove()
    {
        isStop = false;
        donMove = false;
    }
}
