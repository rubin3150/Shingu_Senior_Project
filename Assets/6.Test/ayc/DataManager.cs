using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public TextMeshProUGUI[] nickname;
    
    public TextMeshProUGUI[] stageName;
    
    // Fade 이미지를 담을 변수
    [SerializeField] private Image fadeImage;

    [SerializeField] private GameObject LoseUI;
    
    [SerializeField] private GameObject winUI;
    
    // 실제 시간을 담을 변수
    private float _time;
    // 몇초 동안 페이드인 또는 페이드 아웃을 시킬지 정하는 변수 (1초로 설정)
    private float _currentFadeTime = 1f;

    void Start()
    {
        for(int i=0; i<nickname.Length; i++)
        {
            nickname[i].text = Data.Instance.nicknameBasket;
        }

        if (Data.Instance.isStage[0] == true)
        {
            for(int i=0; i<stageName.Length; i++)
            {
                stageName[i].text = "피터팬 - Boss";
            }
        }
        else if (Data.Instance.isStage[1] == true)
        {
            for(int i=0; i<stageName.Length; i++)
            {
                stageName[i].text = "미녀와 야수 - Boss";
            }
        }
    }
    
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
        
        Data.Instance.mainAudio.clip = Data.Instance.gameoverClip;
        Data.Instance.mainAudio.Play();
        
         fadeImage.gameObject.SetActive(false);
         LoseUI.SetActive(true);

        // 다음 1프레임까지 대기
        yield return null;
    }
    
    public void Fade1()
    {
        // 코루틴 실행
        StartCoroutine(FadeFlow1());
        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
    }

    /// <summary>
    /// 페이드 인 페이드 아웃 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeFlow1()
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
        
        Data.Instance.mainAudio.clip = Data.Instance.gameclearClip;
        Data.Instance.mainAudio.Play();
        
        fadeImage.gameObject.SetActive(false);
        winUI.SetActive(true);

        // 다음 1프레임까지 대기
        yield return null;
    }
}
