using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    #region 변수 영역

    // StageManager 스크립트를 담을 변수
    [SerializeField] private StageManager stageManager;
    
    // 유닛 선택 화면 오브젝트를 담을 변수
    [SerializeField] private GameObject selectUnitBase;
    // 확인 창 오브젝트를 담을 변수
    [SerializeField] private GameObject confirmationWindow;
    // 달빛 에너지 오브젝트를 담을 변수
    [SerializeField] private GameObject moonEnergyBackImage;

    [SerializeField] private GameObject stagePlayerInfo;
    
    // 퀵슬롯 배경 이미지의 위치를 담을 변수
    [SerializeField] private RectTransform quickBackImage;
    
    // 퀵슬롯의 이미지를 담을 변수 
    [SerializeField] private RectTransform[] quickSlotImage;
    [SerializeField] private RectTransform[] quickSlotUnit;
    [SerializeField] private RectTransform[] quickSlotUnitImage;
    // 퀵슬롯 이미지들의 위치
    [SerializeField] private RectTransform quickSlotUnits;
    // 달빛 에너지 이미지의 위치를 담을 변수
    [SerializeField] private RectTransform moonEnergyImage;

    [SerializeField] private Sprite quickBackChangeImage;
    [SerializeField] private Sprite unitBorderChangeImage;
    [SerializeField] private Sprite unitChangeImage;

    // Fade 이미지를 담을 변수
    [SerializeField] private Image fadeImage;
    
    
    [SerializeField] public RectTransform[] skillBorderImage;
    [SerializeField] private RectTransform[] skillImage;

    [SerializeField] private SkillManager skillManager;

    [SerializeField] Button[] btn;

    [SerializeField] private GameObject selectUnitTxt;

    [SerializeField] private Animator animator;

    // 실제 시간을 담을 변수
    private float _time;
    // 몇초 동안 페이드인 또는 페이드 아웃을 시킬지 정하는 변수 (1초로 설정)
    private float _currentFadeTime = 1f;

    #endregion

    /// <summary>
    /// 코루틴을 호출시키는 함수
    /// </summary>
    public void Fade()
    {
        // 코루틴 실행
        StartCoroutine(FadeFlow());
        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
    }

    /// <summary>
    /// 페이드 인 페이드 아웃 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeFlow()
    {
        // fadeImage를 활성화 시킴
        fadeImage.gameObject.SetActive(true);
        
        // 변수에 0이라는 값을 넣어줌 (변수 초기화)
        _time = 0f;
        
        // 임시 변수에 fadeImage의 컬러 값을 넣어줌
        Color alpha = fadeImage.color;
        
        // 임시 변수 값이 1보다 작을 동안 아래 코드 실행 (fadeImage의 알파 값이 255보다 작다면)
        while (alpha.a < 1f)
        {
            // 실제시간 / 설정한 시간을 계산해 변수에 더해줌
            _time += Time.deltaTime / _currentFadeTime;
            
            // 임시 변수에 위에 계산한 값을 넣어줌
            alpha.a = Mathf.Lerp(0, 1, _time);
            
            // 설정한 색을 fadeImage에 넣어줌
            fadeImage.color = alpha;
            
            // 다음 1프레임까지 대기
            yield return null;
        }
        // 변수에 0이라는 값을 넣어줌
        _time = 0f;

        // 0.1초 까지 대기 
        yield return new WaitForSeconds(0.1f);
        
        Data.Instance.mainAudio.clip = Data.Instance.defenseClip;
        Data.Instance.mainAudio.Play();
        animator.SetTrigger("isFlag");

        // 임시 변수 값이 0보다 큰 동안 아래 코드 실행 (fadeImage의 알파 값이 255보다 크다면)
        while (alpha.a > 0f)
        {
            // 실제시간 / 설정한 시간을 계산해 변수에 더해줌
            _time += Time.deltaTime / _currentFadeTime;
            
            // 임시 변수에 위에 계산한 값을 넣어줌
            alpha.a = Mathf.Lerp(1, 0, _time);
            
            // 설정한 색을 fadeImage에 넣어줌
            fadeImage.color = alpha;
            
            // 유닛 선택 화면 오브젝트를 비활성화 시킴
            selectUnitBase.SetActive(false);
            // 확인 창 오브젝트를 비활성화 시킴
            confirmationWindow.SetActive(false);
            // 달빛 에너지 이미지 오브젝트를 활성화 시킴
            moonEnergyBackImage.SetActive(true);
            
            selectUnitTxt.SetActive(false);
            
            stagePlayerInfo.SetActive(true);
            
            // 0 / 달빛 에너지 최대치로 설정 후 변수에 넣음
            stageManager.moonText.text = 0 + " / " + stageManager.maxMoonEnergy;

            quickBackImage.transform.GetComponent<Image>().sprite = quickBackChangeImage;
            // 크기 변경
            quickBackImage.sizeDelta = new Vector2(1920, 188);
            // 퀵슬롯의 배경 이미지의 위치 변경
            quickBackImage.localPosition = new Vector3(0f, -446f, 0);

            quickSlotUnits.localPosition = new Vector3(-420f, 0f, 0);
            // 달빛 에너지 이미지 위치 변경
            moonEnergyImage.localPosition = new Vector3(-450f, -327.5f, 0);

            for (int i = 0; i < quickSlotImage.Length; i++)
            {
                quickSlotImage[i].GetComponent<Image>().sprite = unitBorderChangeImage;
                quickSlotImage[i].sizeDelta = new Vector2(150, 150);
                quickSlotImage[i].localPosition = new Vector3(-395 + 185 * i, 4.75f, 0);
                
                quickSlotUnit[i].sizeDelta = new Vector2(140, 140);
                quickSlotUnit[i].GetComponent<Image>().sprite = unitChangeImage;
                quickSlotUnit[i].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f / 255f);
                quickSlotUnitImage[i].sizeDelta = new Vector2(300, 300);
            }

            for (int i = 0; i < skillBorderImage.Length; i++)
            {
                skillBorderImage[i].sizeDelta = new Vector2(201, 201);
                skillBorderImage[i].localScale = new Vector3(1, 1, 1);

                if (i == 0)
                {
                    skillBorderImage[i].localPosition = new Vector3(-425, -226, 0);
                    skillImage[i].localPosition = new Vector2(-0.25f, 10);
                    skillImage[i].sizeDelta = new Vector2(500, 500);
                    skillImage[i].localScale = new Vector3(0.375f, 0.375f, 0.375f);
                }
                else if (i == 1)
                {
                    skillBorderImage[i].localPosition = new Vector3(-165.5f, -226, 0);
                    skillImage[i].localPosition = new Vector2(0, -16);
                    skillImage[i].sizeDelta = new Vector2(500, 500);
                    skillImage[i].localScale = new Vector3(0.3f, 0.3f, 0.3f);
                }
                else
                {
                    skillBorderImage[i].localPosition = new Vector3(95, -226, 0);
                    skillImage[i].localPosition = new Vector2(0, -2.8f);
                    skillImage[i].sizeDelta = new Vector2(128, 128);
                    skillImage[i].localScale = new Vector3(0.9f, 0.9f, 0.9f);
                }
                // // skillImage[i].localPosition = new Vector2(7.5f, 20);
                //     if (i == 1)
                //     {
                //         skillImage[i].localPosition = new Vector2(5f, -10);
                //     }
                //     skillImage[i].sizeDelta = new Vector2(1, 1);
                //
                // else
                // {
                //     skillImage[i].localPosition = new Vector2(5, 5);
                //     skillImage[i].sizeDelta = new Vector2(128, 128);
                // }

                if (skillManager.isActive[i] == true)
                {
                    skillBorderImage[i].GetComponent<Image>().color =
                        new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                    btn[i].enabled = true;
                }
            }
            // 함수 호출
            stageManager.SetActiveUnitText();

            stageManager.inStages = true;

            // 다음 1프레임까지 대기
            yield return null;
        }
        // fadeImage를 비활성화 시킴
         fadeImage.gameObject.SetActive(false);

        // 다음 1프레임까지 대기
        yield return null;
    }

    
}
