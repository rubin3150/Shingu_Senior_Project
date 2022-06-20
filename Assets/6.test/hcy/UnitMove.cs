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
using Effekseer;

public class UnitMove : MonoBehaviour
{
    public float moveSpeed;
    public float attack;
    public float donMoveDistance;
    public float donTowerMoveDistance;
    public float attackRange;
    public float heal;
    public float pushResist;
    public float attackDelay;
    public float criRate;
    public int criDamage;
    public float maxHp;
    public string attackType;
    public float skillCoolTime;
    public int skillIndex;
    public int skillEffect;

    public GameObject selectUnitBase;
    public SelectUnitBase selectUnitBaseScripts;
    
    public float nowHpStat;
    public Image maxHpStatImage;
    [SerializeField] private Image backSliderHp;

    private bool isMove;
    public bool donMove;

    public Unit unit;

    public LayerMask enemyMask;
    public LayerMask towerMask;
    public LayerMask unitMask;
    public LayerMask enemyTowerMask;
    public LayerMask unitEnemyMask;

    public float pushRange;

    private RaycastHit2D _ray;
    
    private RaycastHit2D _currentRay;
    
    // 공격중인지 아닌지 체크할 변수
    private bool _isAttack;
    
    // 스킬이 사용중인지 아닌지 체크하기 위한 변수
    private bool _isSkill;

    public GameObject hit_Effect;

    // 버프 중인지 아닌지 체크할 변수
    public bool isBuff;

    public string unitType;

    public Animator animator;
    
    private float _currentDelay;
    public float _skillDelay;

    [SerializeField] private BoxCollider2D boxCol;

    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;

    public Image unitImage;

    public SpriteRenderer[] unitImages;

    public bool isDead;

    [SerializeField] public Image hpBackImage;

    public bool isStop;
    
    private int k = 0;

    private int deadRan;

    [SerializeField] public Image skillCoolTimeImage;

    [SerializeField] private UnitSkillManager _unitSkillManager;

    private bool _firstCheck;

    private Vector3 first;

    private bool backHpHit;

    public GameObject damageText;
    
    public int nowDamagePos;
    
    public List<GameObject> damageTexts = new List<GameObject>();
    
    [SerializeField] private GameObject attackIcon;
    
    [SerializeField] private EffekseerEmitter attackAnim;
    
    public EffekseerEmitter[] skillAnim;
    
    // 스킬 마다 다른 이펙트 설정
    public EffekseerEmitter[] skillShowEffect;
    
    // 출혈증인지 아닌지 체크할 변수 
    public bool isHurt;
    // 출혈 유지 시간
    public float hurtMaintainTime;
    // 출혈 동안 몇초나 지났는지 체크할 변수
    public float currentHurtDamageTime;

    public bool isTaunt;
    // 도발 유지 시간
    public float tauntMaintainTime;

    private bool isAppleBox;
    private float appleBoxMaintainTime;
    private float currentAppleBoxTime;
    private float appleTimes;

    private bool isAttackAnim;
    private bool isMoveAttackAnim;
    private bool axeAttackAnim;
    private bool roseWindAttackAnim;
    private bool throwHedgehogAnim;
    
    // 출혈증인지 아닌지 체크할 변수 
    public bool isBlind;
    // 출혈 유지 시간
    public float blindMaintainTime;

    private float times;

    private Vector3 startPos;
    private Vector3 endPos;

    [SerializeField] private int thinkBellThrowTime;
    
    [SerializeField] private int axeThrowTime;
    
    [SerializeField] private int roseThrowTime;
    
    [SerializeField] private GameObject[] setUnits;

    public bool isAppleBuff;

    private int buffNum;
    
    public bool isStun;
    public float stunMaintainTime;
    
    [SerializeField] private Collider2D[] collider2Ds;

