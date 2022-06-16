using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float maxTowerHp;
    public float towerHp;

    public Image towerHpImage;
    [SerializeField] private Image backSliderHp;

    [SerializeField] private StageManager _stageManager;
    
    // 유닛을 소환할 때 부모로 사용할 오브젝트 (캔버스)
    [SerializeField] private GameObject[] parentTrans;

    public Unit[] Units;

    [SerializeField] private int[] maxSpawnEnemy;

    [SerializeField] private bool[] _isSpawn;

    [SerializeField] private int[] nowEnemy;
    
    private bool isDead;

    public Image towerImage;
    
    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;
    
    [SerializeField] private BoxCollider2D boxCol;

    private bool isCheck;

    [SerializeField] private DefenceLevel _defenceLevel;

    private float spwanCooltime;
    
    private bool backHpHit;

    public int nowDamagePos;
    
    public List<GameObject> damageTexts = new List<GameObject>();

    private void Start()
    {
        towerHp = maxTowerHp;
    }

    private void Update()
    {
        towerHpImage.fillAmount = Mathf.Lerp(towerHpImage.fillAmount, towerHp / maxTowerHp, Time.deltaTime * 5f);
       
        if (backHpHit == true)
        {
            backSliderHp.fillAmount = Mathf.Lerp(backSliderHp.fillAmount, towerHpImage.fillAmount, Time.deltaTime * 5f);
            if (towerHpImage.fillAmount >= backSliderHp.fillAmount - 0.01f)
            {
                backHpHit = false;
                backSliderHp.fillAmount = towerHpImage.fillAmount;
            }
        }
        
        if (_stageManager.inStage == true && isDead == false)
        {
            if (_isSpawn[0] == false)
            {
                SpawnMonster(0);
            }

            if (_isSpawn[1] == false)
            {
                SpawnMonster(1);
            }

            if (_isSpawn[2] == false)
            {
                SpawnMonster(2);
            }
        }
    }

    private void SpawnMonster(int num)
    {
        _isSpawn[num] = true;
        StartCoroutine(SpawnSnailCoroutine(num));
    }

    private IEnumerator SpawnSnailCoroutine(int num)
    {
        if (nowEnemy[num] < maxSpawnEnemy[num])
        {
            if (isCheck == true)
            {
                SpawnEnemySet(num);
                nowEnemy[num] += 1;
            }

            if (_defenceLevel.isLevel[0] == true)
            {
                SetSpawnCoolTime(num, 0);
            }
            else if (_defenceLevel.isLevel[1] == true)
            {
                SetSpawnCoolTime(num, 1);
            }
            else if (_defenceLevel.isLevel[2] == true)
            {
                SetSpawnCoolTime(num, 2);
            }
            else
            {
                SetSpawnCoolTime(num, 3);
            }
            
            yield return new WaitForSeconds(spwanCooltime);
            
            _isSpawn[num] = false;
            if (isCheck == false)
            {
                isCheck = true;
            }
        }
    }

    private void SetSpawnCoolTime(int num, int i)
    {
        if (num == 0)
        {
            spwanCooltime = Units[num].spawnCoolTime - _defenceLevel.ghostLevelCoolTime[i];
        }
        else if (num == 1)
        {
            spwanCooltime = Units[num].spawnCoolTime - _defenceLevel.snailLevelCoolTime[i];
        }
        else
        {
            spwanCooltime = Units[num].spawnCoolTime - _defenceLevel.slimeLevelCoolTime[i];
        }
    }
    
    private void SpawnEnemySet(int num)
    {
        int _ranValue = UnityEngine.Random.Range(0, 3);

        // 슬라임이 소환될 때마다 3마리 중 랜덤으로 하나 소환
        if (num == 2)
        {
            int randomMonster = UnityEngine.Random.Range(2, 5);
            num = randomMonster;
        }

        // 몬스터 생성 부분 작성
        GameObject go = Instantiate(Units[num].unitPrefab, Vector3.zero, Quaternion.identity);
        
        go.GetComponent<Enemy>().unit = Units[num];


        if (_ranValue == 0)
        {
            go.GetComponent<RectTransform>().SetParent(parentTrans[0].transform, false);
        }
        else if (_ranValue == 1)
        {
            go.GetComponent<RectTransform>().SetParent(parentTrans[1].transform, false);
        }
        else
        {
            go.GetComponent<RectTransform>().SetParent(parentTrans[2].transform, false);
        }

        go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        // 유령이라면 
        if (num == 0)
        {
            go.GetComponent<RectTransform>().localPosition = new Vector3(40f, -1.75f + -2.5f * _ranValue, 0);
        }
        else
        {
            go.GetComponent<RectTransform>().localPosition = new Vector3(40f, -0.5f + -2.5f * _ranValue, 0);
        }
    }
    
    public void UpdateHpBar(float damage)
    {
        towerImage.color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
        Invoke("ResetImageAlpha", 0.5f);
        towerHp -= damage;
        Invoke("BackHpFun", 0.5f);

        if (towerHp <= 0)
        {
            isDead = true;
            boxCol.enabled = false;
            StartCoroutine(FadeUnit());
        }
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
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
