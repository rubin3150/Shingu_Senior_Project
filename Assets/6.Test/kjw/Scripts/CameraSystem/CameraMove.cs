using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : Singleton<CameraMove>
{
    public Camera cam;
    public float zoomSpeed = -5f;
    public bool isMove = true;

    public bool IsMove;
    private Vector2 m_Input;

    private void Update()
    {
        if (isMove)
            CamMove();
        Zoom();
    }

    public void Zoom()
    {
        float scroll = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (cam.orthographicSize <= 4f && scroll > 0) // Maximum Zoom in
            cam.orthographicSize = 4f;
        else if (cam.orthographicSize >= 15.0f && scroll < 0) // Maximum Zoom out
            cam.orthographicSize = 15f;
        else // Zoom in
            cam.orthographicSize -= scroll;

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

            cam.transform.position = new Vector3(Mathf.Clamp(m_Input.x, 0, 25), cam.transform.position.y, Mathf.Clamp(m_Input.y, -25, -10));
        }
    }
}
