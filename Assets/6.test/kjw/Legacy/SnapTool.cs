using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapTool : Singleton<SnapTool>
{
    public Camera cam;
    public GameObject edittingObj;

    private RaycastHit hit;
    public bool editMode;
    [SerializeField] private Material noneSelectedColor;
    [SerializeField] private Material selectedColor;
    
    [SerializeField] Text logText;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ObjectSelect();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeEditMode();
        }
    }
    public void ObjectSelect()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Building")
            {
                
                if (hit.collider.gameObject.GetComponent<Building>().isSeleted == true) // ���� ���� üũ, ������Ʈ�� �����ϰ� ������ ����, �ٸ� ������Ʈ Ŭ���� UI ����
                {
                    hit.collider.gameObject.GetComponent<Building>().EditMode(false);
                    ChangeState(hit.collider.gameObject, noneSelectedColor);
                    editMode = false;
                    edittingObj = null;
                }
                else
                {
                    hit.collider.gameObject.GetComponent<Building>().EditMode(true);
                    ChangeState(hit.collider.gameObject, selectedColor);
                    editMode = true;
                }

                edittingObj = hit.collider.gameObject; // �ݶ��̴����� ���� �� ���ӿ�����Ʈ�� edittingObj ��Ƶ�
                
                
                /*if(hit.collider.gameObject != edittingObj) // ���� �ʿ�: ������� Ż�� �Ұ�, ObjMove 37Line���� �ذ���
                {
                    EscapeEditMode();
                }*/
            }

        }
    }
    
    public void ChangeState(GameObject go, Material _mat)
    {
        go.GetComponent<MeshRenderer>().material = _mat;
    }

    public void EscapeEditMode()
    {
        if(editMode)
        {
            ChangeState(edittingObj, noneSelectedColor);
            edittingObj.GetComponent<Building>().EditMode(false);
        }
    }
}
