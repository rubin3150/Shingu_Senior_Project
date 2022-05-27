using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;

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
    public float arrackDelay;
    public float criRate;
    public int criDamage;
    
    public LayerMask layerMask;

    public GameObject hit_Effect;

    private RaycastHit2D _ray;
    
    private RaycastHit2D _currentRay;

    private bool _isAttack;

    public Unit unit;

    private float _currentDelay;

    public Image unitImage;
    
    private Vector3 _hitPos;

    private bool isDead;
    
    [SerializeField] private BoxCollider2D boxCol;
    
    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;

    [SerializeField] private GameObject damageText;
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = unit.speedStat;
        nowHpStat = unit.hpStat;
        attack = unit.attackStat;
        attackRange = unit.attackRangeStat;
        attackAddRange = unit.attackAddRangeStat;
        arrackDelay = unit.attackDelayStat;
        pushRange = unit.pushRange;
        criRate = unit.criRate;
        criDamage = unit.criDamage;
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

                if (_isAttack == true)
                {
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

    private RaycastHit2D[] _rays;
    
    private void CheckObject()
    {
        // _rays = Physics2D.BoxCastAll(transform.position, new Vector2(7.5f,18), 0, Vector2.left, attackRange, layerMask);
        _ray = Physics2D.BoxCast(transform.position, new Vector2(1f,18), 0, Vector2.left, attackRange + attackAddRange, layerMask);
        // Debug.DrawRay();
        // ray를 rays로 변경
        if (_ray.collider != null)
        {
            Debug.Log("아군 발견");
            donMove = true;

            if (!_isAttack)
            {
                // _currentRay = _ray;
                Attack();
            }
            
        }
        else
        {
            // _isAttack = false;
            donMove = false;
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
            
            if (_ray.transform.tag == "Player")
            {
                _ray.transform.GetComponent<Player>().UpdateHpBar(criticalDamage);
                ShowDamageTxt(criticalDamage, true, new Vector3(0, 6.5f, 0));
                _hitPos = new Vector3(3f, 1, 0);
            }
            else if (_ray.transform.tag == "Unit")
            {
                _ray.transform.GetComponent<UnitMove>().UpdateHpBar(criticalDamage, true);

                if (_ray.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
                {
                    // Debug.Log("팅커벨 발견");
                    _hitPos = new Vector3(3f, 4, 0);
                    ShowDamageTxt(criticalDamage, true, new Vector3(0, 9.5f, 0));
                }
                else
                {
                    ShowDamageTxt(criticalDamage, true, new Vector3(0, 5.5f, 0));
                    _hitPos = new Vector3(1f, -1, 0);
                }
                _ray.transform.position -= new Vector3(_ray.transform.GetComponent<UnitMove>().pushRange, 0f, 0f);
            }
        }
        else
        {
            if (_ray.transform.tag == "Player")
            {
                // Debug.Log(1);
                _ray.transform.GetComponent<Player>().UpdateHpBar(attack);

                ShowDamageTxt(attack, false, new Vector3(0, 6.5f, 0));
                
                _hitPos = new Vector3(3f, 1, 0);
            }
            else if (_ray.transform.tag == "Unit")
            {
                _ray.transform.GetComponent<UnitMove>().UpdateHpBar(attack, true);

                if (_ray.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
                {
                    // Debug.Log("팅커벨 발견");
                    ShowDamageTxt(attack, false, new Vector3(0, 9.5f, 0));
                    _hitPos = new Vector3(3f, 4, 0);
                }
                else
                {
                    ShowDamageTxt(attack, false, new Vector3(0, 5.5f, 0));
                    _hitPos = new Vector3(1f, -1, 0);
                }
            }
        }

        GameObject go = Instantiate(hit_Effect, _ray.transform.position + _hitPos, Quaternion.identity);
        go.GetComponent<EffekseerEmitter>().Play();
        
        Destroy(go, 1.5f);
        
        // _isAttack = false;
    }

    private void ShowDamageTxt(float damage, bool cirDamage, Vector3 yPos)
    {
        GameObject damageGo = Instantiate(damageText);
        damageGo.transform.parent = _ray.transform;
        damageGo.transform.position = _ray.transform.position + yPos; // 일반 유닛 5.5 // 팅커벨 유닛 7.5
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
    
    private void ResetImageAlpha()
    {
        unitImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
    }
}
