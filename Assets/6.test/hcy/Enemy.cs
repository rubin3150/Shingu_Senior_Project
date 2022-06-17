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
    public bool isCheckTaunt;
    // 도발 유지 시간
    public float tauntMaintainTime;

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
                Taunt();

                if (_isAttack == true)
                {
                    _currentDelay += Time.deltaTime;
                    _skillDelay += Time.deltaTime;

                    skillCoolTimeImage.fillAmount = _skillDelay / skillCoolTime;

                    if (_skillDelay >= skillCoolTime)
                    {
                        _isSkill = true;
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
        else if (isCheckTaunt == false)
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
        else if (isCheckTaunt == true)
        {
            for (int i = 0; i < rays.Length; i++)
            {
                if (rays[i].transform.GetComponent<UnitMove>().isTaunt == true)
                {
                    _ray = rays[i];

                    break;
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
        if (skillIndex == 0)
        {
            // 도끼 던지기 스킬 방동
            ThrowAxe();
        }
        else if (skillIndex == 1)
        {
            MagicHeal();
        }
        else if (skillIndex == 2)
        {
            Showering();
        }
        else if (skillIndex == 7)
        {
            // 달팽이 스킬
            Butt();
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
                Player _player = _currentRay.transform.GetComponent<Player>();

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
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
                
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
        }
        else
        {
            if (_currentRay.transform.tag == "Player")
            {
                Player _player = _currentRay.transform.GetComponent<Player>();

                if (_player.isHurt == false)
                {
                    _player.UpdateHpBar(attack);

                    ShowDamageTxt(_player.transform, attack.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);
                }
                else
                {
                    int hurtDamage = Mathf.RoundToInt(attack * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _player.UpdateHpBar(attack + hurtDamage);

                    ShowDamageTxt(_player.transform, attack.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.red);

                    ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _currentRay.transform.GetComponent<Player>().hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
            }
            else if (_currentRay.transform.tag == "Unit")
            {
                UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();

                if (_unitMove.isHurt == false)
                {
                    _unitMove.UpdateHpBar(attack, true);
                
                    ShowDamageTxt(_unitMove.transform, attack.ToString(), false,_unitMove.hpBackImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                }
                else
                {
                    int hurtDamage = Mathf.RoundToInt(attack * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _unitMove.UpdateHpBar(hurtDamage+ attack, true);
                
                    ShowDamageTxt(_unitMove.transform, attack.ToString(),false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.red);
                    ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
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

                    _unitMove.isCheckTaunt = false;
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
    
    private void ThrowAxe()
    {
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);
    
        for (int i = 0; i < rays.Length; i++)
        {
            if (_firstCheck == false)
            {
                first = rays[i].transform.position;
                _firstCheck = true;
            }
            
            if (first.y == 0.5f || first.y == -1.75f)
            {
                skillAnim[0].transform.position = new Vector3(0, 2, 0);
            }
            else if (first.y == 3f || first.y == -4.25f)
            {
                skillAnim[0].transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                skillAnim[0].transform.position = new Vector3(0, -2, 0);
            }
            
            // 동일 선상에 있다면
            if (first.y == rays[i].transform.position.y || first.y == rays[i].transform.position.y + 1.25f)
            {  
                
                skillAnim[0].Play();
                Invoke("StopAxeAnim", _unitSkillManager.throwAxeStat[3]);
                
                rays[i].transform.position = new Vector3(first.x - _unitSkillManager.throwAxeStat[0], first.y, 0);

                int damage =
                    Mathf.RoundToInt(_unitSkillManager.throwAxeStat[1] + attack * _unitSkillManager.throwAxeStat[2] * 0.01f);
                
                if (rays[i].transform.tag == "Unit")
                {
                    UnitMove _unitMove = rays[i].transform.GetComponent<UnitMove>();
                    
                    if (_unitMove.transform.position.x < -46.25f)
                    {
                        _unitMove.transform.position = new Vector3(46.25f, _unitMove.transform.position.y, 0);
                    }

                    if (_unitMove.isHurt == false)
                    {
                        ShowDamageTxt(rays[i].transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                        _unitMove.UpdateHpBar(damage, true);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        ShowDamageTxt(_unitMove.transform ,damage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        ShowDamageTxt(_unitMove.transform, hurtDamage.ToString(), false, _unitMove.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                        _unitMove.UpdateHpBar(damage + hurtDamage, true);
                    }

                    // 스킬 효과가 출혈이라면
                    if (skillEffect == 0)
                    {
                        SetSkillEffect(0, null, _unitMove, true);
                    }
                    
                    if (_unitMove.isStop == false)
                    {
                        _unitMove.StopMove();
                    }
                }
                // 맞은 물체가 플레이어 라면
                else
                {
                    Player _player = rays[i].transform.GetComponent<Player>();

                    if (_player.isHurt == false)
                    {
                        ShowDamageTxt(rays[i].transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                        _player.UpdateHpBar(damage);
                    }
                    else
                    {
                        int hurtDamage = Mathf.RoundToInt(damage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                        ShowDamageTxt(_player.transform ,damage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                        ShowDamageTxt(_player.transform, hurtDamage.ToString(), false, _player.hpImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                
                        _player.UpdateHpBar(damage + hurtDamage);
                    }
                    
                    // 스킬 효과가 출혈이라면
                    if (skillEffect == 0)
                    {
                        SetSkillEffect(0, _player, null, false);
                    }
                }
            }
        }
    }

    private void StopAxeAnim()
    {
        skillAnim[0].Stop();
    }
    
    private void StopHealAnim()
    {
        skillAnim[1].Stop();
    }
    
    private void StopShoweringAnim()
    {
        skillAnim[2].Stop();
    }
    
    private void StopButtAnim()
    {
        skillAnim[7].Stop();
    }
    
    private void SetSkillEffect(int i, Player player, UnitMove unitMove, bool isUnitMove)
    {
        if (isUnitMove == true)
        {
            unitMove.skillShowEffect[i].Play();
            unitMove.isHurt = true;
            unitMove.hurtMaintainTime = i;
            unitMove.currentHurtDamageTime = i;
        }
        else
        {
            player.skillShowEffect[i].Play();
            player.isHurt = true;
            player.hurtMaintainTime = i;
            player.currentHurtDamageTime = i;
        }
    }
    
    
    
    private void MagicHeal()
    {
        skillAnim[1].Play();
        skillAnim[1].transform.SetParent(_currentRay.transform);
        skillAnim[1].transform.position = _currentRay.transform.position - new Vector3(0, 2, 0);
        Invoke("StopHealAnim", _unitSkillManager.magicHealStat[1]);
        
        Enemy _enemy = _currentRay.transform.GetComponent<Enemy>();

        if (_enemy != null)
        {
            _enemy._skillDelay += _unitSkillManager.magicHealStat[0];
            _enemy.skillCoolTimeImage.fillAmount = _enemy._skillDelay / _enemy.skillCoolTime;
            ShowDamageTxt(_enemy.transform, (_enemy.maxHpStat - _enemy.nowHpStat).ToString(), false, _enemy.hpBackImage.transform.position + new Vector3(0, 1, 0), Color.green);
            _enemy.nowHpStat = _enemy.maxHpStat;
            _enemy.UpdateHpBar(0, false);
        }
        
    }
    
    private void Showering()
    {
        isTaunt = true;
        skillAnim[2].Play();
        Invoke("StopShoweringAnim", _unitSkillManager.showeringStat[1]);
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);
        
        pushResist += _unitSkillManager.showeringStat[0];
        
        for (int i = 0; i < rays.Length; i++)
        {
            if (rays[i].transform.tag != "Player")
            {
                UnitMove _unitMove = rays[i].transform.GetComponent<UnitMove>();
            
                if (skillEffect == 0)
                {
                    SetSkillEffect(0, null, _unitMove, true);
                }
                else if (skillEffect == 1)
                {
                    _unitMove.isCheckTaunt = true;
                }
            }
        }
    }


    private void Butt()
    {
        skillAnim[7].Play();
        Invoke("StopButtAnim", _unitSkillManager.buttStat[3]);
        
        int damage =
            Mathf.RoundToInt(_unitSkillManager.buttStat[0] + attack * _unitSkillManager.buttStat[1] * 0.01f);

        if (_currentRay.transform.tag == "Unit")
        {
            UnitMove _unitMove = _currentRay.transform.GetComponent<UnitMove>();
            
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
            
            // 스킬 효과가 출혈이라면
            if (skillEffect == 0)
            {
                SetSkillEffect(0, null, _unitMove, true);
            }
        }
        else
        {
            Player _player = _currentRay.transform.GetComponent<Player>();
            
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
            
            // 스킬 효과가 출혈이라면
            if (skillEffect == 0)
            {
                SetSkillEffect(0, _player, null, true);
            }
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
        
            // 지속 시간 끝남
            if (tauntMaintainTime >= _unitSkillManager.tauntStat[0])
            {
                RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.left, attackRange, layerMask);
                pushResist -= _unitSkillManager.showeringStat[0];
        
                for (int i = 0; i < rays.Length; i++)
                {
                    if (rays[i].transform.tag != "Player")
                    {
                        UnitMove _unitMove = rays[i].transform.GetComponent<UnitMove>();

                        _unitMove.isCheckTaunt = false;
                    }
                }
                
                tauntMaintainTime = 0;
                isTaunt = false;
            }
        }
    }
}
