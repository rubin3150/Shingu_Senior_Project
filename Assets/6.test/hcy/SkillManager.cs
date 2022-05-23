using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Effekseer;
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
        
    [SerializeField] private Text txtSkillName;
    [SerializeField] private Text txtSkillDesc;
    [SerializeField] private Text txtSkillHowToUsed;
    
    // 스킬 마다 다른 이펙트 설정
    public EffekseerEmitter[] skillEffect;

    public float[] currentMaintain;

    public bool[] isMaintain;

    [SerializeField] private float range;

    [SerializeField] private LayerMask unitLayer;

    [SerializeField] private Collider2D[] units;
    [SerializeField] private UnitMove[] setUnits;
    
    private UnitMove _unitMove;
    
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
            txtSkillDesc.text += "\n스킬 가이드 : 사거리 내에 있는 모든 탱커 유\n닛의 체력을 " + playerSet.addStat[currentSkillNum] + "만큼 상승시킨다";
        }
        else if (currentSkillNum == 1)
        {
            txtSkillDesc.text += "\n스킬 가이드 : 사거리 내에 있는 모든 근거리\n딜러 유닛의 공격력을 " + playerSet.addStat[currentSkillNum] +
                                 "만큼 상승시킨다";
        }
        else
        {
            txtSkillDesc.text += "\n스킬 가이드 : 사거리 내에 있는 모든 원거리\n딜러 유닛의 치명타 확률을 " + playerSet.addStat[currentSkillNum] + "만큼 상승시킨\n다";
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
        if (stageManager.inStage == true)
        {
            if (skillNum == 0 && isActive[0] == true)
            {
                if (stageManager.nowMoonEnergy >= playerSet.skillMoonEnergy[skillNum])
                {
                    ResetMaintain(1);
                    ResetMaintain(2);

                    // 첫번째 스킬 호출
                    SkillEffect(6);
                }
            }
            else if (skillNum == 1 && isActive[1] == true)
            {
                if (stageManager.nowMoonEnergy >= playerSet.skillMoonEnergy[skillNum])
                {
                    ResetMaintain(0);
                    ResetMaintain(2);

                    SkillEffect(7);
                }
            }
            else if (skillNum == 2 && isActive[2] == true)
            {
                if (stageManager.nowMoonEnergy >= playerSet.skillMoonEnergy[skillNum])
                {
                    ResetMaintain(0);
                    ResetMaintain(1);

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
            stageManager.nowMoonEnergy -= playerSet.skillMoonEnergy[num - 6];

                for (int i = 0; i < skillImage.Length; i++)
                {
                    skillEffect[i].Stop();
                }
                
                skillEffect[num - 6].Play();
                
                Maintain(num - 6);
        }
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

        if (isMaintain[1] == true)
        {
            currentMaintain[1] -= Time.deltaTime;

            if (currentMaintain[1] <= 0)
            {
                
                skillEffect[1].Stop();
                isMaintain[1] = false;
                coolTimeManager.CoolTime(7, true);
                ResetSetSkill();
            }
        }
        
        if (isMaintain[2] == true)
        {
            currentMaintain[2] -= Time.deltaTime;

            if (currentMaintain[2] <= 0)
            {
                skillEffect[2].Stop();
                isMaintain[2] = false;
                coolTimeManager.CoolTime(8, true);
                ResetSetSkill();
            }
        }
    }
    
    private void Update()
    {
        if (stageManager.inStage == true)
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
            units = Physics2D.OverlapCircleAll(transform.position, range, unitLayer);
            
            for (int i = 0; i < units.Length; i++)
            {
                Transform _targetTf = units[i].transform;

                if (_targetTf.transform.tag == "Unit")
                {
                    _unitMove = _targetTf.transform.GetComponent<UnitMove>();
                    
                    // 탱커 버프 스킬 발동 
                    if (_unitMove.unitType == "Tanker")
                    {
                        if (_unitMove.isBuff == false)
                        {
                            setUnits[k] = _unitMove;
                            k++;
                            _unitMove.isBuff = true;
                            if (_unitMove.nowHpStat == _unitMove.maxHp)
                            {
                                _unitMove.maxHp += playerSet.addStat[0];
                                _unitMove.nowHpStat += playerSet.addStat[0];
                                _unitMove.UpdateHpBar(0, false);
                            }
                            else
                            {
                                _unitMove.nowHpStat += playerSet.addStat[0];
                                _unitMove.UpdateHpBar(0, false);
                            }

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
                        // 탱커 버프 스킬 발동 
                        if (unitMove.unitType == "Tanker")
                        {
                            if (unitMove.isBuff == true)
                            {
                                float dis = Vector2.Distance(transform.position, unitMove.transform.position);
                                if (dis >= 19f)
                                {
                                    unitMove.isBuff = false;
                                    if (unitMove.maxHp > unitMove.unit.hpStat)
                                    {
                                        unitMove.maxHp -= playerSet.addStat[0];
                                        unitMove.nowHpStat -= playerSet.addStat[0];
                                        unitMove.UpdateHpBar(0, false);
                                    }
                                    else
                                    {
                                        unitMove.nowHpStat -= playerSet.addStat[0];
                                        unitMove.UpdateHpBar(0, false);
                                    }
                                    setUnits[i] = null;
                                }
                            }
                        }
                    }
                }
            }
        }
        // 두번째 스킬이 사용중이라면 아래 코드 실행
        else if (isMaintain[1] == true)
        {
            units = Physics2D.OverlapCircleAll(transform.position, range, unitLayer);
            
            for (int i = 0; i < units.Length; i++)
            {
                Transform _targetTf = units[i].transform;

                if (_targetTf.transform.tag == "Unit")
                {
                    _unitMove = _targetTf.transform.GetComponent<UnitMove>();
                    
                    // 탱커 버프 스킬 발동 
                    if (_unitMove.unitType == "Bruiser")
                    {
                        if (_unitMove.isBuff == false)
                        {
                            setUnits[k] = _unitMove;
                            k++;
                            _unitMove.isBuff = true;
                            _unitMove.attack += playerSet.addStat[1];
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
                        // 탱커 버프 스킬 발동 
                        if (unitMove.unitType == "Bruiser")
                        {
                            if (unitMove.isBuff == true)
                            {
                                float dis = Vector2.Distance(transform.position, unitMove.transform.position);
                                if (dis >= 19f)
                                {
                                    unitMove.isBuff = false;
                                    unitMove.attack -= playerSet.addStat[1];
                                    setUnits[i] = null;
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (isMaintain[2] == true)
        {
            units = Physics2D.OverlapCircleAll(transform.position, range, unitLayer);
            
            for (int i = 0; i < units.Length; i++)
            {
                Transform _targetTf = units[i].transform;

                if (_targetTf.transform.tag == "Unit")
                {
                    _unitMove = _targetTf.transform.GetComponent<UnitMove>();
                    
                    // 탱커 버프 스킬 발동 
                    if (_unitMove.unitType == "Melee")
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
                        // 탱커 버프 스킬 발동 
                        if (unitMove.unitType == "Melee")
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
                                    setUnits[i] = null;
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
                    // 탱커 유닛 스킬 발동 
                    if (unitMove.unitType == "Tanker")
                    {
                        if (unitMove.isBuff == true)
                        {
                            unitMove.isBuff = false;
                            if (unitMove.maxHp > unitMove.unit.hpStat)
                            {
                                unitMove.maxHp -= playerSet.addStat[0];
                                unitMove.nowHpStat -= playerSet.addStat[0];
                                unitMove.UpdateHpBar(0, false);
                            }
                            else
                            {
                                unitMove.nowHpStat -= playerSet.addStat[0];
                                unitMove.UpdateHpBar(0, false);
                            }
                            setUnits[i] = null;
                        }
                    }
                    else if (unitMove.unitType == "Bruiser")
                    {
                        if (unitMove.isBuff == true)
                        {
                            unitMove.isBuff = false;
                            unitMove.attack -= playerSet.addStat[1];
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
                            setUnits[i] = null;
                        }
                    }
                }
            }
        }
        k = 0;
    }
}
