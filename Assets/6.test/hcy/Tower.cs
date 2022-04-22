using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float maxTowerHp;
    public float towerHp;

    public Image towerHpImage;

    [SerializeField] private StageManager _stageManager;
    
    // 유닛을 소환할 때 부모로 사용할 오브젝트 (캔버스)
    [SerializeField] private GameObject parentTrans;

    private int _r;
    
    public Unit[] Units;

    public int maxSpwanEnemy;

    private bool _isSpawn;

    private int nowEnemy;

    [SerializeField] private float spawnDelay;
    
    

    private void Start()
    {
        towerHp = maxTowerHp;
    }

    private void Update()
    {
        if (_stageManager.inStage == true)
        {
            if (_isSpawn == false)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        _isSpawn = true;
        StartCoroutine(SpawnCoroutine());
        
    }

    private IEnumerator SpawnCoroutine()
    {
        if (nowEnemy < maxSpwanEnemy)
        {
            _r = UnityEngine.Random.Range(0, Units.Length);
            
            // 몬스터 스폰 구현

            SpawnEnemySet();
            //theBase.AddMaxUnit(Units[_r].GetComponent<UnitPickUp>().unit);

            yield return new WaitForSeconds(spawnDelay);
            nowEnemy += 1;
            _isSpawn = false;
        }
    }
    
    public void SpawnEnemySet()
    {
    
        int _ranValue = UnityEngine.Random.Range(0, 3);

        // 몬스터 생성 부분 작성
        GameObject go = Instantiate(Units[_r].unitPrefab, Vector3.zero, Quaternion.identity);
        
        go.GetComponent<Enemy>().unit = Units[_r];

        go.GetComponent<RectTransform>().SetParent(parentTrans.transform, false);

        go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        go.GetComponent<RectTransform>().localPosition = new Vector3(38.75f, -0.15f * _ranValue, 0);
    }
 
    public void UpdateHpBar(float damage)
    {
        towerHp -= damage;
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        towerHpImage.fillAmount = towerHp / maxTowerHp;

        if (towerHp <= 0)
        {
            Dead();
        }
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
