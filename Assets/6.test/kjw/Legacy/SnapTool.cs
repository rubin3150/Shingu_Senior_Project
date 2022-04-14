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
                
                if (hit.collider.gameObject.GetComponent<Building>().isSeleted == true) // 현재 상태 체크, 오브젝트가 존재하고 있으면 없앰, 다른 오브젝트 클릭시 UI 끄기
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

                edittingObj = hit.collider.gameObject; // 콜라이더에서 검출 된 게임오브젝트를 edittingObj 담아둠
                
                
                /*if(hit.collider.gameObject != edittingObj) // 수정 필요: 에딧모드 탈출 불가, ObjMove 37Line으로 해결중
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
