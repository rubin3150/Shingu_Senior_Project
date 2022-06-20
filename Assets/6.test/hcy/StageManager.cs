using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Effekseer;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    // 스테이지에 진입 했는지 아닌지 체크할 변수
    public bool inStages;
    public bool inStage;

    public GameObject[] spawnUnitTxt;
    public GameObject[] moonEnergyText;
    public GameObject[] unitMoonIcon;

    public GameObject[] skillTxt;
    public GameObject[] skillManaTxt;
    public GameObject[] skillManaIcon;

    public SelectUnitBase selectUnitBase;

    [SerializeField] private PlayerSet playerSet;

    [SerializeField] private Skill skill;

    [SerializeField] private GameObject stopUI;

    public bool isStop;

    public float maxMoonEnergy;
    public float nowMoonEnergy;
    public float maxMana;
    public float nowMana;
    
    public Image maxMoonEnergyImage;
    public Image manaImage;
        
    public TextMeshProUGUI moonText;
    public TextMeshProUGUI manaText;

    public float moonEnergySpeed;
    
    public float manaSpeed;

    [SerializeField] private Transform point;
    
    [SerializeField] private Collider2D[] player;
    
    [SerializeField] private float range;

    [SerializeField] private LayerMask unitLayer;

    private bool isPlayer;

    [SerializeField] private Transform playerTf;

    [SerializeField] private SkillManager _skillManager;
    
    private void Start() {
        Data.Instance.mainAudio.clip = Data.Instance.selectunitClip;
        Data.Instance.mainAudio.Play();
    }

    public void SetActiveUnitText()
    {
        for (int i = 0; i < skillTxt.Length; i++)
        {
            if (_skillManager.isActive[i] == true)
            {
                skillManaIcon[i].SetActive(true);
                skillTxt[i].SetActive(true);
                skillManaTxt[i].SetActive(true);
                skillManaTxt[i].GetComponentInChildren<TextMeshProUGUI>().text = playerSet.skillMoonEnergy[skill.skillNum + i].ToString();
            }
            else
            {
                break;
            }
        }
        
        for (int i = 0; i < spawnUnitTxt.Length; i++)
        {
            if (selectUnitBase.quickSlot[i].unit != null)
            {
                unitMoonIcon[i].SetActive(true);
                spawnUnitTxt[i].SetActive(true);
                moonEnergyText[i].SetActive(true);
                moonEnergyText[i].GetComponentInChildren<TextMeshProUGUI>().text = selectUnitBase.quickSlot[i].unit.moonEnergy.ToString();
            }
            else
            {
                return;
            }
        }
    }

    private void Update()
    {
        if (inStage == true && isStop == false)
        {
            CheckPlayer();
            UpdateMoonBar();

            if (isPlayer == true)
            {
                UpdateManaBar();
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CheckSpawnUnit(0);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                CheckSpawnUnit(1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                CheckSpawnUnit(2);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                CheckSpawnUnit(3);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                CheckSpawnUnit(4);
            }
        }
    }

    /// <summary>
    /// 유닛을 소환할 수 있는 체크
    /// </summary>
    private void CheckSpawnUnit(int num)
    {
        if (selectUnitBase.quickSlot[num].unit != null)
        {
            selectUnitBase.quickSlot[num].SpawnUnit(num + 1);
        }
    }
    
    private void UpdateMoonBar()
    {
        if (nowMoonEnergy >= 100)
        {
            nowMoonEnergy = 100;
        }
        else
        {
            nowMoonEnergy += moonEnergySpeed * Time.deltaTime;
        }
        moonText.text = Mathf.Floor(nowMoonEnergy) + " / " + maxMoonEnergy;
        // 체력 게이지 값 설정.
        maxMoonEnergyImage.fillAmount = nowMoonEnergy / maxMoonEnergy;
        
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }

    private void CheckPlayer()
    {
        player = Physics2D.OverlapCircleAll(point.transform.position, range, unitLayer);
            
        for (int i = 0; i < player.Length; i++)
        {
            Transform _targetTf = player[i].transform;

            if (_targetTf.transform.tag == "Player")
            {
                isPlayer = true;
            }
        }
        
        //24.5
        if (isPlayer == true)
        {
            float dis = Vector2.Distance(point.transform.position, playerTf.position);

            if (dis >= 24.5f)
            {
                isPlayer = false;
            }
        }
    }
    
    public void UpdateManaBar()
    {
        if (nowMana >= 100)
        {
            nowMana = 100;
        }
        else
        {
            nowMana += manaSpeed * Time.deltaTime;
        }
        manaText.text = Mathf.Floor(nowMana) + " / " + maxMana;
        // 체력 게이지 값 설정.
        manaImage.fillAmount = nowMana / maxMana;
        
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }

    public void StopUIBtn()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
        stopUI.SetActive(true);
        Time.timeScale = 0;
        isStop = true;
    }

    public void RestartUIBtn()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
        stopUI.SetActive(false);
        Time.timeScale = 1;
        isStop = false;
    }

    public void GiveUpBtn()
    {
        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
        SceneManager.LoadScene("Tycoon");
        stopUI.SetActive(false);
        Time.timeScale = 1;
        isStop = false;
    }
    
    public void GoToWorld()
    {
        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
        SceneManager.LoadScene("Tycoon");
    }
}
