using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;
using UnityEngine.Rendering.PostProcessing;
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
    public float skillCoolTime;
    public int skillIndex;
    public int skillEffect;
    
    public LayerMask layerMask;

    public GameObject hit_Effect;

    private RaycastHit2D _ray;
    
    private RaycastHit2D _currentRay;

    private bool _isAttack;
    
    // 스킬이 사용중인지 아닌지 체크하기 위한 변수
    private bool _isSkill;

    public Unit unit;

    private float _currentDelay;
    public float _skillDelay;

    public Image unitImage;

    public bool isDead;
    
    [SerializeField] private BoxCollider2D boxCol;
    
    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;

    [SerializeField] private StageManager _stageManager;

    [SerializeField] private Image hpBackImage;

    public bool isStop;

    private int k;

    [SerializeField] private DefenceLevel _defenceLevel;
    
    [SerializeField] private UnitSkillManager _unitSkillManager;
    
    private bool _firstCheck;
    
    private Vector3 first;

    public GameObject damageText;

    private bool backHpHit;

    public int nowDamagePos;

    public List<GameObject> damageTexts = new List<GameObject>();
    
    [SerializeField] private GameObject attackIcon;
    
    [SerializeField] private EffekseerEmitter attackAnim;
    
    public EffekseerEmitter[] skillAnim;
    
    // 스킬 마다 다른 이펙트 설정
    public EffekseerEmitter[] skillShowEffect;

    [SerializeField] public Image skillCoolTimeImage;

    // 출혈증인지 아닌지 체크할 변수 
    public bool isHurt;
    // 출혈 유지 시간
    public float hurtMaintainTime;
    // 출혈 동안 몇초나 지났는지 체크할 변수
    public float currentHurtDamageTime;
    public bool isTaunt;
    // 도발 유지 시간
    public float tauntMaintainTime;
    
    // 출혈증인지 아닌지 체크할 변수 
    public bool isBlind;
    // 출혈 유지 시간
    public float blindMaintainTime;
    
    public bool isAppleBuff;

    public bool isStun;
    public float stunMaintainTime;

    private int countRan;

    private bool isUpgrade;
    private float upgradeMaintainTime;

    private bool isAttackAnim;

    private float currentAttackAnim;
    private Vector3 endPos;
    
    private Vector3 startPos;
    private bool roseWindAttackAnim;
    private float times;

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
        skillCoolTime = unit.skillCoolTime;
        skillIndex = unit.skillIndex;
        skillEffect = unit.skillEffect;
        isMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttackAnim == true)
        {
            if (_currentRay.transform.position != null)
            {
                currentAttackAnim += Time.deltaTime * 3;
                
                if (isUpgrade == false)
                {
                    attackAnim.transform.position = Vector3.Lerp(transform.position, endPos, currentAttackAnim);
                }
                else
                {
                    skillAnim[9].transform.position = Vector3.Lerp(transform.position, endPos, currentAttackAnim);
                }
                
                if (currentAttackAnim >= 1f)
                {
                    
                    attackAnim.Stop();

                    if (unit.unitName == "유령")
                    {
                        skillAnim[9].Stop();
                    }
                    
                    currentAttackAnim = 0;
                    isAttackAnim = false;
                }
            }
        }
        else if (roseWindAttackAnim == true)
        {
            if (_currentRay.transform.position != null)
            {
                times += Time.deltaTime * 2;
                skillAnim[4].transform.position = Vector3.Lerp(startPos, _currentRay.transform.position, times);

                if (times >= 1f)
                {
                    skillAnim[4].Stop();
                    times = 0;
                    roseWindAttackAnim = false;
                }
            }
        }

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
                if (donMove == false && isStun == false)
                {
                    transform.position -= new Vector3(moveSpeed  * Time.deltaTime, 0, 0);
                }
            
                CheckObject();
                CheckAttack();
                Hurt();
                Taunt();
                Blind();
                Stun();
                UpGrade();
                    
                if (_isAttack == true)
                {
                    _currentDelay += Time.deltaTime;
                    
                    skillCoolTimeImage.fillAmount = _skillDelay / skillCoolTime;

                    
                    if (isBlind == false)
                    {
                        _skillDelay += Time.deltaTime;

                        
                        if (_skillDelay >= skillCoolTime)
                        {
                            _isSkill = true;
                        }
                    }
                    
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
        else if (isTaunt == false)
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
        else if (isTaunt == true)
        {
            for (int i = 0; i < rays.Length; i++)
            {
                if (rays[i].transform.tag == "Unit")
                {
                    if (rays[i].transform.GetComponent<UnitMove>().isTaunt == true)
                    {
                        _ray = rays[i];

                        break;
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

                if (_isAttack == false)
                {
                    if (_isSkill == true)
                    {
                        UseSkill();
                    }
                    else
                    {
                        Attack();
                    }
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

    private void UseSkill()
    {
        _skillDelay = 0;
        _isAttack = true;
        _isSkill = false;
        _firstCheck = false;

        if (isStun == false)
        {
            if (skillIndex == 3)
            {
                SwingSword();
            }
            else if (skillIndex == 4)
            {
                WindRose();
            }
            else if (skillIndex == 7)
            {
                // 달팽이 스킬
                Butt();
            }
            else if (skillIndex == 8)
            {
                // 슬라임 스킬
                SlimeSkill();
            }
            else if (skillIndex == 9)
            {
                UpGradeAttack();
            }
        }
    }
    
    private void HideAttackIcon()
    {
        attackIcon.SetActive(false);
    }

    private void Attack()
    {
        _isAttack = true;
        attackIcon.SetActive(true);
        Invoke("HideAttackIcon", 0.5f);

        if (attackAnim != null)
        {
            if (unit.unitName != "유령")
            {
                attackAnim.Play();
                
                if (_currentRay.transform.tag == "Unit")
                {
                    if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
                    {
                        attackAnim.transform.position = _currentRay.transform.position + new Vector3(0, 3.5f, 0);
                    }
                    else if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "나나")
                    {
                        attackAnim.transform.position = _currentRay.transform.position - new Vector3(0, 2, 0);
                    }
                    else
                    {
                        attackAnim.transform.position = _currentRay.transform.position;
                    }
                }
                else
                {
                    attackAnim.transform.position = _currentRay.transform.position;
                }
            }
            else
            {
                // if (_currentRay.transform.tag == "Unit")
                // {
                //     if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
                //     {
                //         endPos = _currentRay.transform.position + new Vector3(0, 3.5f, 0);
                //     }
                //     else if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "나나")
                //     {
                //         endPos = _currentRay.transform.position - new Vector3(0, 2, 0);
                //     }
                //     else
                //     {
                //         endPos = _currentRay.transform.position;
                //     }
                // }
                // else
                // {
                //     endPos = _currentRay.transform.position;
                // }
                //
                // if (isUpgrade == false)
                // {
                //     attackAnim.Play();
                // }
                // else
                // {
                //     skillAnim[9].Play();
                // }
                // isAttackAnim = true;
            }
        }
        
        if (isBlind == true)
        {
            if (_currentRay.transform.tag == "Player")
            {
                Player _player = _currentRay.transform.GetComponent<Player>();
                
                _player.UpdateHpBar(0);
                ShowDamageTxt(_player.transform, "0", false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);
            }
            else
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                
                _unitMove.UpdateHpBar(0, true);
                
                ShowDamageTxt(_unitMove.transform, "0", false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.red);
            }
        }
        else if (isBlind == false || isStun == false)
        {
            AttackDelay();
        }
    }

    private void AttackDelay()
    {
        int r = Random.Range(1, 101);
        
        if (r <= criRate)
        {
            if (isUpgrade == true)
            {
                attack = Mathf.RoundToInt(_unitSkillManager.upgradeAttackStat[0] + (attack * _unitSkillManager.upgradeAttackStat[1 ]* 0.01f));
            }
            else
            {
                attack = unit.attackStat;
            }
            int criticalDamage = Mathf.RoundToInt(attack * (criDamage * 0.01f));

            if (_currentRay.transform.tag == "Player")
            {
                Player _player = _currentRay.transform.GetComponent<Player>();
                
                if (_player.isBlind == true)
                {
                    criticalDamage = Mathf.RoundToInt(criticalDamage + (criticalDamage * _unitSkillManager.blindStat[1] * 0.01f));
                }

                if (_player.isHurt == false)
                {
                    _player.UpdateHpBar(criticalDamage);
                    ShowDamageTxt(_player.transform, criticalDamage.ToString(), true, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);
                }
                else
                {
                    int hurtDamage = Mathf.RoundToInt(criticalDamage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _player.UpdateHpBar(criticalDamage + hurtDamage);
                    
                    ShowDamageTxt(_player.transform, criticalDamage.ToString(), true, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);
                    ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
                
                if (isUpgrade == true)
                {
                    SetSkillEffect(null, _player, false);
                }
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                
                if (_unitMove.isBlind == true)
                {
                    criticalDamage = Mathf.RoundToInt(criticalDamage + (criticalDamage * _unitSkillManager.blindStat[1] * 0.01f));
                }
                
                if (_unitMove.unit.unitName == "팅커벨")
                {
                    _unitMove.MoveAnim();
                }

                // 출혈중이 아니라면 아래 코드 실행
                if (_unitMove.isHurt == false)
                {
                    _unitMove.UpdateHpBar(criticalDamage, true);
                
                    ShowDamageTxt(_unitMove.transform, criticalDamage.ToString(), true, _unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                }
                // 출혈중이라면 아래 코드 실행
                else
                {
                    int hurtDamage = Mathf.RoundToInt(criticalDamage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _unitMove.UpdateHpBar(hurtDamage + criticalDamage, true);
                
                    ShowDamageTxt(_unitMove.transform, criticalDamage.ToString(),true, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.red);
                    ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
                
                if (isUpgrade == true)
                {
                    SetSkillEffect(_unitMove, null, true);
                }
                
                if (_unitMove.pushResist - pushRange < 0)
                {
                    _unitMove.transform.position += new Vector3(_unitMove.pushResist - pushRange, 0f, 0f);
                }
                
                if (_unitMove.transform.position.x < -46.25f)
                {
                    _unitMove.transform.position = new Vector3(46.25f, _unitMove.transform.position.y, 0);
                }

                if (_unitMove.isStop == false)
                {
                    _unitMove.StopMove();
                }
            }
            
            if (_currentRay.transform.tag == "Unit")
            {
                if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
                {
                    endPos = _currentRay.transform.position + new Vector3(0, 3.5f, 0);
                }
                else if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "나나")
                {
                    endPos = _currentRay.transform.position - new Vector3(0, 2, 0);
                }
                else
                {
                    endPos = _currentRay.transform.position;
                }
            }
            else
            {
                endPos = _currentRay.transform.position;
            }
                
            if (isUpgrade == false)
            {
                attackAnim.Play();
            }
            else
            {
                skillAnim[9].Play();
            }

            if (unit.unitName == "유령" || unit.unitName == "벨")
            {
                isAttackAnim = true;
            }
            
        }
        else
        {
            if (isUpgrade == true)
            {
                attack = Mathf.RoundToInt(_unitSkillManager.upgradeAttackStat[0] + (attack * _unitSkillManager.upgradeAttackStat[1 ]* 0.01f));
            }
            else
            {
                attack = unit.attackStat;
            }
            
            if (_currentRay.transform.tag == "Player")
            {
                Player _player = _currentRay.transform.GetComponent<Player>();
                
                int damage = Mathf.RoundToInt(attack);
                
                if (_player.isBlind == true)
                {
                    damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                }

                if (_player.isHurt == false)
                {
                    _player.UpdateHpBar(damage);

                    ShowDamageTxt(_player.transform, damage.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);
                }
                else
                {
                    int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _player.UpdateHpBar(damage + hurtDamage);

                    ShowDamageTxt(_player.transform, damage.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);

                    ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
                
                if (isUpgrade == true)
                {
                    SetSkillEffect(null, _player, false);
                }
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();

                int damage = Mathf.RoundToInt(attack);
                
                if (_unitMove.isBlind == true)
                {
                    damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                }
                
                if (_unitMove.isHurt == false)
                {
                    _unitMove.UpdateHpBar(damage, true);
                
                    ShowDamageTxt(_unitMove.transform, damage.ToString(), false,_unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                }
                else
                {
                    int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _unitMove.UpdateHpBar(hurtDamage+ damage, true);
                
                    ShowDamageTxt(_unitMove.transform, damage.ToString(),false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.red);
                    ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
                
                if (isUpgrade == true)
                {
                    SetSkillEffect(_unitMove, null, true);
                }
            }
            
            if (_currentRay.transform.tag == "Unit")
            {
                if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
                {
                    endPos = _currentRay.transform.position + new Vector3(0, 3.5f, 0);
                }
                else if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "나나")
                {
                    endPos = _currentRay.transform.position - new Vector3(0, 2, 0);
                }
                else
                {
                    endPos = _currentRay.transform.position;
                }
            }
            else
            {
                endPos = _currentRay.transform.position;
            }
                
            if (isUpgrade == false)
            {
                attackAnim.Play();
            }
            else
            {
                skillAnim[9].Play();
            }
            if (unit.unitName == "유령" || unit.unitName == "벨")
            {
                isAttackAnim = true;
            }
        }

        if (_currentRay.transform.tag == "Unit")
        {
            if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
            {
                GameObject go = Instantiate(hit_Effect, _currentRay.transform.position + new Vector3(0, 3.5f, 0),
                    Quaternion.identity);
                go.GetComponent<EffekseerEmitter>().Play();
                Destroy(go, 1.5f);
            }
            else if (_currentRay.transform.GetComponent<UnitMove>().unit.unitName == "나나")
            {
                GameObject go = Instantiate(hit_Effect, _currentRay.transform.position - new Vector3(0, 2f, 0),
                    Quaternion.identity);
                go.GetComponent<EffekseerEmitter>().Play();
                Destroy(go, 1.5f);
            }
            else
            {
                GameObject go = Instantiate(hit_Effect, _currentRay.transform.position, Quaternion.identity);
                go.GetComponent<EffekseerEmitter>().Play();
                Destroy(go, 1.5f);
            }
        }
        else
        {
            GameObject go = Instantiate(hit_Effect, _currentRay.transform.position, Quaternion.identity);
            go.GetComponent<EffekseerEmitter>().Play();
            Destroy(go, 1.5f);
        }
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

    public void UpdateHpBar(float damage, bool isDamage)
    {
        if (isMove == true && isDamage == true)
        {
            unitImage.color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
            Invoke("ResetImageAlpha", 0.5f);
        }
        
        nowHpStat -= damage;
        Invoke("BackHpFun", 0.5f);
        
        if (nowHpStat <= 0)
        {
            RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);
            pushResist -= _unitSkillManager.showeringStat[0];
        
            for (int i = 0; i < rays.Length; i++)
            {
                if (rays[i].transform.tag != "Player")
                {
                    UnitMove _unitMove = rays[i].transform.GetComponent<UnitMove>();

                    _unitMove.isTaunt = false;
                }
            }
                
            tauntMaintainTime = 0;
            isTaunt = false;
            
            isDead = true;
            boxCol.enabled = false;

            _stageManager.nowMana += mana;
            _stageManager.UpdateManaBar();
            StartCoroutine(FadeUnit());
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

    private void StopButtAnim()
    {
        skillAnim[7].Stop();
    }

    private void Butt()
    {
        skillAnim[7].Play();
        skillAnim[7].transform.position = _currentRay.transform.position;
        Invoke("StopButtAnim", _unitSkillManager.buttStat[3]);
        
        int damage =
            Mathf.RoundToInt(_unitSkillManager.buttStat[0] + attack * _unitSkillManager.buttStat[1] * 0.01f);

        if (_currentRay.transform.tag == "Unit")
        {
            UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
            
            if (_unitMove.isBlind == true)
            {
                damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
            }
            
            if (_unitMove.isHurt == false)
            {
                ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                _unitMove.UpdateHpBar(damage, true);
            }
            else
            {
                int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                _unitMove.UpdateHpBar(damage + hurtDamage, true);
            }
            
            SetSkillEffect(_unitMove, null, true);
        }
        else
        {
            Player _player = _currentRay.transform.GetComponent<Player>();
            
            if (_player.isBlind == true)
            {
                damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
            }
            
            if (_player.isHurt == false)
            {
                ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                _player.UpdateHpBar(damage);
            }
            else
            {
                int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                _player.UpdateHpBar(damage + hurtDamage);
            }

            SetSkillEffect(null, _player, false);
        }
        
        if (nowHpStat + _unitSkillManager.buttStat[2] >= maxHpStat)
        {
            ShowDamageTxt(transform ,(maxHpStat - nowHpStat).ToString(), false, maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.green);
            nowHpStat = maxHpStat;
        }
        else
        {
            ShowDamageTxt(transform ,_unitSkillManager.buttStat[2].ToString(), false, maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.green);
            nowHpStat += _unitSkillManager.buttStat[2];
        }
        
        if (isDead == false)
        {
            UpdateHpBar(0, false);
        }

    }

    private void SlimeSkill()
    {
        skillAnim[8].Play();
        
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);

        for (int i = 0; i < rays.Length; i++)
        {
            countRan = Random.Range(1, 216);

            if (rays[i].transform.tag == "Unit")
            {
                UnitMove _unitMove = rays[i].transform.GetComponent<UnitMove>();
                
                int damage =
                    Mathf.RoundToInt(_unitSkillManager.slimeStat[0] + attack * _unitSkillManager.slimeStat[1] * 0.01f);
                
                // 10회 공격
                if (countRan <= _unitSkillManager.slimeStat[7])
                {
                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_unitMove.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 10; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _unitMove.UpdateHpBar(damage * 10, true);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 10; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _unitMove.UpdateHpBar((damage + hurtDamage) * 10, true);
                    }
                }
                // 9회 공격
                else if (countRan <= _unitSkillManager.slimeStat[6])
                {
                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_unitMove.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 9; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _unitMove.UpdateHpBar(damage * 9, true);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 9; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _unitMove.UpdateHpBar((damage + hurtDamage) * 9, true);
                    }
                }
                // 8회 공격
                else if (countRan <= _unitSkillManager.slimeStat[5])
                {
                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_unitMove.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 8; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _unitMove.UpdateHpBar(damage * 8, true);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 8; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _unitMove.UpdateHpBar((damage + hurtDamage) * 8, true);
                    }
                }
                // 7회 공격
                else if (countRan <= _unitSkillManager.slimeStat[4])
                {
                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_unitMove.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 7; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _unitMove.UpdateHpBar(damage * 7, true);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 7; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _unitMove.UpdateHpBar((damage + hurtDamage) * 7, true);
                    }
                }
                // 6회 공격
                else if (countRan <= _unitSkillManager.slimeStat[3])
                {
                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_unitMove.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 6; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _unitMove.UpdateHpBar(damage * 6, true);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 6; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _unitMove.UpdateHpBar((damage + hurtDamage) * 6, true);
                    }
                }
                // 5회 공격
                else if (countRan <= _unitSkillManager.slimeStat[2])
                {
                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_unitMove.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 5; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _unitMove.UpdateHpBar(damage * 5, true);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 5; roopNum++)
                        {
                            ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _unitMove.UpdateHpBar((damage + hurtDamage) * 5, true);
                    }
                }
                
                SetSkillEffect(_unitMove, null, true);
            }
            else
            {
                Player _player = rays[i].transform.GetComponent<Player>();
                
                int damage =
                    Mathf.RoundToInt(_unitSkillManager.slimeStat[0] + attack * _unitSkillManager.slimeStat[1] * 0.01f);
                
                // 10회 공격
                if (countRan <= _unitSkillManager.slimeStat[7])
                {
                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_player.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 10; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _player.UpdateHpBar(damage * 10);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 10; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _player.UpdateHpBar((damage + hurtDamage) * 10);
                    }
                }
                // 9회 공격
                else if (countRan <= _unitSkillManager.slimeStat[6])
                {
                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_player.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 9; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _player.UpdateHpBar(damage * 9);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 9; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _player.UpdateHpBar((damage + hurtDamage) * 9);
                    }
                }
                // 8회 공격
                else if (countRan <= _unitSkillManager.slimeStat[5])
                {
                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_player.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 8; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _player.UpdateHpBar(damage * 8);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 8; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _player.UpdateHpBar((damage + hurtDamage) * 8);
                    }
                }
                // 7회 공격
                else if (countRan <= _unitSkillManager.slimeStat[4])
                {
                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_player.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 7; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _player.UpdateHpBar(damage * 7);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 7; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _player.UpdateHpBar((damage + hurtDamage) * 7);
                    }
                }
                // 6회 공격
                else if (countRan <= _unitSkillManager.slimeStat[3])
                {
                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_player.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 6; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _player.UpdateHpBar(damage * 6);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 6; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _player.UpdateHpBar((damage + hurtDamage) * 6);
                    }
                }
                // 5회 공격
                else if (countRan <= _unitSkillManager.slimeStat[2])
                {
                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
            
                    if (_player.isHurt == false)
                    {
                        for (int roopNum = 0; roopNum < 5; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        }
                        
                        _player.UpdateHpBar(damage * 5);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        for (int roopNum = 0; roopNum < 5; roopNum++)
                        {
                            ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                            ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                        }

                        _player.UpdateHpBar((damage + hurtDamage) * 5);
                    }
                }
                
                SetSkillEffect(null, _player, false);
            }
        }
    }

    private int deadRan;
    
    private void SwingSword()
    {
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);

        for (int i = 0; i < rays.Length; i++)
        {
            deadRan = Random.Range(1, 101);
            skillAnim[3].Play();

            if (rays[i].transform.tag == "Unit")
            {
                UnitMove _unitMove = rays[i].transform.GetComponent<UnitMove>();

                if (deadRan <= _unitSkillManager.swingSwordStat[2])
                {
                    ShowDamageTxt(_unitMove.transform, "999", false,
                        _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);

                    _unitMove.UpdateHpBar(999, true);
                }
                else
                {
                    int damage =
                        Mathf.RoundToInt(_unitSkillManager.swingSwordStat[0] +
                                         attack * _unitSkillManager.swingSwordStat[1] * 0.01f);
            
                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
                
                    // 출혈중이 아니라면 아래 코드 실행
                    if (_unitMove.isHurt == false)
                    {
                        ShowDamageTxt(_unitMove.transform, damage.ToString(), false,
                            _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
            
                        _unitMove.UpdateHpBar(damage, true);
                    }
                    // 출혈중이라면 아래 코드 실행
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
            
                        ShowDamageTxt(_unitMove.transform, damage.ToString(), false,
                            _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false,
                            _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
            
                        _unitMove.UpdateHpBar(damage + hurtDamage, true);
                    }
                }

                SetSkillEffect(_unitMove, null, true);
            }
            else
            {
                Player _player = rays[i].transform.GetComponent<Player>();

                if (deadRan <= _unitSkillManager.swingSwordStat[2])
                {
                    ShowDamageTxt(_player.transform, (_player.hpStat * 0.2f).ToString(), false,
                        _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);

                    _player.UpdateHpBar(_player.hpStat * 0.2f);
                }
                else
                {
                    int damage =
                        Mathf.RoundToInt(_unitSkillManager.swingSwordStat[0] +
                                         attack * _unitSkillManager.swingSwordStat[1] * 0.01f);
            
                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
                
                    // 출혈중이 아니라면 아래 코드 실행
                    if (_player.isHurt == false)
                    {
                        ShowDamageTxt(_player.transform, damage.ToString(), false,
                            _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
            
                        _player.UpdateHpBar(damage);
                    }
                    // 출혈중이라면 아래 코드 실행
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
            
                        ShowDamageTxt(_player.transform, damage.ToString(), false,
                            _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        ShowDamageTxt(_player.transform, hurtDamage.ToString(), false,
                            _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
            
                        _player.UpdateHpBar(damage + hurtDamage);
                    }
                }

                SetSkillEffect(null, _player, false);
            }
        }
    }
    
    private void WindRose()
    {
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);

        for (int i = 0; i < rays.Length; i++)
        {
            if (_firstCheck == false)
            {
                first = rays[0].transform.position;
                _firstCheck = true;
            }
            
            // 동일 선상에 있다면
            if (first.y == rays[i].transform.position.y || first.y == rays[i].transform.position.y + 1.25f)
            {
                if (rays[i].transform.tag == "Unit")
                {
                    UnitMove _unitMove = rays[i].transform.GetComponent<UnitMove>();
                
                    skillAnim[4].Play();
                
                    roseWindAttackAnim = true;
                    startPos = transform.position;

                    int damage = Mathf.RoundToInt(_unitSkillManager.windRoseStat[0] + attack * _unitSkillManager.windRoseStat[1] * 0.01f);

                    if (_unitMove.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
                
                    // 출혈중이 아니라면 아래 코드 실행
                    if (_unitMove.isHurt == false)
                    {
                        ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                        _unitMove.UpdateHpBar(damage, true);
                    }
                    // 출혈중이라면 아래 코드 실행
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                        _unitMove.UpdateHpBar(damage + hurtDamage, true);
                    }
                
                    SetSkillEffect(_unitMove, null, true);
                }
                else if (rays[i].transform.tag == "Player")
                {
                    Player _player = rays[i].transform.GetComponent<Player>();
                
                    skillAnim[4].Play();
                
                    roseWindAttackAnim = true;
                    startPos = transform.position;

                    int damage = Mathf.RoundToInt(_unitSkillManager.windRoseStat[0] + attack * _unitSkillManager.windRoseStat[1] * 0.01f);

                    if (_player.isBlind == true)
                    {
                        damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                    }
                
                    // 출혈중이 아니라면 아래 코드 실행
                    if (_player.isHurt == false)
                    {
                        ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                        _player.UpdateHpBar(damage);
                    }
                    // 출혈중이라면 아래 코드 실행
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                        _player.UpdateHpBar(damage + hurtDamage);
                    }
                
                    SetSkillEffect(null, _player, false);
                }
            }
        }
    }

    private void UpGradeAttack()
    {
        isUpgrade = true;
    }
    
    private void SetSkillEffect(UnitMove unitMove, Player player, bool isUnit)
    {
        if (isUnit == true)
        {
            if (skillEffect == 0)
            {
                unitMove.skillShowEffect[0].Play();
                unitMove.isHurt = true;
                unitMove.hurtMaintainTime = 0;
                unitMove.currentHurtDamageTime = 0;
            }
            else if (skillEffect == 1)
            {
                skillShowEffect[1].Play();
                unitMove.isTaunt = true;
                isTaunt = true;
                unitMove.tauntMaintainTime = 0;
            }
            else if (skillEffect == 2)
            {
                unitMove.skillShowEffect[2].Play();
                unitMove.isBlind = true;
                unitMove.blindMaintainTime = 0;
            }
        }
        else
        {
            if (skillEffect == 0)
            {
                player.skillShowEffect[0].Play();
                player.isHurt = true;
                player.hurtMaintainTime = 0;
                player.currentHurtDamageTime = 0;
            }
            else if (skillEffect == 2)
            {
                player.skillShowEffect[1].Play();
                player.isBlind = true;
                player.blindMaintainTime = 0;
            }
        }
    }

    private void UpGrade()
    {
        if (isUpgrade == true)
        {
            upgradeMaintainTime += Time.deltaTime;
            
            if (upgradeMaintainTime >= _unitSkillManager.upgradeAttackStat[2])
            {
                upgradeMaintainTime = 0;
                isUpgrade = false;
            }
        }
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
                UpdateHpBar(_unitSkillManager.hurtStat[3], true);
                ShowDamageTxt(transform, _unitSkillManager.hurtStat[3].ToString(), false, hpBackImage.transform.position + new Vector3(0, 1, 0), new Color(128f / 255f, 0f / 255f, 255f / 255f));
                currentHurtDamageTime = 0;
            }

            // 지속 시간 끝남
            if (hurtMaintainTime >= _unitSkillManager.hurtStat[0])
            {
                skillShowEffect[0].Stop();
                currentHurtDamageTime = 0;
                hurtMaintainTime = 0;
                isHurt = false;
            }
        }
    }
    
    private void Taunt()
    {
        if (isTaunt == true)
        {
            tauntMaintainTime += Time.deltaTime;
            
            RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);


            // 지속 시간 끝남
            if (tauntMaintainTime >= _unitSkillManager.tauntStat[0])
            {
                skillShowEffect[1].Stop();
                // pushResist -= _unitSkillManager.showeringStat[0];
                tauntMaintainTime = 0;
                isTaunt = false;
            }
        }
    }

    private void Blind()
    {
        if (isBlind == true)
        {
            blindMaintainTime += Time.deltaTime;
            
            if (blindMaintainTime >= _unitSkillManager.blindStat[0])
            {
                skillShowEffect[2].Stop();
                blindMaintainTime = 0;
                isBlind = false;
            }
        }
    }
    
    /// <summary>
    /// 실명
    /// </summary>
    private void Stun()
    {
        if (isStun == true)
        {
            stunMaintainTime += Time.deltaTime;
            
            if (stunMaintainTime >= _unitSkillManager.stunStat[0])
            {
                pushResist += _unitSkillManager.stunStat[1];
                skillShowEffect[3].Stop();
                stunMaintainTime = 0;
                isStun = false;
            }
        }
    }
}
