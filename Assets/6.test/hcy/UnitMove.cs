using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;

public class UnitMove : MonoBehaviour
{
    public float moveSpeed;
    public float attack;
    public float attackRange;
    public float arrackDelay;
    public float criRate;
    public int criDamage;
    public float maxHp;

    public GameObject selectUnitBase;
    public SelectUnitBase selectUnitBaseScripts;
    
    public float nowHpStat;
    public Image maxHpStatImage;

    private bool isMove;
    public bool donMove;

    public Unit unit;

    public LayerMask layerMask;

    public float pushRange;

    private RaycastHit2D _ray;

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

    private bool isDead;

   

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
            
                if (_isAttack == true)
                {
                    // animator.SetBool("Attack", false);
                    _currentDelay += Time.deltaTime;

                    if (_currentDelay >= arrackDelay)
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
        _ray = Physics2D.BoxCast(transform.position, new Vector2(7.5f,18), 0, Vector2.right, attackRange, layerMask);
        // Vector2.right, attackRange, layerMask
        // Debug.DrawRay(transform.position + new Vector3(0, 5, 0), Vector2.right, Color.red, attackRange);
        
        if (_ray.collider != null)
        {
            donMove = true;

            if (animator != null)
            {
                animator.SetBool("Move", false);
            }
            
            // Debug.Log("적 발견");
            
            if (!_isAttack)
            {
                Attack();
            }
        }
        else
        {
            // _isAttack = false;
            donMove = false;
            
            if (animator != null)
            {
                animator.SetBool("Move", true);
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
        // transform.position -= new Vector3(pushRange, 0, 0);
        
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
            
            yield return null;
        }

        yield return null;
        Destroy(gameObject);
    }

    private void Attack()
    { 
        if (animator != null)
        {
            animator.SetBool("Attack", true);
            Invoke("ResetAttackAnim", 0.5f);
        }
        _isAttack = true;
        AttackDelay();
    }

    private void AttackDelay()
    {
        if (_ray.transform.tag == "Tower")
        {
            _ray.transform.GetComponent<Tower>().UpdateHpBar(attack);
        }
        else if (_ray.transform.tag == "Enemy")
        {
            _ray.transform.GetComponent<Enemy>().UpdateHpBar(attack);
            // UpdateHpBar(_ray.transform.GetComponent<Enemy>().attack);
        }
        
        GameObject go = Instantiate(hit_Effect, _ray.transform.position - new Vector3(3.5f, 0, 0), Quaternion.identity);
        go.GetComponent<EffekseerEmitter>().Play();

        Destroy(go, 1.5f);
    }
}
