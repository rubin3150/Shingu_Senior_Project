using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class SkillManager : MonoBehaviour
{
    public Image[] skillImage;
    
    public bool[] isActive;
    
    private int _currentSkillNum;
    
    [SerializeField] private PlayerSet playerSet;

    [SerializeField] private CoolTimeManager coolTimeManager;

    [SerializeField] private StageManager stageManager;
    
    [SerializeField] private GameObject goBase;
        
    [SerializeField] private TextMeshProUGUI txtSkillName;
    [SerializeField] private TextMeshProUGUI txtSkillDesc;
    [SerializeField] private TextMeshProUGUI txtSkillHowToUsed;
    
    // 스킬 마다 다른 이펙트 설정
    public EffekseerEmitter[] skillEffect;

    public float[] currentMaintain;

    public bool[] isMaintain;

    [SerializeField] private float range;

    [SerializeField] private LayerMask unitLayer;

    [SerializeField] private Collider2D[] collider2Ds;
    [SerializeField] private UnitMove[] unitHPs;
    [SerializeField] private UnitMove[] setUnits;
    
    private UnitMove _unitMove;

    [SerializeField] private GameObject[] units;
    
    [SerializeField] private GameObject damageText;
     
    private int k = 0;
    
    private void Start()
    {
        _currentSkillNum = playerSet.playerNum * 3;

        for (int i = 0; i < skillImage.Length; i++)
        { 
            skillImage[i].sprite = playerSet.skillImage[_currentSkillNum + i];
        }
        
        SetColor();
    }
    
    public void SetColor()
    {
        for (int i = 0; i < skillImage.Length; i++)
        {
            if (isActive[i] == true)
            {
                skillImage[i].color = Color.white;
            }
            else
            {
                skillImage[i].color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 125f / 255f);
            }
        }
    }
    
    public void ShowSkillToolTip(Vector3 pos, int currentSkillNum, int skillNum)
    {
        goBase.SetActive(true);
    
        if (stageManager.inStage == true)
        {
            pos += new Vector3(-goBase.GetComponent<RectTransform>().rect.width * 0.5f, goBase.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        }
        else
        {
            pos += new Vector3(-goBase.GetComponent<RectTransform>().rect.width * 0.5f, -goBase.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        }
        
        goBase.transform.position = pos;
        
        txtSkillName.text = playerSet.skillName[currentSkillNum];
    
        txtSkillDesc.text = "소비하는 달빛 에너지 : " + playerSet.skillMoonEnergy[currentSkillNum] + "\n스킬 사거리 : " + playerSet.skillRange[currentSkillNum] + "\n스킬 쿨타임 : " +
                            playerSet.skillCoolTime[currentSkillNum] + "초" + "\n스킬 유지 시간 : " +
                            playerSet.maintainTime[currentSkillNum] + "초";
        
        if (currentSkillNum == 0)
        {
            txtSkillDesc.text += "\n스킬 가이드 : 사거리 내에 있는 모든 힐러 유닛 회복력을 " + playerSet.addStat[0] + "," + "모든 근거리 딜러 유닛 공격력을 " + playerSet.addStat[1] + "," + "모든 원거리 딜러 유닛 치명타 확률을 " + playerSet.addStat[2] + "," + "모든 탱커 유닛의 넉백 저항 수치를 " + playerSet.addStat[3] + ", " + "모든 유닛들의 이동속도를 " + playerSet.addStat[4] + "만큼 증가 시킨다";
        }
        else if (currentSkillNum == 1)
        {
            txtSkillDesc.text += "\n스킬 가이드 : 소환된 모든 아군 유닛들의 체력을 최대 체력으로 회복시키고 유닛의 스킬 쿨타임을 " + playerSet.addStat[5] +
                                 "만큼 감소시킨다";
        }
        else
        {
            txtSkillDesc.text += "\n스킬 가이드 : 범위 안에 있는 모든 적군 유닛을 " + playerSet.addStat[6] + "~" + playerSet.addStat[7] + "랜덤 넉백 수치 만큼 넉백하고 고정 데미지 " + playerSet.addStat[8] + "를 입힌다";
        }
        
        if (stageManager.inStage == false)
        {
            txtSkillHowToUsed.text = "";
        } 
        else
        {
            if (skillNum == 0)
            {
                txtSkillHowToUsed.text = "좌클릭 or (Z) - 사용";
            }
            else if (skillNum == 1)
            {
                txtSkillHowToUsed.text = "좌클릭 or (X) - 사용";
            }
            else
            {
                txtSkillHowToUsed.text = "좌클릭 or (C) - 사용";
            }
        }
    }

    public void HideSkillToolTip()
    {
        goBase.SetActive(false);
    }
    
    public void UseSkill(int skillNum)
    {
        if (stageManager.inStage == true && stageManager.isStop == false)
        {
            if (skillNum == 0 && isActive[0] == true)
            {
                if (stageManager.nowMana >= playerSet.skillMoonEnergy[skillNum])
                {
                    // 첫번째 스킬 호출
                    SkillEffect(6);
                }
            }
            else if (skillNum == 1 && isActive[1] == true)
            {
                if (stageManager.nowMana >= playerSet.skillMoonEnergy[skillNum])
                {
                    SkillEffect(7);
                }
            }
            else if (skillNum == 2 && isActive[2] == true)
            {
                if (stageManager.nowMana >= playerSet.skillMoonEnergy[skillNum])
                {
                    SkillEffect(8);
                }
            }
        }
    }

    private void ResetMaintain(int num)
    {
        if (isMaintain[num] == true)
        {
            skillEffect[num].Stop();
            isMaintain[num] = false;
            coolTimeManager.CoolTime(num + 6, true);
        }
    }
    
    public void SkillEffect(int num)
    {
        if (!coolTimeManager.isCoolTime[num - 1] && isMaintain[num - 6] == false)
        {
            stageManager.nowMana -= playerSet.skillMoonEnergy[num - 6];
            stageManager.UpdateManaBar();
            
            skillEffect[num - 6].Play();

            if (num == 6)
            {
                Maintain(num - 6);
            }
            else if (num == 7)
            {
                Skill2();
            }
            else if (num == 8)
            {
                Skill3();
            }
        }
    }
    
    private void Skill2()
    {
        coolTimeManager.CoolTime(7, true);

        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] != null)
            {
                // 유닛 쿨타임 감소 구현해야함
                UnitMove _unitMove = units[i].GetComponent<UnitMove>();

                if (_unitMove.unit.unitName == "팅커벨")
                {
                    ShowDamageTxt(_unitMove, _unitMove.maxHp - _unitMove.nowHpStat, new Vector3(0, 9.5f, 0));
                }
                else
                {
                    ShowDamageTxt(_unitMove, _unitMove.maxHp - _unitMove.nowHpStat, new Vector3(0, 5.5f, 0));
                }
                
                _unitMove.nowHpStat = _unitMove.maxHp;
                _unitMove.UpdateHpBar(0, false);
            }
        }

        Invoke("HideEffectSkill2", 2f);
    }
    
    private void ShowDamageTxt(UnitMove unitMove, float damage, Vector3 yPos)
    {
        GameObject damageGo = Instantiate(damageText);
        damageGo.transform.parent = unitMove.transform;
        damageGo.transform.position = unitMove.transform.position + yPos; // 일반 유닛 5.5 // 팅커벨 유닛 7.5
        damageGo.GetComponent<DamageText>().text.color = Color.green;
        damageGo.GetComponent<DamageText>().damage = damage;
        
        damageGo.GetComponent<DamageText>().isCri = false;
    }

    public void AddUnit(GameObject unit)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i] == null)
            {
                units[i] = unit;
                break;
            }
        }
    }

    private void Skill3()
    {
        // 스킬 구현 전에 적 먼저 만들기
        Invoke("HideEffectSkill2", 2f);
    }

    private void HideEffectSkill2()
    {
        skillEffect[1].Stop();
    }

    private void HideEffectSkill3()
    {
        skillEffect[2].Stop();
    }
    
    public void Maintain(int num)
    {
        currentMaintain[num] = playerSet.maintainTime[num];

        // 배열 인자값 - 1번째 변수에 참이라는 값을 넣어줌 (쿨타임 실행 가능)
        isMaintain[num] = true;
    }

    private void MaintainCalc()
    {
        if (isMaintain[0] == true)
        {
            currentMaintain[0] -= Time.deltaTime;
            
            if (currentMaintain[0] <= 0)
            {
                skillEffect[0].Stop();
                isMaintain[0] = false;
                coolTimeManager.CoolTime(6, true);
                ResetSetSkill();
            }
        }
    }
    
    private void Update()
    {
        if (stageManager.inStage == true && stageManager.isStop == false)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UseSkill(0);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                UseSkill(1);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                UseSkill(2);
            }
            
            MaintainCalc();
            SetSkill();
        }
    }

    
    private void SetSkill()
    {
        // 첫번째 스킬이 사용중이라면 아래 코드 실행 
        if (isMaintain[0] == true)
        {
            collider2Ds = Physics2D.OverlapCircleAll(transform.position, range, unitLayer);
            
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                Transform _targetTf = collider2Ds[i].transform;

                if (_targetTf.transform.tag == "Unit")
                {
                    _unitMove = _targetTf.transform.GetComponent<UnitMove>();
                    
                    //  힐러 유닛 능력치 증가
                    if (_unitMove.unitType == "Healer")
                    {
                        if (_unitMove.isBuff == false)
                        {
                            setUnits[k] = _unitMove;
                            k++;
                            _unitMove.isBuff = true;
                            _unitMove.heal += playerSet.addStat[0];
                            _unitMove.moveSpeed += playerSet.addStat[4];
                            _unitMove = null;
                        }
                    }
                    // 근거리 딜러 유닛 능력치 증가
                    else if (_unitMove.unitType == "Bruiser")
                    {
                        if (_unitMove.isBuff == false)
                        {
                            setUnits[k] = _unitMove;
                            k++;
                            _unitMove.isBuff = true;
                            _unitMove.attack += playerSet.addStat[1];
                            _unitMove.moveSpeed += playerSet.addStat[4];
                            _unitMove = null;
                        }
                    }
                    // 원거리 딜러 유닛 능력치 증가
                    else if (_unitMove.unitType == "Melee")
                    {
                        if (_unitMove.isBuff == false)
                        {
                            setUnits[k] = _unitMove;
                            k++;
                            _unitMove.isBuff = true;
                            
                            _unitMove.criRate += playerSet.addStat[2];

                            if (_unitMove.criRate >= 100)
                            {
                                _unitMove.criRate = 100;
                            }
                            _unitMove.moveSpeed += playerSet.addStat[4];
                            _unitMove = null;
                        }
                    }
                    // 탱커 버프 스킬 발동 
                    else if (_unitMove.unitType == "Tanker")
                    {
                        if (_unitMove.isBuff == false)
                        {
                            setUnits[k] = _unitMove;
                            k++;
                            _unitMove.isBuff = true;
                            _unitMove.pushResist += playerSet.addStat[3];
                            _unitMove.moveSpeed += playerSet.addStat[4];
                            _unitMove = null;
                        }
                    }
                }
            }
            
            // 버프 사거리 밖으로 나갈 시 아래 코드 실행 
            for (int i = 0; i < setUnits.Length; i++)
            {
                if (setUnits[i] != null)
                {
                    if (setUnits[i].transform.tag == "Unit")
                    {
                        UnitMove unitMove = setUnits[i].transform.GetComponent<UnitMove>();
                        
                        if (setUnits[i].transform.tag == "Unit")
                        {
                            // 힐러 유닛 능력치 감소
                            if (unitMove.unitType == "Healer")
                            {
                                if (unitMove.isBuff == true)
                                {
                                    float dis = Vector2.Distance(transform.position, unitMove.transform.position);
                                    
                                    if (dis >= 19f)
                                    {
                                        unitMove.isBuff = false;
                                        unitMove.heal -= playerSet.addStat[0];
                                        unitMove.moveSpeed -= playerSet.addStat[4];
                                        setUnits[i] = null;
                                    }
                                }
                            }
                            // 근거리 유닛 능력치 감소
                            else if (unitMove.unitType == "Bruiser")
                            {
                                if (unitMove.isBuff == true)
                                {
                                    float dis = Vector2.Distance(transform.position, unitMove.transform.position);
                                    
                                    if (dis >= 19f)
                                    {
                                        unitMove.isBuff = false;
                                        unitMove.attack -= playerSet.addStat[1];
                                        unitMove.moveSpeed -= playerSet.addStat[4];
                                        setUnits[i] = null;
                                    }
                                }
                            }
                            // 원거리 유닛 능력치 감소
                            else if (unitMove.unitType == "Melee")
                            {
                                if (unitMove.isBuff == true)
                                {
                                    float dis = Vector2.Distance(transform.position, unitMove.transform.position);
                                    if (dis >= 19f)
                                    {
                                        unitMove.isBuff = false;
                                        if (unitMove.criRate > unitMove.unit.criRate)
                                        {
                                            unitMove.criRate -= playerSet.addStat[2];
                                        }
                                        unitMove.moveSpeed -= playerSet.addStat[4];
                                        setUnits[i] = null;
                                    }
                                }
                            }
                            // 탱커 유닛 능력치 감소
                            else if (unitMove.unitType == "Tanker")
                            {
                                if (unitMove.isBuff == true)
                                {
                                    float dis = Vector2.Distance(transform.position, unitMove.transform.position);
                                    
                                    if (dis >= 19f)
                                    {
                                        unitMove.isBuff = false;
                                        unitMove.pushResist -= playerSet.addStat[3];
                                        unitMove.moveSpeed -= playerSet.addStat[4];
                                        setUnits[i] = null;
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
        }
    }

    /// <summary>
    /// 스킬로 증가한 스텟을 초기화하는 함수
    /// </summary>
    private void ResetSetSkill()
    {
        for (int i = 0; i < setUnits.Length; i++)
        {
            if (setUnits[i] != null)
            {
                if (setUnits[i].transform.tag == "Unit")
                {
                    UnitMove unitMove = setUnits[i].transform.GetComponent<UnitMove>();

                    if (unitMove.unitType == "Healer")
                    {
                        if (unitMove.isBuff == true)
                        {
                            unitMove.isBuff = false;
                            unitMove.heal -= playerSet.addStat[0];
                            unitMove.moveSpeed -= playerSet.addStat[4];
                            setUnits[i] = null;
                        }
                    }
                    else if (unitMove.unitType == "Bruiser")
                    {
                        if (unitMove.isBuff == true)
                        {
                            unitMove.isBuff = false;
                            unitMove.attack -= playerSet.addStat[1];
                            unitMove.moveSpeed -= playerSet.addStat[4];
                            setUnits[i] = null;
                        }
                    }
                    else if (unitMove.unitType == "Melee")
                    {
                        if (unitMove.isBuff == true)
                        {
                            unitMove.isBuff = false;
                            if (unitMove.criRate > unitMove.unit.criRate)
                            {
                                unitMove.criRate -= playerSet.addStat[2];
                            }
                            unitMove.moveSpeed -= playerSet.addStat[4];
                            setUnits[i] = null;
                        }
                    }
                    else if (unitMove.unitType == "Tanker")
                    {
                        if (unitMove.isBuff == true)
                        {
                            unitMove.isBuff = false;
                            unitMove.pushResist -= playerSet.addStat[3];
                            unitMove.moveSpeed -= playerSet.addStat[4];
                            setUnits[i] = null;
                        }
                    }
                }
            }
        }
        k = 0;
    }
}
