using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    #region 변수 영역

    // 카메라가 이동할 수 있는 영역을 담을 변수 
    [SerializeField] private BoxCollider2D bound;

    // 카메라 오브젝트르르 담을 변수 
    private Camera _theCamera;
    
    // 이동 할 수 있는 영역의 최소 X, Y값을 담을 변수
    private Vector3 _minBound;
    // 이동 할 수 있는 영역의 최대 X, Y값을 담을 변수
    private Vector3 _maxBound;

    // 카메라의 오소그래픽 사이즈를 담을 변수
    private float _halfWidth;
    // 이동할 수 있는 X값을 담을 변수
    private float _halfHeight;

    #endregion

    /// <summary>
    /// 처음 한번만 호출되는 함수
    /// </summary>
    void Start()
    {
        // 변수에 카메라 오브젝트를 담읆
        _theCamera = GetComponent<Camera>();
        
        // 변수에 이동할 수 있는 영역의 최소 값을 넣어줌
        _minBound = bound.bounds.min;
        // 변수에 이동할 수 있는 영역의 최대 값을 넣어줌
        _maxBound = bound.bounds.max;
         
        // 변수에 카메라 오브젝트의 오소그래픽 사이즈를 담음
        _halfHeight = _theCamera.orthographicSize;
        
        // 오소그래픽 사이즈 * 실제 화면의 가로 / 실제 화면의 세로를 계산한 후에 변수에 넣음
        _halfWidth = _halfHeight * Screen.width / Screen.height;
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수
    /// </summary>
    void Update()
    {
        // 이 오브젝트의 X값이 최소 / 최대 사이의 float 값이 value 범위 외의 값이 되지 않도록 합니다
        float clampedX = Mathf.Clamp(transform.position.x, _minBound.x + _halfWidth, _maxBound.x - _halfWidth);

        // 카메라의 위치 재설정
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
