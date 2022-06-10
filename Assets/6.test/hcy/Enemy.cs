using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private bool isMove;
    public bool donMove;

    public float nowHpStat;
    public float maxHpStat;
    public Image maxHpStatImage;
    public float pushRange;
    public float moveSpeed;
    public float attack;
    public float attackRange;
    public float attackAddRange;
    public float pushResist;
    public float attackDelay;
    public float criRate;
    public int criDamage;
    public float mana;
    public string bossType;
    public string attackType;
    
    public LayerMask layerMask;

    public GameObject hit_Effect;

    private RaycastHit2D _ray;
    
    private RaycastHit2D _currentRay;

    private bool _isAttack;

    public Unit unit;

    private float _currentDelay;

    public Image unitImage;
    
    private Vector3 _hitPos;

    public bool isDead;
    
    [SerializeField] private BoxCollider2D boxCol;
    
    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;

    [SerializeField] private GameObject damageText;

    [SerializeField] private StageManager _stageManager;

    [SerializeField] private Image hpBackImage;

    public bool isStop;

    private int k;

    private Vector3 unitPos;

    // Start is called before the first frame update
    void Start()
    {
        _stageManager = GameObject.Find("Manager").gameObject.GetComponent<StageManager>();
        moveSpeed = unit.speedStat;
        nowHpStat = unit.hpStat;
        attack = unit.attackStat;
        attackRange = unit.attackRangeStat;
        attackAddRange = unit.attackAddRangeStat;
        attackDelay = unit.attackDelayStat;
        pushRange = unit.pushRange;
        pushResist = unit.pushResist;
        criRate = unit.criRate;
        criDamage = unit.criDamage;
        mana = unit.mpGet;
        bossType = unit.bossType;
        attackType = unit.attackType;
        isMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            if (isMove == true)
            {
                if (donMove == false)
                {
                    transform.position -= new Vector3(moveSpeed  * Time.deltaTime, 0, 0);
                }
            
                CheckObject();
                CheckAttack();

                if (_isAttack == true)
                {
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

    private void CheckObject()
    {
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left,
            attackRange + attackAddRange, layerMask);

        for (int i = 0; i < rays.Length; i++)
        {
            if (rays[i].transform.tag == "Player")
            {
                _ray = rays[i];
                
                break;
            }
            else
            {
                if (attackType != "약점")
                {
                    _ray = Physics2D.BoxCast(transform.position, new Vector2(1f, 18), 0, Vector2.left,
                        attackRange + attackAddRange, layerMask);
                }
                else
                {
                    if (i + 1 < rays.Length)
                    {
                        if (rays[i + 1].transform.tag != "Player")
                        {
                            if (rays[k].transform.GetComponent<UnitMove>().nowHpStat >=
                                rays[i + 1].transform.GetComponent<UnitMove>().nowHpStat)
                            {
                                k = i + 1;
                            }
                        }
                    }
                    else
                    {
                        _ray = rays[k];
                        k = 0;
                    }
                }
            }
        }
    }
    
    private void CheckAttack()
    {
        if (_ray.collider != null)
        {
            
            donMove = true;
            _currentRay = _ray;

            if (!_isAttack)
            {

                Attack();
            }

        }
        else
        {
            if (isStop == false)
            {
                donMove = false;
            }
            // _isAttack = false;
            
        }
    }
    
    private void Attack()
    {
        _isAttack = true;
        AttackDelay();
    }

    private void AttackDelay()
    {
        int r = Random.Range(1, 101);
        
        if (r <= criRate)
        {
            // Debug.Log("적이 치명타 공격함");
            
            float criticalDamage = attack * (criDamage * 0.01f);
            
            if (_currentRay.transform.tag == "Player")
            {
                _currentRay.transform.GetComponent<Player>().UpdateHpBar(criticalDamage);
                ShowDamageTxt(criticalDamage, true, new Vector3(0, 6.5f, 0));
                _hitPos = new Vector3(3f, 1, 0);
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                
                _unitMove.UpdateHpBar(criticalDamage, true);

                if (_unitMove.unit.unitName == "팅커벨")
                {
                    _unitMove.MoveAnim();
                    // Debug.Log("팅커벨 발견");
                    _hitPos = new Vector3(3f, 4, 0);
                    ShowDamageTxt(criticalDamage, true, new Vector3(0, 9.5f, 0));
                }
                else
                {
                    ShowDamageTxt(criticalDamage, true, new Vector3(0, 5.5f, 0));
                    _hitPos = new Vector3(1f, -1, 0);
                }

                if (pushRange - _unitMove.pushResist >= 0)
                {
                    unitPos = _currentRay.transform.position -= new Vector3(pushRange - _unitMove.pushResist, 0f, 0f);
                }
                
                if (unitPos.x < -46.25f)
                {
                    _currentRay.transform.position = new Vector3(46.25f, 0, 0);
                }
                else
                {
                    _currentRay.transform.position = unitPos;
                }

                if (_unitMove.isStop == false)
                {
                    _unitMove.StopMove();
                }
            }
        }
        else
        {
            if (_currentRay.transform.tag == "Player")
            {
                // Debug.Log(1);
                _currentRay.transform.GetComponent<Player>().UpdateHpBar(attack);

                ShowDamageTxt(attack, false, new Vector3(0, 6.5f, 0));
                
                _hitPos = new Vector3(3f, 1, 0);
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                
                _unitMove.UpdateHpBar(attack, true);

                if (_unitMove.unit.unitName == "팅커벨")
                {
                    // Debug.Log("팅커벨 발견");
                    ShowDamageTxt(attack, false, new Vector3(0, 9.5f, 0));
                    _hitPos = new Vector3(3f, 4, 0);
                }
                else
                {
                    ShowDamageTxt(attack, false, new Vector3(0, 5, 0));
                    _hitPos = new Vector3(1f, -1, 0);
                }
            }
        }

        GameObject go = Instantiate(hit_Effect, _currentRay.transform.position + _hitPos, Quaternion.identity);
        go.GetComponent<EffekseerEmitter>().Play();
        
        Destroy(go, 1.5f);
        
        // _isAttack = false;
    }

    private void ShowDamageTxt(float damage, bool cirDamage, Vector3 yPos)
    {
        GameObject damageGo = Instantiate(damageText);
        damageGo.transform.SetParent(_currentRay.transform);
        damageGo.transform.position = _currentRay.transform.position + yPos; // 일반 유닛 5.5 // 팅커벨 유닛 7.5
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

    public void UpdateHpBar(float damage)
    {
        if (isMove == true)
        {
            nowHpStat -= damage;
        
            unitImage.color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
            Invoke("ResetImageAlpha", 0.5f);
            // transform.position += new Vector3(pushRange, 0, 0);
        
            // hpText.text = nowHpStat + " / " + unit.hpStat;
            // 체력 게이지 값 설정.
            maxHpStatImage.fillAmount = nowHpStat / unit.hpStat;

            if (nowHpStat <= 0)
            {
                isDead = true;
                boxCol.enabled = false;

                _stageManager.nowMana += unit.mpGet;
                _stageManager.UpdateManaBar();
                StartCoroutine(FadeUnit());
            }
        }
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

    private void ResetImageAlpha()
    {
        unitImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
    }
}
