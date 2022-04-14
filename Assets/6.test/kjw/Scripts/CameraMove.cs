using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : Singleton<CameraMove>
{
    public Camera cam;
    public float zoomSpeed = -5f;
    public bool isMove = true;

    public bool IsMove { set { isMove = value; } }
    private Vector2 m_Input;

    private void Update()
    {
        if(isMove)
            CamMove();
        Zoom();
    }

    public void Zoom()
    {
        float scroll = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        //√÷¥Î ¡‹ ¿Œ
        if (cam.orthographicSize <= 4f && scroll > 0)
        {
            cam.orthographicSize = 4f;
        }
        // √÷¥Î ¡‹ æ∆øÙ
        else if (cam.orthographicSize >= 15.0f && scroll < 0)
        {
            cam.orthographicSize = 15f;
        }
        // ¡‹¿Œ æ∆øÙ «œ±‚.
        else
        {
            cam.orthographicSize -= scroll;
        }
    }

    public void CamMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Input.x = cam.transform.position.x;
            m_Input.y = cam.transform.position.z;
        }

        if (Input.GetMouseButton(0))
        {
            m_Input.x += -Input.GetAxis("Mouse X");
            m_Input.y += -Input.GetAxis("Mouse Y");

            cam.transform.position = new Vector3(m_Input.x, cam.transform.position.y, m_Input.y);
        }
    }
}
