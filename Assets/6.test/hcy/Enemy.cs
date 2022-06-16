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
    public Image backSliderHp;
    public float pushRange;
    public float moveSpeed;
    public float attack;
    public float donMoveDistance;
    public float donPlayerMoveDistance;
    public float attackRange;
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

    public bool isDead;
    
    [SerializeField] private BoxCollider2D boxCol;
    
    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;

    [SerializeField] private StageManager _stageManager;

    [SerializeField] private Image hpBackImage;

    public bool isStop;

    private int k;

    private Vector3 _unitPos;

    [SerializeField] private DefenceLevel _defenceLevel;
    
    [SerializeField] private UnitSkillManager _unitSkillManager;

    public GameObject damageText;

    private bool backHpHit;

    public int nowDamagePos;

    public List<GameObject> damageTexts = new List<GameObject>();

    // 출혈증인지 아닌지 체크할 변수 
    public bool isHurt;
    // 출혈 유지 시간
    public float hurtMaintainTime;
    // 출혈 동안 몇초나 지났는지 체크할 변수
    public float currentHurtDamageTime;

    private void SetHpStat(int i)
    {
        if (unit.unitName == "유령")
        {
            maxHpStat = unit.hpStat + _defenceLevel.ghostLevelHp[i];
            nowHpStat = unit.hpStat + _defenceLevel.ghostLevelHp[i];
        }
        else if (unit.unitName == "달팽이")
        {
            maxHpStat = unit.hpStat + _defenceLevel.snailLevelHp[i];
            nowHpStat = unit.hpStat + _defenceLevel.snailLevelHp[i];
        }
        else if (unit.unitName == "레드 큐브 슬라임")
        {
            maxHpStat = unit.hpStat + _defenceLevel.redSlimeLevelHp[i];
            nowHpStat = unit.hpStat + _defenceLevel.redSlimeLevelHp[i];
        }
        else if (unit.unitName == "그린 큐브 슬라임")
        {
            maxHpStat = unit.hpStat + _defenceLevel.greenSlimeLevelHp[i];
            nowHpStat = unit.hpStat + _defenceLevel.greenSlimeLevelHp[i];
        }
        else
        {
            maxHpStat = unit.hpStat + _defenceLevel.blueSlimeLevelHp[i];
            nowHpStat = unit.hpStat + _defenceLevel.blueSlimeLevelHp[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _stageManager = GameObject.Find("Manager").gameObject.GetComponent<StageManager>();
        _defenceLevel = GameObject.Find("TestDefenceLevel").gameObject.GetComponent<DefenceLevel>();
        _unitSkillManager = GameObject.Find("Manager").transform.GetComponent<UnitSkillManager>();
        moveSpeed = unit.speedStat;

        if (_defenceLevel.isLevel[0] == true)
        {
            SetHpStat(0);
        }
        else if (_defenceLevel.isLevel[1] == true)
        {
            SetHpStat(1);
        }
        else if (_defenceLevel.isLevel[2] == true)
        {
            SetHpStat(2);
        }
        else if (_defenceLevel.isLevel[3] == true)
        {
            SetHpStat(3);
        }
        
        
        attack = unit.attackStat;
        donMoveDistance = unit.donMoveDistance;
        donPlayerMoveDistance = unit.donTowerMoveDistance;
        attackRange = unit.attackRangeStat;
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
        maxHpStatImage.fillAmount = Mathf.Lerp(maxHpStatImage.fillAmount, nowHpStat / maxHpStat, Time.deltaTime * 5f);
       
        if (backHpHit == true)
        {
            backSliderHp.fillAmount = Mathf.Lerp(backSliderHp.fillAmount, maxHpStatImage.fillAmount, Time.deltaTime * 5f);
            if (maxHpStatImage.fillAmount >= backSliderHp.fillAmount - 0.01f)
            {
                backHpHit = false;
                backSliderHp.fillAmount = maxHpStatImage.fillAmount;
            }
        }
        
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
                Hurt();

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
            attackRange, layerMask);

        if (rays.Length == 0)
        {
            // 포탑 공격대상으로 선택
            _ray = Physics2D.BoxCast(transform.position, new Vector2(1f, 18), 0, Vector2.left,
                attackRange, layerMask);
        }
        else
        {
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
                            attackRange, layerMask);
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
    }
    
    private void CheckAttack()
    {
        if (_ray.collider != null)
        {
            if (_ray.transform.tag == "Player")
            {
                if (Vector2.Distance(new Vector2(transform.position.x, 0f), new Vector2(_ray.transform.position.x, 0)) <= donPlayerMoveDistance)
                {
                    donMove = true;
                }
            }
            else
            {
                if (Vector2.Distance(new Vector2(transform.position.x, 0f), new Vector2(_ray.transform.position.x, 0)) <= donMoveDistance)
                {
                    donMove = true;
                }
            }

            if (donMove == true)
            {
                _currentRay = _ray;

                if (!_isAttack)
                {

                    Attack();
                }
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
            int criticalDamage = Mathf.RoundToInt(attack * (criDamage * 0.01f));

            if (_currentRay.transform.tag == "Player")
            {
                _currentRay.transform.GetComponent<Player>().UpdateHpBar(criticalDamage);
                ShowDamageTxt(_currentRay.transform, criticalDamage.ToString(), true, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                
                _unitMove.UpdateHpBar(criticalDamage, true);

                if (_unitMove.unit.unitName == "팅커벨")
                {
                    _unitMove.MoveAnim();
                }

                ShowDamageTxt(_currentRay.transform, criticalDamage.ToString(), true, _unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.red);

                if (_unitMove.pushResist - pushRange < 0)
                {
                    _currentRay.transform.position += new Vector3(_unitMove.pushResist - pushRange, 0f, 0f);
                }
                
                if (_unitPos.x < -46.25f)
                {
                    _currentRay.transform.position = new Vector3(46.25f, 0, 0);
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

                ShowDamageTxt(_currentRay.transform, attack.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                
                _unitMove.UpdateHpBar(attack, true);
                
                ShowDamageTxt(_currentRay.transform, attack.ToString(), false,_unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.red);
            }
        }

        GameObject go = Instantiate(hit_Effect, _currentRay.transform.position, Quaternion.identity);
        go.GetComponent<EffekseerEmitter>().Play();
        
        Destroy(go, 1.5f);
        
        // _isAttack = false;
    }

    private void ShowDamageTxt(Transform go, string damage, bool cirDamage, Vector3 yPos, Color color)
    {
        GameObject damageGo = Instantiate(damageText);
        damageGo.transform.SetParent(go);
        damageGo.GetComponent<DamageText>().parent = go.gameObject;
        
        if (go.tag == "Player")
        {
            Player _player = go.GetComponent<Player>();
            
            _player.damageTexts.Add(damageGo); 
            _player.nowDamagePos += 1;

            int currentDamagePos = _player.nowDamagePos;
            
            for (int i = 0; i < _player.damageTexts.Count; i++)
            {
                if (_player.damageTexts[i] != null)
                {
                    currentDamagePos -= 1;
                    _player.damageTexts[i].transform.position = yPos + new Vector3(0, currentDamagePos);
                }
            }
        }
        else if (go.tag == "Unit")
        {
            UnitMove _unitMove = go.GetComponent<UnitMove>();
            
            _unitMove.damageTexts.Add(damageGo); 
            _unitMove.nowDamagePos += 1;

            int currentDamagePos = _unitMove.nowDamagePos;
            
            for (int i = 0; i < _unitMove.damageTexts.Count; i++)
            {
                if (_unitMove.damageTexts[i] != null)
                {
                    currentDamagePos -= 1;
                    _unitMove.damageTexts[i].transform.position = yPos + new Vector3(0, currentDamagePos);
                }
            }
        }
        else
        {
            Enemy _enemy = go.GetComponent<Enemy>();
            
            _enemy.damageTexts.Add(damageGo); 
            _enemy.nowDamagePos += 1;

            int currentDamagePos = _enemy.nowDamagePos;
            
            for (int i = 0; i < _enemy.damageTexts.Count; i++)
            {
                if (_enemy.damageTexts[i] != null)
                {
                    currentDamagePos -= 1;
                    _enemy.damageTexts[i].transform.position = yPos + new Vector3(0, currentDamagePos);
                }
            }
        }
        
        damageGo.GetComponent<DamageText>().damage = damage;
        damageGo.GetComponent<DamageText>().text.color = color;
        
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
            Invoke("BackHpFun", 0.5f);
            nowHpStat -= damage;
        
            unitImage.color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
            Invoke("ResetImageAlpha", 0.5f);
            // transform.position += new Vector3(pushRange, 0, 0);
        
            // hpText.text = nowHpStat + " / " + unit.hpStat;
            // 체력 게이지 값 설정.
            

            if (nowHpStat <= 0)
            {
                isDead = true;
                boxCol.enabled = false;

                _stageManager.nowMana += mana;
                _stageManager.UpdateManaBar();
                StartCoroutine(FadeUnit());
            }
        }
    }

    private void BackHpFun()
    {
        backHpHit = true;
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
    }

    private void ResetImageAlpha()
    {
        unitImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
    }

    private void Hurt()
    {
        if (isHurt == true)
        {
            hurtMaintainTime += Time.deltaTime;
            currentHurtDamageTime += Time.deltaTime;
            
            // 일정 지속 시간 지나 지속 대미지를 받음
            if (currentHurtDamageTime >= _unitSkillManager.hurtStat[2])
            {
                UpdateHpBar(_unitSkillManager.hurtStat[3]);
                ShowDamageTxt(transform, _unitSkillManager.hurtStat[3].ToString(), false, hpBackImage.transform.position + new Vector3(0, 1, 0), Color.red);
                currentHurtDamageTime = 0;
            }

            // 지속 시간 끝남
            if (hurtMaintainTime >= _unitSkillManager.hurtStat[0])
            {
                currentHurtDamageTime = 0;
                hurtMaintainTime = 0;
                isHurt = false;
            }
        }
    }
}
