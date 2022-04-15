using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float hpStat;

    public float maxHpStat;

    public float speedStat;

    public Image hpImage;

    [SerializeField] private StageManager stageManager;

    public LayerMask layerMask;

    private BoxCollider2D _boxCollider2D;

    public bool donLeftMove;

    public bool donRightMove;

    private Vector2 _dir;
    

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        hpStat = maxHpStat;
    }

    // Update is called once per frame
    void Update()
    {
     
        if (stageManager.inStage == true)
        {
            // Vector2 end = 
            if (Input.GetKey(KeyCode.LeftArrow) && donLeftMove == false)
            {
                transform.position -= new Vector3(speedStat, 0, 0) * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow) && donRightMove == false)
            {
                transform.position += new Vector3(speedStat, 0, 0) * Time.deltaTime;
            }
            
            CheckObject();
        }
    }

    private void CheckObject()
    {
        RaycastHit2D rayLeftHit = Physics2D.Raycast(transform.position, Vector2.left, 150f, layerMask);
        
        RaycastHit2D rayRightHit = Physics2D.Raycast(transform.position, Vector2.right, 150f, layerMask);
        
            if (rayLeftHit.collider != null)
            {
                donLeftMove = true;
           
            }
            else
            {
                donLeftMove = false;
            }
        
       
            if (rayRightHit.collider != null)
            {
                donRightMove = true;
            }
            else
            {
                donRightMove = false;
            }
        
    }
    
    public void UpdateHpBar()
    {
        // hpText.text = nowHpStat + " / " + unit.hpStat;
        // 체력 게이지 값 설정.
        hpImage.fillAmount = hpStat / maxHpStat;
        
        // 텍스트는 now값의 버림 소수점 제거한 값만 받음
    }
}
