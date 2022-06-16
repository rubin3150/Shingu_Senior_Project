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

    private Vector3 _enemyPos;

    [SerializeField] public Image skillCoolTimeImage;

    [SerializeField] private UnitSkillManager _unitSkillManager;

    private bool _firstCheck;

    private Vector3 first;

    private bool backHpHit;

    public GameObject damageText;
    
    public int nowDamagePos;
    
    public List<GameObject> damageTexts = new List<GameObject>();

    private void Update()
    {
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
            
                if (_isAttack == true)
                {
                    // animator.SetBool("Attack", false);
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
                        UseSkill();
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
        if (skillIndex == 0)
        {
            ThrowAxe();
        }
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
            // Debug.Log("유닛이 치명타 ");
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

                if (_enemy.isHurt == false)
                {
                    ShowDamageTxt(_currentRay.transform, criticalDamage.ToString(), true, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                    _enemy.UpdateHpBar(criticalDamage);
                }
                else
                {
                    int hurtDamage = Mathf.RoundToInt(criticalDamage * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _enemy.UpdateHpBar(hurtDamage);
                
                    ShowDamageTxt(_enemy.transform, criticalDamage.ToString(),true, _enemy.maxHpStatImage.transform.position + new Vector3(0, 2, 0), Color.red);
                    ShowDamageTxt(_enemy.transform, hurtDamage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
                

                if (_enemy.pushResist - pushRange < 0)
                {
                    _currentRay.transform.position -= new Vector3(_enemy.pushResist - pushRange, 0f, 0f);
                }

                if (_enemyPos.x > 40f)
                {
                    _currentRay.transform.position = new Vector3(38.75f, 0, 0);
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
                ShowDamageTxt(_currentRay.transform, attack.ToString(), false, _currentRay.transform.GetComponent<Tower>().towerHpImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                _currentRay.transform.GetComponent<Tower>().UpdateHpBar(attack);
            }
            else if (_currentRay.transform.tag == "Enemy")
            {
                Enemy _enemy = _currentRay.transform.GetComponent<Enemy>();

                if (_enemy.isHurt == false)
                {
                    ShowDamageTxt(_currentRay.transform, attack.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0),Color.red);

                    _enemy.UpdateHpBar(attack);
                }
                else
                {
                    int hurtDamage = Mathf.RoundToInt(attack * _unitSkillManager.hurtStat[1] * 0.01f);
                    
                    _enemy.UpdateHpBar(hurtDamage);
                
                    ShowDamageTxt(_enemy.transform, attack.ToString(),false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.red);
                    ShowDamageTxt(_enemy.transform, hurtDamage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1, 0), Color.yellow);
                }
            }
        }
        
        GameObject go = Instantiate(hit_Effect, _currentRay.transform.position, Quaternion.identity);
        go.GetComponent<EffekseerEmitter>().Play();
        Destroy(go, 1.5f);
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
        RaycastHit2D[] rays = Physics2D.BoxCastAll(transform.position, new Vector2(1f, 18), 0, Vector2.right, _unitSkillManager.throwAxeStat[0], enemyMask);

        for (int i = 0; i < rays.Length; i++)
        {
            // 동일 선상에 있다면
            if (transform.position.y == rays[i].transform.position.y || transform.position.y == rays[i].transform.position.y + 1.25f)
            {
                if (_firstCheck == false)
                {
                     first = rays[i].transform.position;
                    _firstCheck = true;
                }
                
                rays[i].transform.position = new Vector3(first.x + _unitSkillManager.throwAxeStat[0], first.y, 0);
                Enemy _enemy = rays[i].transform.GetComponent<Enemy>();

                int damage =
                    Mathf.RoundToInt(_unitSkillManager.throwAxeStat[1] + attack * _unitSkillManager.throwAxeStat[2] * 0.01f);
                
                ShowDamageTxt(rays[i].transform ,damage.ToString(), false, _enemy.maxHpStatImage.transform.position + new Vector3(0, 1f, 0), Color.red);
                
                _enemy.UpdateHpBar(damage);

                // 스킬 효과가 출혈이라면
                if (skillEffect == 0)
                {
                    _enemy.isHurt = true;
                    _enemy.hurtMaintainTime = 0;
                    _enemy.currentHurtDamageTime = 0;
                }
                
                if (_enemy.isStop == false)
                {
                    _enemy.StopMove();
                }
            }
        }
    }
}
