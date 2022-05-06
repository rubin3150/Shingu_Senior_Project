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
    public float arrackDelay;
    
    public LayerMask layerMask;

    public GameObject hit_Effect;

    private RaycastHit2D _ray;

    private bool _isAttack;

    public Unit unit;

    private float _currentDelay;
    
    
    private Vector3 _hitPos;
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = unit.speedStat;
        nowHpStat = unit.hpStat;
        attack = unit.attackStat;
        attackRange = unit.attackRangeStat;
        arrackDelay = unit.attackDelayStat;
        pushRange = unit.pushRange;
        isMove = true;
    }

    // Update is called once per frame
    void Update()
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

    private RaycastHit2D[] _rays;
    
    private void CheckObject()
    {
        // _rays = Physics2D.BoxCastAll(transform.position, new Vector2(7.5f,18), 0, Vector2.left, attackRange, layerMask);
        _ray = Physics2D.BoxCast(transform.position, new Vector2(7.5f,18), 0, Vector2.left, attackRange, layerMask);
    
        // ray를 rays로 변경
        if (_ray.collider != null)
        {
            donMove = true;

            if (!_isAttack)
            {
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
        AttackDealy();
    }

    private void AttackDealy()
    {
        if (_ray.transform.tag == "Player")
        {
            _ray.transform.GetComponent<Player>().UpdateHpBar(attack);
            _hitPos = new Vector3(3f, 1, 0);
        }
        else if (_ray.transform.tag == "Unit")
        {
            _ray.transform.GetComponent<UnitMove>().UpdateHpBar(attack);

            if (_ray.transform.GetComponent<UnitMove>().unit.unitName == "팅커벨")
            {
                // Debug.Log("팅커벨 발견");
                _hitPos = new Vector3(3f, 4, 0);
            }
            else
            {
                _hitPos = new Vector3(1f, -1, 0);
            }
        }
        
        // Debug.Log(_ray.transform.position);
        
        GameObject go = Instantiate(hit_Effect, _ray.transform.position + _hitPos, Quaternion.identity);
        go.GetComponent<EffekseerEmitter>().Play();
        
        Destroy(go, 1.5f);
        
        // _isAttack = false;
        
    }

    public void UpdateHpBar(float damage)
    {
        nowHpStat -= damage;
        // transform.position += new Vector3(pushRange, 0, 0);
        
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        maxHpStatImage.fillAmount = nowHpStat / unit.hpStat;

        if (nowHpStat <= 0)
        {
            Destroy(gameObject);
        }
    }
}
