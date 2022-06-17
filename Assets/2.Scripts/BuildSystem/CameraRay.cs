using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRay : Singleton<CameraRay>
{
    private GameObject prefab;
    public const int w = 13;
    public const int h = 13;
    public bool[,] isTrue = new bool[w, h];

    public int pickScaleX;
    public int pickScaleZ;

    public GameObject Prefab { set { prefab = value; } }
    public int PickScaleX { set { pickScaleX = value; } }
    public int PickScaleZ { set { pickScaleZ = value; } }

    private int index;
    private Transform pickObject;
    private GameObject dummyGameObject;
    private int wallLayerMask = 1 << 6;
    private int unitLayerMask = 1 << 7;
    private int IgnoreLayerMask = 1 << 11;

    private Ray ray;
    private RaycastHit hit;

    public bool isEditing;
    public bool IsEditing { set { isEditing = value; } }
    public bool isMoving;
    public Text statusText;
    public GameObject constructionModel;
    public GameObject constructionEffect;
    public GameObject option;
    public List<GameObject> buildings = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
                if (i == w - 1 || j == h - 1)
                    isTrue[i, j] = true;
    }

    [System.Obsolete]
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            OnClick();

        if(isEditing)
        {
            OnDrag();
            CameraMove.Instance.isMove = false;
            statusText.text = "수정중입니다.";
        }
        else
        {
            CameraMove.Instance.isMove = true;
            statusText.text = "수정중이 아닙니다.";
        }
    }

    private void OnClick()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.CompareTag("Sell"))
            {
                SellBuilding(hit.transform.parent.transform.parent.gameObject);
            }
            if(hit.transform.CompareTag("Move"))
            {
                pickObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                Destroy(hit.transform.parent.gameObject);
                isMoving = true;
            }
            if(!isEditing)
            {
                if (hit.transform.CompareTag("Resource"))
                {
                    Building go = hit.transform.gameObject.GetComponent<Building>();
                    go.StartCoroutine(go.GetResource(300f));
                    //go.GetResource(go.resourceType, hit.transform.gameObject, 300f);
                }

                if (hit.transform.CompareTag("Unit"))
                {
                    Building go = hit.transform.gameObject.GetComponent<Building>();
                    go.GetBuildingResource(go);
                    go.resource = 0;
                }
            }
        }

        if (Physics.Raycast(ray, out hit, float.MaxValue, IgnoreLayerMask))
            return;

        if (Physics.Raycast(ray, out hit, float.MaxValue, unitLayerMask))
        {
            if(isEditing)
            {
                if(pickObject != null) return;
                //if(pickObject.GetComponent<Building>().isConstruct) return;

                index = int.Parse(hit.transform.parent.name);
                int _x = Mathf.RoundToInt(hit.transform.localScale.x);
                int _z = Mathf.RoundToInt(hit.transform.localScale.z);
                pickScaleX = _x;
                pickScaleZ = _z;//(int)hit.transform.localScale.z;

                IsBool(index, false, pickScaleX, pickScaleZ);
                pickObject = hit.transform;

                Vector3 _position = hit.transform.position - new Vector3(0, -2.5f, 1);
                option.transform.localScale = new Vector3(1,1,1);
                Vector3 _option = new Vector3(option.transform.localScale.x / pickScaleX, 1, option.transform.localScale.z / pickScaleX);
                option.transform.localScale = _option;
                Instantiate(option, _position, option.transform.localRotation, hit.transform);
            }
        }
        else if (Physics.Raycast(ray, out hit, float.MaxValue, wallLayerMask)) 
            return;
    }

    public void BringObject()
    {
        if(isEditing)
        {
            if(!buildings.Contains(dummyGameObject)) Destroy(dummyGameObject);
            ResetBuildingsPosition();
            dummyGameObject = Instantiate(prefab, this.transform);
            dummyGameObject.transform.position += new Vector3(0, 100, 0);
            dummyGameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
            pickObject = dummyGameObject.transform;
            isMoving = true;
        }
    }

    [System.Obsolete]
    private void OnDrag()
    {
        if (pickObject == null) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue, IgnoreLayerMask))
        {
            return;
        }

        if (Physics.Raycast(ray, out hit, float.MaxValue, wallLayerMask))
        {
            if (CheckBool(int.Parse(hit.transform.name), pickScaleX, pickScaleZ)) return;
            if (!isMoving) return;

            pickObject.SetParent(hit.transform);
            pickObject.localPosition = new Vector3(-(pickScaleX - 1) * 0.5f, 1, (pickScaleZ - 1) * 0.5f);
        }

        if(Input.GetMouseButtonDown(1))
        {
            if (pickObject != null)
            {
                isMoving = false;
                IsBool(int.Parse(pickObject.transform.parent.name), true, pickScaleX, pickScaleZ);
                pickObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                ResetBuildingsPosition();
                if(buildings.Contains(pickObject.gameObject))
                {
                    pickObject = null;
                    return;
                }
                else
                {
                    pickObject.GetComponent<Building>().buildBuilding(constructionModel, constructionEffect);
                    buildings.Add(pickObject.gameObject);
                    ResetBuildingsPosition();
                    pickObject = null;
                }
            }
        }
    }

    private bool CheckBool(int index, int sizeX, int sizeZ)
    {
        for (int i = 1; i <= sizeX; i++)
        {
            for (int j = 1; j <= sizeZ; j++)
            {
                if (isTrue[index / h + i - 1, index % h + j - 1])
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void IsBool(int index, bool isBool, int sizeX, int sizeZ)
    {
        for (int i = 1; i <= sizeX; i++)
        {
            for (int j = 1; j <= sizeZ; j++)
            {
                isTrue[index / h + i - 1, index % h + j - 1] = isBool;
            }
        }
    }

    public void LockUnLock(int w, int h, bool _bool)
    {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (i == w - 1 || j == h - 1)
                    isTrue[i, j] = _bool;
            }
        }
    }

    public void LockBlock(int _int)
    {
        if(_int <= w)
        {
            for (int i = _int; i < w; i++)
            {
                LockUnLock(i, i, true);
            }
        }
    }

    public void SellBuilding(GameObject go)
    {
        buildings.Remove(go);
        pickObject = null;
        Destroy(go);
    }

    public void ResetBuildingsPosition()
    {
        if(isEditing)
        {
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                        isTrue[i, j] = false;
            
            if(LevelSystem.Instance.level < 5)
            {
                LockBlock(LevelSystem.Instance.level + 9);
            }

            Start();

            for (int i = 0; i < buildings.Count; i++)
            {
                var LocalScale = buildings[i].gameObject.transform.localScale;
                IsBool(int.Parse(buildings[i].transform.parent.name), true, (int)LocalScale.x, (int)LocalScale.z);
                //Debug.Log(buildings[i].transform.parent.name+"의 x 크기를"+int.Parse(x)+"으로 바꿨습니다.");
            }
        }
    }
}