using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using  Effekseer;

public class Player : MonoBehaviour
{
    #region 변수 영역

    // StageManager 스크립트를 담을 변수
    [SerializeField] private StageManager stageManager;

    [SerializeField] private PlayerSet playerSet;
    
    // 레이캐스트 발사시 충돌할 레이어를 설정하는 변수
    [SerializeField] private LayerMask layerMask;
    
    // 플레이어의 Hp 이미지를 담을 변수
    [SerializeField] public Image hpImage;
    [SerializeField] private Image backSliderHp;
    
    // 플레이어의 스피드를 담을 변수
    public float speedStat;

    public float maxMana;

    // 플레이어의 현재 Hp를 담을 변수
    public float hpStat;

    // 왼쪽으로 이동할 수 있는지 아닌지 체크할 변수
    private bool _donLeftMove;
    // 오른쪽으로 이동할 수 있는지 아닌지 체크할 변수
    private bool _donRightMove;

    public Image stagePlayerImage;
    public Image quickSlotSetPlayerImage;

    public bool isDead;
    
    private float _unitAlpha;
    
    private float _unitAlphaTime = 1f;
    
    [SerializeField] private BoxCollider2D boxCol;

    [SerializeField] private TextMeshProUGUI hpTxt;

    [SerializeField] private Image hpTxtImage;

    [SerializeField] private UnitSkillManager _unitSkillManager;

    private bool backHpHit;

    public int nowDamagePos;
    
    public List<GameObject> damageTexts = new List<GameObject>();

    [SerializeField] private GameObject damageText;
    
    public EffekseerEmitter[] skillShowEffect;
    
    // 출혈증인지 아닌지 체크할 변수 
    public bool isHurt;
    // 출혈 유지 시간
    public float hurtMaintainTime;
    // 출혈 동안 몇초나 지났는지 체크할 변수
    public float currentHurtDamageTime;

    #endregion
    
    /// <summary>
    /// 처음 한번만 실행하는 함수
    /// </summary>
    private void Start()
    {
        // 변수에 최대 Hp값을 넣어줌
        hpStat = playerSet.maxHpStat[playerSet.playerNum];
        maxMana = playerSet.manaStat[playerSet.playerNum];
        stagePlayerImage.sprite = playerSet.playerImages[playerSet.playerNum];
        quickSlotSetPlayerImage.sprite = playerSet.playerImages[playerSet.playerNum];
        speedStat = playerSet.speedStat[playerSet.playerNum];
        hpTxt.text = hpStat + "/" + playerSet.maxHpStat[playerSet.playerNum];
    }

    /// <summary>
    /// 매 프레임마다 호출하는 함수
    /// </summary>
    void Update()
    {
        hpTxtImage.fillAmount = Mathf.Lerp(hpTxtImage.fillAmount, hpStat / playerSet.maxHpStat[playerSet.playerNum], Time.deltaTime * 5f);
        hpImage.fillAmount = Mathf.Lerp(hpImage.fillAmount, hpStat / playerSet.maxHpStat[playerSet.playerNum], Time.deltaTime * 5f);

        if (backHpHit == true)
        {
            backSliderHp.fillAmount = Mathf.Lerp(backSliderHp.fillAmount, hpImage.fillAmount, Time.deltaTime * 5f);
            if (hpImage.fillAmount >= backSliderHp.fillAmount - 0.01f)
            {
                backHpHit = false;
                backSliderHp.fillAmount = hpImage.fillAmount;
            }
        }
        
        // 변수 값이 참이라면 아래 코드 실행 (전투 스테이지라면)
        if (stageManager.inStage == true && isDead == false)
        {
            // 왼쪽 화살표를 누르고 있으면서 변수 값이 거짓이라면 아래 코드 실행 (왼쪽으로 이동 할 수 있다면)
            if (Input.GetKey(KeyCode.LeftArrow) && _donLeftMove == false)
            {
                // 이 오브젝트의 위치를 좌측으로 스피드 * 실제시간 만큼 이동
                transform.position -= new Vector3(speedStat * Time.deltaTime , 0, 0);
            }

            // 오른쪽 화살표를 누르고 있으면서 변수 값이 거짓이라면 아래 코드 실행 (오른쪽으로 이동 할 수 있다면)
            if (Input.GetKey(KeyCode.RightArrow) && _donRightMove == false)
            {
                // 이 오브젝트의 위치를 우측으로 스피드 * 실제시간 만큼 이동
                transform.position += new Vector3(speedStat * Time.deltaTime, 0, 0);
            }
            
            // 함수 호출
            CheckObject();
            Hurt();
        }
    }

