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

    private bool isDead;

    public Image towerImage;
    
    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;
    
    [SerializeField] private BoxCollider2D boxCol;

    private void Start()
    {
        towerHp = maxTowerHp;
    }

    private void Update()
    {
        if (_stageManager.inStage == true && isDead == false)
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
        
        go.GetComponent<RectTransform>().localPosition = new Vector3(38.75f, -0.5f + -2.5f * _ranValue, 0);
    }
 
    public void UpdateHpBar(float damage)
    {
        towerImage.color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
        Invoke("ResetImageAlpha", 0.5f);
        towerHp -= damage;
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        towerHpImage.fillAmount = towerHp / maxTowerHp;

        if (towerHp <= 0)
        {
            isDead = true;
            boxCol.enabled = false;
            StartCoroutine(FadeUnit());
        }
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }
    
    private IEnumerator FadeUnit()
    {
        // 변수 초기화
        _unitAlpha = 0f;

        // 변수에 몬스터 이미지 컬러 값을 넣음
        Color alpha =  towerImage.color;

        // 알파 값이 255보다 작을 동안에 아래 코드 실행
        while (alpha.a < 1f)
        {
            // 실제 시간 / 설정한 딜레이 시간을 계산한 값을 변수에 넣음
            _unitAlpha += Time.deltaTime / _unitAlphaTime;

            // 알파 값 조절
            alpha.a = Mathf.Lerp(0, 1, _unitAlpha);

            towerImage.color = alpha;

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
            towerImage.color = alpha;
            
            yield return null;
        }

        yield return null;
        Destroy(gameObject);
    }
    
    private void ResetImageAlpha()
    {
        towerImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
    }
    
}
