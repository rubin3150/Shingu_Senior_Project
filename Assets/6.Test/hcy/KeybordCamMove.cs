using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybordCamMove : MonoBehaviour
{
    // 키메라 오브젝트를 담을 변수
    [SerializeField] private Camera theCam;
    
    // 카메라의 이동 속도를 담을 변수 
    [SerializeField] private float keyBordCameraSpeed;

    [SerializeField] private StageManager _stageManager;

    [SerializeField] private CameraMovehcy[] _cameraMovehcy;
    // Update is called once per frame
    void Update()
    {
        if (_stageManager.inStage == true)
        {
            if (_cameraMovehcy[0]._dragMouse == false && _cameraMovehcy[1]._dragMouse == false)
            {
                if (Input.GetKey(KeyCode.A) && theCam.transform.position.x > -13.5f)
                {
                    // 카메라의 위치를 카메라 스피드 만큼 좌측으로 이동
                    theCam.transform.position -= new Vector3(keyBordCameraSpeed  * Time.deltaTime, 0, 0);
                }
                else if (Input.GetKey(KeyCode.D) && theCam.transform.position.x < 32.5f)
                {
                    // 카메라의 위치를 카메라 스피드 만큼 우측으로 이동
                    theCam.transform.position += new Vector3(keyBordCameraSpeed  * Time.deltaTime, 0, 0);
                }
            }
        }
    }
}
