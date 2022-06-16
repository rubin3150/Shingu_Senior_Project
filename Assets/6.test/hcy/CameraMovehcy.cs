using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CameraMovehcy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region 변수 영역

    // 키메라 오브젝트를 담을 변수
    [SerializeField] private Camera theCam;
    
    // StageManager 스크립트를 담을 변수 
    [SerializeField] private StageManager stageManager;
    
    // 왼쪽 영역인지 아닌지 체크할 변수
    [SerializeField] private bool isLeft;

    // 카메라의 이동 속도를 담을 변수 
    [SerializeField] private float mouseCameraSpeed;

    // 카메라가 영역에 들어왔는지 아닌지 체크할 변수
    public bool _dragMouse;

    #endregion
    
    /// <summary>
    /// 매 프레임마다 호출되는 함수
    /// </summary>
    private void Update()
    {
        // 마우스 포인트가 영역에 들어와 있으면서 전투 스테이지라면 아래 코드 실행
        if (stageManager.inStage == true)
        {
            if (_dragMouse == true)
            {
                // 변수 값이 참이라면 아래 코드 실행 (왼쪽 영역이라면)
                if (isLeft == true && theCam.transform.position.x > -13.5f)
                {
                    // 카메라의 위치를 카메라 스피드 만큼 좌측으로 이동
                    theCam.transform.position -= new Vector3(mouseCameraSpeed  * Time.deltaTime, 0, 0);
                }
                // 변수 값이 거짓이라면 아래 코드 실행 (오른쪽 영역이라면)
                else if (isLeft == false && theCam.transform.position.x < 32.5f)
                {
                    // 카메라의 위치를 카메라 스피드 만큼 우측으로 이동
                    theCam.transform.position += new Vector3(mouseCameraSpeed  * Time.deltaTime, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// 마우스 포인트가 영역에 들어오면 호출되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 변수에 참이라는 값을 넣음 (카메라 이동 가능)
        _dragMouse = true;
    }

    /// <summary>
    /// 마우스 포인트가 영역에서 나가면 호출되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // 변수에 거짓이라는 값을 넣음 (카메라 이동 중단)
        _dragMouse = false;
    }
}
