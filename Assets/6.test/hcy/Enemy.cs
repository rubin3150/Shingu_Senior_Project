using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public bool isMove;
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

    

    private RaycastHit2D _ray;

    private bool _isAttack;

    public Unit unit;
    
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
                transform.position -= new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
            }
            
            CheckObject();
        }
    }
    
    private void CheckObject()
    {
        _ray = Physics2D.Raycast(transform.position, Vector2.left, 3.75f + attackRange, layerMask);

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
            _isAttack = false;
            donMove = false;
        }
    }
    
    private void Attack()
    {
        _isAttack = true;
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        if (_ray.transform.tag == "Player")
        {
            _ray.transform.GetComponent<Player>().UpdateHpBar(attack);
        }
        else if (_ray.transform.tag == "Unit")
        {
            //_ray.transform.GetComponent<UnitMove>().UpdateHpBar(attack);
        }

        yield return new WaitForSeconds(arrackDelay);
        _isAttack = false;
        
    }

    public void UpdateHpBar(float damage)
    {
        nowHpStat -= damage;
        transform.position += new Vector3(pushRange, 0, 0);
        
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        maxHpStatImage.fillAmount = nowHpStat / unit.hpStat;

        if (nowHpStat <= 0)
        {
            Destroy(gameObject);
        }
    }
}