    private void Update()
    {
        if (isMoveAttackAnim == true)
        {
            if (_currentRay.transform.position != null)
            {
                times += Time.deltaTime * thinkBellThrowTime;
                attackAnim.transform.position = Vector3.Lerp(startPos, _currentRay.transform.position, times);

                if (times >= 1f)
                {
                    attackAnim.Stop();
                    times = 0;
                    isMoveAttackAnim = false;
                }
            }
        }
        else if (axeAttackAnim == true)
        {
            if (_currentRay.transform.position != null)
            {
                times += Time.deltaTime * axeThrowTime;
                skillAnim[0].transform.position = Vector3.Lerp(startPos, endPos, times);

                if (times >= 1f)
                {
                    skillAnim[0].Stop();
                    times = 0;
                    axeAttackAnim = false;
                }
            }
        }
        else if (isAttackAnim == true)
        {
            if (_currentRay.transform.position != null)
            {
                times += Time.deltaTime * roseThrowTime;
                attackAnim.transform.position = Vector3.Lerp(startPos, endPos, times);

                if (times >= 1f)
                {
                    attackAnim.Stop();
                    times = 0;
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
        else if (throwHedgehogAnim == true)
        {
            times += Time.deltaTime * 2;
            Vector3 _endPos = new Vector3(_currentRay.transform.position.x, 7f, _currentRay.transform.position.z);

            if (times < 1)
            {
                skillAnim[6].transform.position = Vector3.Lerp(transform.position, _endPos, times);
            }

            if (times >= 1f)
            {
                // skillAnim[6].Stop();
                times = 0;
                throwHedgehogAnim = false;
            }
        }
        
        maxHpStatImage.fillAmount = Mathf.Lerp(maxHpStatImage.fillAmount, nowHpStat / maxHp, Time.deltaTime * 5f);
       
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
                    if (animator != null)
                    {
                        animator.SetBool("Move", true);
                        // animator.SetBool("Attack", false);
                    }
                    transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                }
                
                CheckObject();
                CheckAttack();
                Hurt();
                Taunt();
                Blind();
                Stun();
                CountAppleBox();

                if (_isAttack == true)
                {
                    // animator.SetBool("Attack", false);
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

    private void Start()
    {
        selectUnitBase = GameObject.Find("Canvas").transform.Find("SelectUnitBase").gameObject;
        _unitSkillManager = GameObject.Find("Manager").transform.GetComponent<UnitSkillManager>();
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
                    donMoveDistance = unit.donMoveDistance;
                    donTowerMoveDistance = unit.donTowerMoveDistance;
                    attackRange = unit.attackRangeStat;
                    attackDelay = unit.attackDelayStat;
                    pushRange = unit.pushRange;
                    pushResist = unit.pushResist;
                    unitType = unit.type;
                    criRate = unit.criRate;
                    criDamage = unit.criDamage;
                    attackType = unit.attackType;
                    skillCoolTime = unit.skillCoolTime;
                    skillIndex = unit.skillIndex;
                    skillEffect = unit.skillEffect;
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
            RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, enemyMask);

            // 앞에 몬스터가 없을 경우
            if (rays.Length == 0)
            {
                // 포탑 공격대상으로 선택
                _ray = Physics2D.BoxCast(transform.position, new Vector2(1f, 18), 0, Vector2.right,
                    attackRange, towerMask);
            }
            // 앞에 몬스터가 있음
            else if (isTaunt == false)
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
                                attackRange, enemyMask);
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
            else if (isTaunt == true)
            {
                for (int i = 0; i < rays.Length; i++)
                {
                    if (rays[i].transform.GetComponent<Enemy>().isTaunt == true)
                    {
                        _ray = rays[i];

                        break;
                    }
                }
            }
        }
        // 힐러라면 아래 코드 실행
        else
        {
            // 플레이어와 유닛이 있는 인덱스를 제외하고 검출하기

            RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, unitMask);
            
            // 앞에 유닛이 없을 경우
            if (rays.Length == 0)
            {
                // 포탑 공격대상으로 선택
                _ray = Physics2D.BoxCast(transform.position, new Vector2(1f, 18), 0, Vector2.right,
                    2, enemyTowerMask);
            }
            else
            {
                for (int i = 0; i < rays.Length; i++)
                {
                    if (rays[i].transform.tag != "Player")
                    {
                        if (i + 1 < rays.Length)
                        {
                            // if (rays[i + 1].transform.name != transform.name)
                            // {
                                if (rays[k].transform.GetComponent<UnitMove>().nowHpStat >=
                                    rays[i + 1].transform.GetComponent<UnitMove>().nowHpStat)
                                {
                                    k = i + 1;
                                }
                            // }
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

    private void CheckAttack()
    {
        if (_ray.collider != null)
        {
            if (_ray.transform.tag == "Tower")
            {
                if (Vector2.Distance(new Vector2(transform.position.x, 0f), new Vector2(_ray.transform.position.x, 0)) <= donTowerMoveDistance)
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
                
                if (animator != null)
                {
                    animator.SetBool("Move", false);
                }

                if (_isAttack == false)
                {
                    if (_isSkill == true)
                    {
                        if (_currentRay.transform.tag == "Tower")
                        {
                            if (unit.type != "Healer")
                            {
                                Attack();
                            }
                        }
                        else
                        {
                            if (unit.type != "Healer")
                            {
                                UseSkill();
                            }
                            else
                            {
                                if (_currentRay.transform.tag == "Unit")
                                {
                                    UseSkill();
                                }
                            }
                        }
                        
                    }
                    else
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
                animator.SetBool("Move", false);
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
        Invoke("BackHpFun", 0.5f);

        if (nowHpStat <= 0)
        {
            skillAnim[5].Stop();
            // 지속 시간 끝남
            skillAnim[2].Stop();
                RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, enemyMask);
                pushResist -= _unitSkillManager.showeringStat[0];
        
                for (int i = 0; i < rays.Length; i++)
                {
                    Enemy _enemy = rays[i].transform.GetComponent<Enemy>();

                    _enemy.isTaunt = false;
                }
                
                tauntMaintainTime = 0;

                isTaunt = false;
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
    }
    
    private void BackHpFun()
    {
        backHpHit = true;
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

    private void UseSkill()
    {
        _skillDelay = 0;
        _isAttack = true;
        _isSkill = false;
        _firstCheck = false;

        if (isStun == false)
        {
            if (skillIndex == 0)
            {
                // 도끼 던지기 스킬 발동
                ThrowAxe();
            }
            else if (skillIndex == 1)
            {
                // 팅커벨 스킬
                MagicHeal();
            }
            else if (skillIndex == 2)
            {
                // 나나 스킬
                Showering();
            }
            else if (skillIndex == 3)
            {
                // 후크 선장 스킬
                SwingSword();
            }
            else if (skillIndex == 4)
            {
                // 벨 스킬
                WindRose();
            }
            else if (skillIndex == 5)
            {
                // 백설 스킬
                AppleBox();
            }
            else if (skillIndex == 6)
            {
                // 앨리스 스킬
                ThrowHedgehog();
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
            attackAnim.Play();

            if (unit.unitName == "나나")
            {
                if (_currentRay.transform.tag == "Enemy")
                {
                    if (_currentRay.transform.GetComponent<Enemy>().unit.unitName == "유령")
                    {
                        attackAnim.transform.position = _currentRay.transform.position;
                    }
                    else
                    {
                        attackAnim.transform.position = _currentRay.transform.position - new Vector3(0, 2, 0);
                    }
                }
                else
                {
                    attackAnim.transform.position = _currentRay.transform.position - new Vector3(0, 4, 0);
                }
            }
            else if (unit.unitName == "벨" || unit.unitName == "앨리스")
            {
                startPos = transform.position + new Vector3(0, -1f, 0);
                
                if (_currentRay.transform.tag == "Enemy")
                {
                    if (_currentRay.transform.GetComponent<Enemy>().unit.unitName == "유령")
                    {
                        endPos = _currentRay.transform.position;
                    }
                    else
                    {
                        endPos = _currentRay.transform.position - new Vector3(0, 2, 0);
                    }
                }
                else
                {
                    endPos = _currentRay.transform.position - new Vector3(0, 4, 0);
                }
                isAttackAnim = true;
            }
        }

        if (isBlind == true)
        {
            if (_currentRay.transform.tag == "Tower")
            {
                Tower _tower = _currentRay.transform.GetComponent<Tower>();
                
                _tower.UpdateHpBar(0);
                ShowDamageTxt(_tower.transform, "0", false, _currentRay.transform.GetComponent<Tower>().towerHpImage.transform.position + new Vector3(0, 1, 0), Color.red);
            }
            else
            {
                Enemy _enemy = _currentRay.transform.GetComponent<Enemy>();
                
                _enemy.UpdateHpBar(0, true);
                
                ShowDamageTxt(_enemy.transform, "0", false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
            }
        }
        else if (isBlind == false || isStun == false)
        {
            AttackDelay();
        }
    }

    private void Heal()
    {
        _isAttack = true;
        if (_ray.transform.tag == "Unit")
        {
            attackIcon.SetActive(true);
            Invoke("HideAttackIcon", 0.5f);
            
            if (attackAnim != null)
            {
                attackAnim.Play();

                if (unit.unitName == "팅커벨")
                {
                    startPos = transform.position + new Vector3(0, 3.5f, 0);
                    isMoveAttackAnim = true;
                    // attackAnim.transform.position = _currentRay.transform.position;
                }
                else
                {
                    startPos = transform.position + new Vector3(0, 0f, 0);
                    isMoveAttackAnim = true;
                }
            }
            
            if (animator != null)
            {
                animator.SetBool("Attack", false);
            }

            if (_currentRay.transform.tag != "Enemy" && _currentRay.transform.tag != "Tower")
            {
                if (animator != null)
                {
                    animator.SetBool("Attack", true);
                    Invoke("ResetAttackAnim", 0.5f);
                    if (isBlind == false)
                    {
                        Invoke("HealDelay", 0.25f);
                    }
                    else
                    {
                        UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                        
                        _unitMove.UpdateHpBar(0, false);
                    
                        ShowDamageTxt(_unitMove.transform, "0", false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.green);
                    }
                }
                else
                {
                    if (isBlind == false)
                    {
                        HealDelay();
                    }
                    else
                    {
                        UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                        
                        _unitMove.UpdateHpBar(0, false);
                    
                        ShowDamageTxt(_unitMove.transform, "0", false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.green);
                    }
                }
            }
        }
    }

    private void HealDelay()
    {
        UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
        if (_unitMove.nowHpStat + heal >= _unitMove.maxHp)
        {
            ShowDamageTxt(_unitMove.transform, (_unitMove.maxHp - _unitMove.nowHpStat).ToString(), false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1, 0), Color.green);
            _unitMove.nowHpStat = _unitMove.maxHp;
        }
        else
        {
            ShowDamageTxt(_unitMove.transform, heal.ToString(), false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1, 0), Color.green);
            _unitMove.nowHpStat += heal;
        }

        if (_unitMove.isDead == false)
        {
            _unitMove.UpdateHpBar(0, false);
        }
    }

    private void AttackDelay()
    {
        int r = Random.Range(1, 101);

        if (r <= criRate)
        {
            int criticalDamage =
                Mathf.RoundToInt(attack * (criDamage * 0.01f));

            if (_currentRay.transform.tag == "Tower")
            {
                ShowDamageTxt(_currentRay.transform ,criticalDamage.ToString(), true, _currentRay.transform.GetComponent<Tower>().towerHpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                _ray.transform.GetComponent<Tower>().UpdateHpBar(criticalDamage);
            }
            else if (_currentRay.transform.tag == "Enemy")
            {
                Enemy _enemy = _currentRay.transform.GetComponent<Enemy>();

                if (_enemy.isBlind == true)
                {
                    criticalDamage = Mathf.RoundToInt(criticalDamage + (criticalDamage * _unitSkillManager.blindStat[1] * 0.01f));
                }

                // 출혈중이 아니라면 아래 코드 실행
                if (_enemy.isHurt == false)
                {
                    ShowDamageTxt(_enemy.transform, criticalDamage.ToString(), true, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                    _enemy.UpdateHpBar(criticalDamage, true);
                }
                // 출혈중이라면 아래 코드 실행
                else
                {
                    int hurtDamage = Mathf.RoundToInt(criticalDamage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _enemy.UpdateHpBar(hurtDamage + criticalDamage, true);
                
                    ShowDamageTxt(_enemy.transform, criticalDamage.ToString(),true, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.red);
                    ShowDamageTxt(_enemy.transform, hurtDamage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }

                if (_enemy.transform.GetComponent<Enemy>().bossType != "Boss")
                {
                    if (_enemy.pushResist - pushRange < 0)
                    {
                        _enemy.transform.position -= new Vector3(_enemy.pushResist - pushRange, 0f, 0f);
                    }

                    if (_enemy.transform.position.x > 49.5f)
                    {
                        _enemy.transform.position = new Vector3(49.5f, _enemy.transform.position.y, 0);
                    }

                    if (_enemy.isStop == false)
                    {
                        _enemy.StopMove();
                    }
                }
            }
        }
        else
        {
            if (_currentRay.transform.tag == "Tower")
            {
                ShowDamageTxt(_currentRay.transform, attack.ToString(), false, _currentRay.transform.GetComponent<Tower>().towerHpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                _currentRay.transform.GetComponent<Tower>().UpdateHpBar(attack);
            }
            else if (_currentRay.transform.tag == "Enemy")
            {
                Enemy _enemy = _currentRay.transform.GetComponent<Enemy>();

                int damage = Mathf.RoundToInt(attack);
                
                if (_enemy.isBlind == true)
                {
                    damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                }
                
                // 출혈중이 아니라면 아래 코드 실행
                if (_enemy.isHurt == false)
                {
                    ShowDamageTxt(_enemy.transform, damage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0),Color.red);

                    _enemy.UpdateHpBar(damage, true);
                }
                // 출혈중이라면 아래 코드 실행
                else
                {
                    int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _enemy.UpdateHpBar(hurtDamage+ damage, true);
                
                    ShowDamageTxt(_enemy.transform, damage.ToString(),false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.red);
                    ShowDamageTxt(_enemy.transform, hurtDamage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
            }
        }
        
        if (_currentRay.transform.tag == "Enemy")
        {
            if (_currentRay.transform.GetComponent<Enemy>().unit.unitName == "유령")
            {
                GameObject go = Instantiate(hit_Effect, _currentRay.transform.position, Quaternion.identity);
                go.GetComponent<EffekseerEmitter>().Play();
                Destroy(go, 1.5f);
            }
            else
            {
                GameObject go = Instantiate(hit_Effect, _currentRay.transform.position - new Vector3(0, 2, 0), Quaternion.identity);
                go.GetComponent<EffekseerEmitter>().Play();
                Destroy(go, 1.5f);
            }
        }
        else
        {
            GameObject go = Instantiate(hit_Effect, _currentRay.transform.position - new Vector3(0, 4, 0), Quaternion.identity);
            go.GetComponent<EffekseerEmitter>().Play();
            Destroy(go, 1.5f);
        }
    }

    private void ShowDamageTxt(Transform go, string damage, bool cirDamage, Vector3 yPos, Color color)
    {
        GameObject damageGo = Instantiate(damageText);
        damageGo.transform.SetParent(go);
        damageGo.GetComponent<DamageText>().parent = go.gameObject;
        
        if (go.tag == "Tower")
        {
            Tower _tower = go.GetComponent<Tower>();
            
            _tower.damageTexts.Add(damageGo); 
            _tower.nowDamagePos += 1;

            int currentDamagePos = _tower.nowDamagePos;
            
            for (int i = 0; i < _tower.damageTexts.Count; i++)
            {
                if (_tower.damageTexts[i] != null)
                {
                    currentDamagePos -= 1;
                    _tower.damageTexts[i].transform.position = yPos + new Vector3(0, currentDamagePos);
                }
            }
        }
        else if (go.tag == "Enemy")
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
        else
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

    private void ThrowAxe()
    {
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, enemyMask);

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
                rays[i].transform.position = new Vector3(first.x + _unitSkillManager.throwAxeStat[0], first.y, 0);

                Enemy _enemy = rays[i].transform.GetComponent<Enemy>();
                
                skillAnim[0].Play();

                axeAttackAnim = true;
                startPos = transform.position;
                
                if (_enemy.transform.position.x > 49.5f)
                {
                    endPos = new Vector3(49.5f, _enemy.transform.position.y, 0);
                    _enemy.transform.position = new Vector3(49.5f, _enemy.transform.position.y, 0);
                }
                else
                {
                    endPos = new Vector3(first.x + _unitSkillManager.throwAxeStat[0], first.y, 0);
                }
                
                int damage =
                    Mathf.RoundToInt(_unitSkillManager.throwAxeStat[1] + attack * _unitSkillManager.throwAxeStat[2] * 0.01f);
                    
                if (_enemy.isBlind == true)
                {
                    damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                }
                
                // 출혈중이 아니라면 아래 코드 실행
                if (_enemy.isHurt == false)
                {
                    ShowDamageTxt(_enemy.transform ,damage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                    _enemy.UpdateHpBar(damage, true);
                }
                // 출혈중이라면 아래 코드 실행
                else
                {
                    int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    ShowDamageTxt(_enemy.transform ,damage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                    ShowDamageTxt(_enemy.transform, hurtDamage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                    _enemy.UpdateHpBar(damage + hurtDamage, true);
                }

                SetSkillEffect(_enemy);
                
                if (_enemy.isStop == false)
                {
                    _enemy.StopMove();
                }
            }
        }
    }

    private void StopHealAnim()
    {
        skillAnim[1].Stop();
    }
    
    private void StopShoweringAnim()
    {
        skillAnim[2].Stop();
    }
    
    private void SetSkillEffect(Enemy enemy)
    {
        if (skillEffect == 0)
        {
            enemy.skillShowEffect[0].Play();
            enemy.isHurt = true;
            enemy.hurtMaintainTime = 0;
            enemy.currentHurtDamageTime = 0;
        }
        else if (skillEffect == 1)
        {
            skillShowEffect[1].Play();
            enemy.isTaunt = true;
            isTaunt = true;
            enemy.tauntMaintainTime = 0;
        }
        else if (skillEffect == 2)
        {
            enemy.skillShowEffect[2].Play();
            enemy.isBlind = true;
            enemy.blindMaintainTime = 0;
        }
        else if (skillEffect == 3)
        {
            enemy.skillShowEffect[3].Play();
            enemy.isStun = true;
            enemy.pushResist -= _unitSkillManager.stunStat[1];
            enemy.stunMaintainTime = 0;
        }
    }

    // 팅커벨 스킬
    private void MagicHeal()
    {
        skillAnim[1].Play();
        skillAnim[1].transform.SetParent(_currentRay.transform);
        skillAnim[1].transform.position = _currentRay.transform.position - new Vector3(0, 2, 0);
        Invoke("StopHealAnim", _unitSkillManager.magicHealStat[1]);
        
        UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();

        if (_unitMove != null)
        {
            _unitMove._skillDelay += _unitSkillManager.magicHealStat[0];
            _unitMove.skillCoolTimeImage.fillAmount = _unitMove._skillDelay / _unitMove.skillCoolTime;
            ShowDamageTxt(_unitMove.transform, (_unitMove.maxHp - _unitMove.nowHpStat).ToString(), false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1, 0), Color.green);
            _unitMove.nowHpStat = _unitMove.maxHp;
            _unitMove.UpdateHpBar(0, false);
        }
    }

    /// <summary>
    /// 나나 스킬
    /// </summary>
    private void Showering()
    {
        skillAnim[2].Play();
        Invoke("StopShoweringAnim", _unitSkillManager.showeringStat[1]);
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, enemyMask);
        pushResist += _unitSkillManager.showeringStat[0];
        
        for (int i = 0; i < rays.Length; i++)
        {
            Enemy _enemy = rays[i].transform.GetComponent<Enemy>();
            
            SetSkillEffect(_enemy);
        }
    }

    // 후크 선장 스킬 
    private void SwingSword()
    {
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, enemyMask);

        for (int i = 0; i < rays.Length; i++)
        {
            deadRan = Random.Range(1, 101);
                Enemy _enemy = rays[i].transform.GetComponent<Enemy>();
            
            
            skillAnim[3].Play();

            if (deadRan <= _unitSkillManager.swingSwordStat[2])
            {
                if (_enemy.transform.GetComponent<Enemy>().bossType != "Boss")
                {
                    ShowDamageTxt(_enemy.transform, "999", false,
                        _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);

                    _enemy.UpdateHpBar(999, true);
                }
                else
                {
                    
                    ShowDamageTxt(_enemy.transform, (_enemy.nowHpStat * 0.2f).ToString(), false,
                        _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);

                    _enemy.UpdateHpBar(_enemy.nowHpStat * 0.2f, true);
                }
            }
            else
            {
                int damage =
                    Mathf.RoundToInt(_unitSkillManager.swingSwordStat[0] +
                                     attack * _unitSkillManager.swingSwordStat[1] * 0.01f);
            
                if (_enemy.isBlind == true)
                {
                    damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                }
                
                // 출혈중이 아니라면 아래 코드 실행
                if (_enemy.isHurt == false)
                {
                    ShowDamageTxt(_enemy.transform, damage.ToString(), false,
                        _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
            
                    _enemy.UpdateHpBar(damage, true);
                }
                // 출혈중이라면 아래 코드 실행
                else
                {
                    int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
            
                    ShowDamageTxt(_enemy.transform, damage.ToString(), false,
                        _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                    ShowDamageTxt(_enemy.transform, hurtDamage.ToString(), false,
                        _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
            
                    _enemy.UpdateHpBar(damage + hurtDamage, true);
                }
            }

            SetSkillEffect(_enemy);
        }
    }
    
    // 벨의 스킬
    private void WindRose()
    {
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, enemyMask);

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
                Enemy _enemy = rays[i].transform.GetComponent<Enemy>();
                
                skillAnim[4].Play();
                
                roseWindAttackAnim = true;
                startPos = transform.position;

                int damage = Mathf.RoundToInt(_unitSkillManager.windRoseStat[0] + attack * _unitSkillManager.windRoseStat[1] * 0.01f);

                if (_enemy.isBlind == true)
                {
                    damage = Mathf.RoundToInt(damage + (damage * _unitSkillManager.blindStat[1] * 0.01f));
                }
                
                // 출혈중이 아니라면 아래 코드 실행
                if (_enemy.isHurt == false)
                {
                    ShowDamageTxt(_enemy.transform ,damage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                    _enemy.UpdateHpBar(damage, true);
                }
                // 출혈중이라면 아래 코드 실행
                else
                {
                    int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    ShowDamageTxt(_enemy.transform ,damage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                    ShowDamageTxt(_enemy.transform, hurtDamage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                    _enemy.UpdateHpBar(damage + hurtDamage, true);
                }
                
                SetSkillEffect(_enemy);
            }
        }
    }
    
    // 백설 스킬
    private void AppleBox()
    {
        skillAnim[5].Play();
        
        isAppleBox = true;
    }

    private void CountAppleBox()
    {
        if (isAppleBox == true)
        {
            currentAppleBoxTime += Time.deltaTime;
            appleBoxMaintainTime += Time.deltaTime;
            skillAnim[5].transform.SetParent(GameObject.Find("CanvasBack").transform.Find("MiddleArea").transform);

            appleTimes += Time.deltaTime * 1;
            Vector3 _endPos = new Vector3(_currentRay.transform.position.x, 7f, _currentRay.transform.position.z);

            if (appleTimes < 1)
            {
                skillAnim[5].transform.position = Vector3.Lerp(transform.position, _endPos, appleTimes);
            }


            collider2Ds = Physics2D.OverlapCircleAll(skillAnim[5].transform.position, 5, unitEnemyMask);

            for (int i = 0; i < collider2Ds.Length; i++)
            {
                Transform _targetTf = collider2Ds[i].transform;

                if (_targetTf.transform.tag == "Unit")
                {
                    UnitMove _unitMove = _targetTf.transform.GetComponent<UnitMove>();

                    if (_unitMove.isAppleBuff == false)
                    {
                        setUnits[buffNum] = _unitMove.gameObject;
                        buffNum++;
                        _unitMove.isAppleBuff = true;
                        _unitMove.pushRange += _unitSkillManager.appleBoxStat[2];
                        // _unitMove = null;
                    }
                }
                else if (_targetTf.transform.tag == "Enemy")
                {
                    Enemy _enemy = _targetTf.transform.GetComponent<Enemy>();

                    if (_enemy.isAppleBuff == false)
                    {
                        setUnits[buffNum] = _enemy.gameObject;
                        buffNum++;
                        _enemy.isAppleBuff = true;
                        _enemy.pushResist -= _unitSkillManager.appleBoxStat[3];
                        // _enemy = null;
                    }
                }
            }

            // 버프 사거리 밖으로 나갈 시 아래 코드 실행 
            for (int z = 0; z < setUnits.Length; z++)
            {
                if (setUnits[z] != null)
                {
                    if (setUnits[z].transform.tag == "Unit")
                    {
                        UnitMove _unitMove = setUnits[z].transform.GetComponent<UnitMove>();

                        if (_unitMove.isAppleBuff == true)
                        {
                            float dis = Vector2.Distance(new Vector2(skillAnim[5].transform.position.x, 0), new Vector2(_unitMove.transform.position.x, 0));

                            if (dis >= 8f)
                            {
                                _unitMove.isAppleBuff = false;
                                _unitMove.pushRange -= _unitSkillManager.appleBoxStat[2];
                                setUnits[z] = null;
                            }
                        }
                    }
                    else if (setUnits[z].transform.tag == "Enemy")
                    {
                        Enemy _enemy = setUnits[z].transform.GetComponent<Enemy>();

                        if (_enemy.isAppleBuff == true)
                        {
                            float dis = Vector2.Distance(new Vector2(skillAnim[5].transform.position.x, 0), new Vector2(_enemy.transform.position.x, 0));

                            if (dis >= 8f)
                            {
                                _enemy.isAppleBuff = false;
                                _enemy.pushResist += _unitSkillManager.appleBoxStat[3];
                                setUnits[z] = null;
                            }
                        }
                    }
                }
            }
            
            // 일정 지속 시간 지나 힐 받음
            if (currentAppleBoxTime >= _unitSkillManager.appleBoxStat[0])
            {
                for (int i = 0; i < collider2Ds.Length; i++)
                {
                    Transform _targetTf = collider2Ds[i].transform;

                    if (_targetTf.transform.tag == "Unit")
                    {
                        UnitMove _unitMove = _targetTf.transform.GetComponent<UnitMove>();
                        
                        if (_unitMove.nowHpStat + _unitSkillManager.appleBoxStat[1] >= _unitMove.maxHp)
                        {
                            ShowDamageTxt(_unitMove.transform, (_unitMove.maxHp - _unitMove.nowHpStat).ToString(), false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1, 0), Color.green);
                            _unitMove.nowHpStat = _unitMove.maxHp;
                        }
                        else
                        {
                            ShowDamageTxt(_unitMove.transform, _unitSkillManager.appleBoxStat[1].ToString(), false, _unitMove.hpBackImage.transform.position + new Vector3(0, 1, 0), Color.green);
                            _unitMove.nowHpStat += _unitSkillManager.appleBoxStat[1];
                        }

                        if (_unitMove.isDead == false)
                        {
                            _unitMove.UpdateHpBar(0, false);
                        }
                    }
                }
                currentAppleBoxTime = 0;
            }

            // 지속 시간 끝남
            if (appleBoxMaintainTime >= _unitSkillManager.appleBoxStat[4])
            {
                for (int i = 0; i < setUnits.Length; i++)
                {
                    if (setUnits[i] != null)
                    {
                        if (setUnits[i].transform.tag == "Unit")
                        {
                            UnitMove _unitMove = setUnits[i].transform.GetComponent<UnitMove>();

                            if (_unitMove.isAppleBuff == true)
                            {
                                _unitMove.isAppleBuff = false;
                                _unitMove.pushRange -= _unitSkillManager.appleBoxStat[2];
                                setUnits[i] = null;
                            }
                        }
                        else if (setUnits[i].transform.tag == "Enemy")
                        {
                            Enemy _enemy = setUnits[i].transform.GetComponent<Enemy>();

                            if (_enemy.isAppleBuff == true)
                            {
                                _enemy.isAppleBuff = false;
                                _enemy.pushResist += _unitSkillManager.appleBoxStat[3];
                                setUnits[i] = null;
                            }
                        }
                    }
                }
                buffNum = 0;
                appleTimes = 0;
                skillAnim[5].Stop();
                currentAppleBoxTime = 0;
                appleBoxMaintainTime = 0;
                isAppleBox = false;
            }
        }
    }

    private void ThrowHedgehog()
    {
        skillAnim[6].Play();
        throwHedgehogAnim = true;
        
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_currentRay.transform.position, 5, unitEnemyMask);

        for (int i = 0; i < collider2Ds.Length; i++)
        {
            Transform _targetTf = collider2Ds[i].transform;

            if (_targetTf.transform.tag == "Enemy")
            {
                Enemy _enemy = _targetTf.transform.GetComponent<Enemy>();
                
                int damage = Mathf.RoundToInt(_enemy.nowHpStat / 2);

                ShowDamageTxt(_enemy.transform ,damage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                _enemy.UpdateHpBar(damage, true);

                SetSkillEffect(_enemy);
            }
        }
        
    }
    
    /// <summary>
    /// 출혈
    /// </summary>
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
    
    /// <summary>
    /// 도발
    /// </summary>
    private void Taunt()
    {
        if (isTaunt == true)
        {
            tauntMaintainTime += Time.deltaTime;
            
            RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, attackRange, enemyMask);
            
        
            for (int i = 0; i < rays.Length; i++)
            {
                Enemy _enemy = rays[i].transform.GetComponent<Enemy>();

                _enemy.isTaunt = true;
            }
        
            // 지속 시간 끝남
            if (tauntMaintainTime >= _unitSkillManager.tauntStat[0])
            {
                pushResist -= _unitSkillManager.showeringStat[0];
                skillShowEffect[1].Stop();
                tauntMaintainTime = 0;
                isTaunt = false;
            }
        }
    }
    
    /// <summary>
    /// 실명
    /// </summary>
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
    /// 기절
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
