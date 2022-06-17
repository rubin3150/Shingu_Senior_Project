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
    public bool isRight;
    public bool isTop;
    public bool isBottom;

    [SerializeField] private float MaxY;
    [SerializeField] private float MinY;
    [SerializeField] private float MaxX;
    [SerializeField] private float MinX;

    private void Update()
    {
        if (isMove)
        {
            CamMove();
            Zoom();
        }
    }

    public void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (cam.orthographicSize <= 4f && scroll > 0) // Maximum Zoom in
            cam.orthographicSize = 4f;
        else if (cam.orthographicSize >= 30f && scroll < 0) // Maximum Zoom out
            cam.orthographicSize = 30f;
        else // Zoom in
            cam.orthographicSize -= scroll;

    }

    public void CamMove()
    {
        if(Input.mousePosition.x < 100)
        {
            cam.transform.parent.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (1820 < Input.mousePosition.x)
        {
            cam.transform.parent.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        if (Input.mousePosition.y < 100)
        {
            cam.transform.parent.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (980 < Input.mousePosition.y)
        {
            cam.transform.parent.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        //if(Input.mousePosition.x < 100 && Input.mousePosition.y < 100)
        //{
        //    cam.transform.parent.Translate(new Vector3(-1, 0, -1).normalized * moveSpeed * Time.deltaTime);
        //}


        cam.transform.parent.position = new Vector3(Mathf.Clamp(cam.transform.parent.position.x, MinX, MaxX), cam.transform.parent.position.y, Mathf.Clamp(cam.transform.parent.position.z, MinY, MaxY));

        //if (Input.GetMouseButtonDown(0))
        //{
        //    m_Input.x = cam.transform.position.x;
        //    m_Input.y = cam.transform.position.z;
        //}

        //if (Input.GetMouseButton(0))
        //{
        //    m_Input.x += -Input.GetAxis("Mouse X") * moveSpeed;
        //    m_Input.y += -Input.GetAxis("Mouse Y") * moveSpeed;

        //    cam.transform.position = new Vector3(Mathf.Clamp(m_Input.x, MinX, MaxX), cam.transform.position.y, Mathf.Clamp(m_Input.y, MinY, MaxY));
        //}
    }
}
