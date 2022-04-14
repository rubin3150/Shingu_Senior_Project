using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Camera cam;
    public Building building;

    Vector2 m_Input;

    private void OnEnable()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        ObjectMove();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ResetMouse(building.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        building.transform.position = new Vector3(m_Input.x, 0.5f, m_Input.y);
        this.transform.parent.position = Input.mousePosition;
        //Mathf.Clamp(cam.ScreenPointToRay(Input.mousePosition).origin.x, -3, 3);
        //Mathf.Clamp(cam.ScreenPointToRay(Input.mousePosition).origin.y, -3, 3);
        //building.transform.position = new Vector3(cam.ScreenPointToRay(Input.mousePosition).origin.x, 0.5f, cam.ScreenPointToRay(Input.mousePosition).origin.z);    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapTool.Instance.EscapeEditMode();
    }

    public void ResetMouse(GameObject go)
    {
        m_Input.x = go.transform.position.x;
        m_Input.y = go.transform.position.z;
    }

    public void ObjectMove()
    {
        m_Input.x += Input.GetAxis("Mouse X");
        m_Input.y += Input.GetAxis("Mouse Y");
        //logText.text = "X : " + m_Input.x + "\nY : " + m_Input.y;
        //logText.text = "X : " + Input.mousePosition.x / 384 + "\nY : " + Input.mousePosition.y / 216;

        //Vector3 tempPosZ = new Vector3(0, 0, 0);
        //Vector3 mousePos = Input.mousePosition * 0.005f;
        //tempPosZ.z = mousePos.y;
        //mousePos.y = 0.5f;
        // movingObj.transform.position = (mousePos) + tempPosZ;

        //movingObj.transform.position = new Vector3(Input.mousePosition.x / 384, 0.5f, Input.mousePosition.y / 216);

        //logText.text = "X : " + cam.ScreenPointToRay(Input.mousePosition).origin.normalized +"\n"+ cam.ScreenPointToRay(Input.mousePosition).direction.normalized;
    }
}
