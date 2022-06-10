using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : Singleton<CameraMove>
{
    public Camera cam;
    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;
    public bool isMove = true;
    public bool isLeft;
    private Vector2 m_Input;

    [SerializeField] private float MaxY;
    [SerializeField] private float MinY;
    [SerializeField] private float MaxX;
    [SerializeField] private float MinX;

    private void Update()
    {
        if (isMove)
            CamMove();
        Zoom();
    }

    public void Zoom()
    {
        float scroll = -Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;

        if (cam.orthographicSize <= 4f && scroll > 0) // Maximum Zoom in
            cam.orthographicSize = 4f;
        else if (cam.orthographicSize >= 15.0f && scroll < 0) // Maximum Zoom out
            cam.orthographicSize = 15f;
        else // Zoom in
            cam.orthographicSize -= scroll;

    }

    public void CamMove()
    {
        //if (isLeft == true && theCam.transform.position.x > -13.5f)
        //{
        //    theCam.transform.position -= new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        //}
        //else if (isLeft == false && theCam.transform.position.x < 32.5f)
        //{
        //    theCam.transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        //}

        if (Input.GetMouseButtonDown(0))
        {
            m_Input.x = cam.transform.position.x;
            m_Input.y = cam.transform.position.z;
        }

        if (Input.GetMouseButton(0))
        {
            m_Input.x += -Input.GetAxis("Mouse X") * moveSpeed;
            m_Input.y += -Input.GetAxis("Mouse Y") * moveSpeed;

            cam.transform.position = new Vector3(Mathf.Clamp(m_Input.x, MinX, MaxX), cam.transform.position.y, Mathf.Clamp(m_Input.y, MinY, MaxY));
        }
    }
}
