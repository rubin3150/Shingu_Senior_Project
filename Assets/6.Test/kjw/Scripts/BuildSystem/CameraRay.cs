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

    public List<GameObject> buildings = new List<GameObject>();

    public bool isEditing;
    public Text statusText;

    private void Start()
    {
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
                if (i == w - 1 || j == h - 1)
                    isTrue[i, j] = true;
    }

    private void Update()
    {
        if(isEditing)
        {
            if(Input.GetMouseButtonDown(0))
                OnClick();
            
            OnDrag();
            statusText.text = "수정중입니다.";
            CameraMove.Instance.isMove = false;
        }
        else
        {
            statusText.text = "수정중이 아닙니다.";
            CameraMove.Instance.isMove = true;
        }
    }

    private void OnClick()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50f, IgnoreLayerMask))
            return;

        if (Physics.Raycast(ray, out hit, 50f, unitLayerMask))
        {
            index = int.Parse(hit.transform.parent.name);
            int _x = Mathf.RoundToInt(hit.transform.localScale.x);
            pickScaleX = _x;
            pickScaleZ = (int)hit.transform.localScale.z;

            IsBool(index, false, pickScaleX, pickScaleZ);
            pickObject = hit.transform;
        }
        else if (Physics.Raycast(ray, out hit, float.MaxValue, wallLayerMask)) 
            return;

    }

    public void BringObject()
    {
        dummyGameObject = Instantiate(prefab, this.transform);
        buildings.Add(dummyGameObject);
        pickObject = dummyGameObject.transform;
    }

    private void OnDrag()
    {
        if (pickObject == null) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50f, IgnoreLayerMask))
        {
            return;
        }

        if (Physics.Raycast(ray, out hit, float.MaxValue, wallLayerMask))
        {
            if (CheckBool(int.Parse(hit.transform.name), pickScaleX, pickScaleZ)) return;

            pickObject.SetParent(hit.transform);
            pickObject.localPosition = new Vector3(-(pickScaleX - 1) * 0.5f, 1, (pickScaleZ - 1) * 0.5f);
        }

        if(Input.GetMouseButtonDown(1))
        {
            if (pickObject != null)
            {
                IsBool(int.Parse(pickObject.transform.parent.name), true, pickScaleX, pickScaleZ);
                ResetBuildingsPosition();
                if(buildings.Contains(pickObject.gameObject))
                {
                    pickObject = null;
                    return;
                }
                else
                {
                    buildings.Add(pickObject.gameObject);
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

    public void LockBlock(int _level)
    {
        if(_level <= w)
        {
            for (int i = _level; i < w; i++)
            {
                LockUnLock(i, i, true);
            }
        }
    }

    public void UnLockBlock(int _level)
    {
        if(_level <= w)
        {
            for (int i = 8; i < _level; i++)
            {
                LockUnLock(i, i, false);
            }
        }
    }

    public void ResetBuildingsPosition()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            IsBool(int.Parse(buildings[i].transform.parent.name), true, pickScaleX, pickScaleZ);
        }
    }
}