    /// <summary>
    /// 레이캐스트를 쏴서 어떤 오브젝트가 있는지 체크하는 함수
    /// </summary>
    private void CheckObject()
    {
        // 변수에 좌측으로 레이캐스트를 발사해 걸린 물체를 반환함
        RaycastHit2D rayLeftHit = Physics2D.BoxCast(transform.position,new Vector2(1f, 18), 0,Vector2.left, 3f, layerMask);
        // 변수에 우측으로 레이캐스트를 발사해 걸린 물체를 반환함
        RaycastHit2D rayRightHit = Physics2D.BoxCast(transform.position,new Vector2(1f, 18), 0, Vector2.right, 3.75f, layerMask);

        // 변수에 있는 콜라이더 값이 비어 있지 않다면 아래 코드 실행 (물체가 레이캐스트에 걸렸다면)
        if (rayLeftHit.collider != null)
        {
            // 변수에 참이라는 값을 넣음 (왼쪽으로 이동 중단)
            _donLeftMove = true;
        }
        // 변수에 있는 콜라이더 값이 비어있다면 아래 코드 실행 (물체가 레이캐스트에 걸리지 않았다면)
        else
        {
            // 변수에 거짓이라는 값을 넣음 (왼쪽으로 이동 가능)
            _donLeftMove = false;
        }

        // 변수에 있는 콜라이더 값이 비어 있지 않다면 아래 코드 실행 (물체가 레이캐스트에 걸렸다면)
        if (rayRightHit.collider != null)
        {
            // 변수에 참이라는 값을 넣음 (오른쪽으로 이동 중단)
            _donRightMove = true;
        }
        // 변수에 있는 콜라이더 값이 비어있다면 아래 코드 실행 (물체가 레이캐스트에 걸리지 않았다면)
        else
        {
            // 변수에 거짓이라는 값을 넣음 (오른쪽으로 이동 가능)
            _donRightMove = false;
        }
    }

    public void UpdateHpBar(float damage)
    {
        stagePlayerImage.color = new Color(153f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);
        Invoke("ResetImageAlpha", 0.5f);
        Invoke("BackHpFun", 0.5f);
        hpStat -= damage;

        hpTxt.text = hpStat + "/" + playerSet.maxHpStat[playerSet.playerNum];

        if (hpStat <= 0)
        {
            isDead = true;
            boxCol.enabled = false;
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
        Color alpha =  stagePlayerImage.color;

        // 알파 값이 255보다 작을 동안에 아래 코드 실행
        while (alpha.a < 1f)
        {
            // 실제 시간 / 설정한 딜레이 시간을 계산한 값을 변수에 넣음
            _unitAlpha += Time.deltaTime / _unitAlphaTime;

            // 알파 값 조절
            alpha.a = Mathf.Lerp(0, 1, _unitAlpha);

            stagePlayerImage.color = alpha;

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
            stagePlayerImage.color = alpha;
            
            yield return null;
        }

        yield return null;
        Destroy(gameObject);
    }

    private void ResetImageAlpha()
    {
        stagePlayerImage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
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
                UpdateHpBar(_unitSkillManager.hurtStat[3]);
                ShowDamageTxt(transform, _unitSkillManager.hurtStat[3].ToString(), false, hpImage.transform.position + new Vector3(0, 1, 0), new Color(128f / 255f, 0f / 255f, 255f / 255f));
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
}
