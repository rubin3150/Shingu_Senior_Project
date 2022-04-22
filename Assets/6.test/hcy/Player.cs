using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 변수 영역

    // StageManager 스크립트를 담을 변수
    [SerializeField] private StageManager stageManager;
    
    // 레이캐스트 발사시 충돌할 레이어를 설정하는 변수
    [SerializeField] private LayerMask layerMask;
    
    // 플레이어의 Hp 이미지를 담을 변수
    [SerializeField] private Image hpImage;

    // 플레이어의 최대 Hp를 담을 변수
    [SerializeField] private float maxHpStat;

    // 플레이어의 현재 Hp를 담을 변수
    public float hpStat;
    // 플레이어의 스피드를 담을 변수
    public float speedStat;
    
    // 왼쪽으로 이동할 수 있는지 아닌지 체크할 변수
    private bool _donLeftMove;
    // 오른쪽으로 이동할 수 있는지 아닌지 체크할 변수
    private bool _donRightMove;

    #endregion
    
    /// <summary>
    /// 처음 한번만 실행하는 함수
    /// </summary>
    private void Start()
    {
        // 변수에 최대 Hp값을 넣어줌
        hpStat = maxHpStat;
    }

    /// <summary>
    /// 매 프레임마다 호출하는 함수
    /// </summary>
    void Update()
    {
        // 변수 값이 참이라면 아래 코드 실행 (전투 스테이지라면)
        if (stageManager.inStage == true)
        {
            // 왼쪽 화살표를 누르고 있으면서 변수 값이 거짓이라면 아래 코드 실행 (왼쪽으로 이동 할 수 있다면)
            if (Input.GetKey(KeyCode.LeftArrow) && _donLeftMove == false)
            {
                // 이 오브젝트의 위치를 좌측으로 스피드 * 실제시간 만큼 이동
                transform.position -= new Vector3(speedStat, 0, 0) * Time.deltaTime;
            }

            // 오른쪽 화살표를 누르고 있으면서 변수 값이 거짓이라면 아래 코드 실행 (오른쪽으로 이동 할 수 있다면)
            if (Input.GetKey(KeyCode.RightArrow) && _donRightMove == false)
            {
                // 이 오브젝트의 위치를 우측으로 스피드 * 실제시간 만큼 이동
                transform.position += new Vector3(speedStat, 0, 0) * Time.deltaTime;
            }
            
            // 함수 호출
            CheckObject();
        }
    }

    /// <summary>
    /// 레이캐스트를 쏴서 어떤 오브젝트가 있는지 체크하는 함수
    /// </summary>
    private void CheckObject()
    {
        // 변수에 좌측으로 레이캐스트를 발사해 걸린 물체를 반환함
        RaycastHit2D rayLeftHit = Physics2D.Raycast(transform.position, Vector2.left, 3.75f, layerMask);
        // 변수에 우측으로 레이캐스트를 발사해 걸린 물체를 반환함
        RaycastHit2D rayRightHit = Physics2D.Raycast(transform.position, Vector2.right, 3.75f, layerMask);

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
        hpStat -= damage;

        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        hpImage.fillAmount = hpStat / maxHpStat;

        if (hpStat <= 0)
        {
            Destroy(gameObject);
        }
    }
}
