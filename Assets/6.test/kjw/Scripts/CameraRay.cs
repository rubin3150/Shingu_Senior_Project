using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    private GameObject prefab;
    public const int w = 21;
    public const int h = 21;
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

    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {
        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
                if (i == w - 1 || j == h - 1)
                    isTrue[i, j] = true;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
            OnClick();

        if (Input.GetMouseButton(0))
        {
            OnDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (pickObject != null)
            {
                IsBool(int.Parse(pickObject.transform.parent.name), true, pickScaleX, pickScaleZ);
                pickObject = null;
            }
        }
    }

    private void OnClick()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //fix require
        if (Physics.Raycast(ray, out hit, 50f, unitLayerMask))
        {
            index = int.Parse(hit.transform.parent.name);
            int _x = Mathf.RoundToInt(hit.transform.localScale.x);
            pickScaleX = _x;
            pickScaleZ = (int)hit.transform.localScale.z;

            IsBool(index, false, pickScaleX, pickScaleZ);
            pickObject = hit.transform;

            CameraMove.Instance.isMove = false;
        }
        else if (Physics.Raycast(ray, out hit, float.MaxValue, wallLayerMask))
        {
            CameraMove.Instance.isMove = true;
            // 0x0 return;
            if (prefab == null) return;

            index = int.Parse(hit.transform.name);

            if (CheckBool(index, pickScaleX, pickScaleZ))
            {
                return;
            }

            dummyGameObject = Instantiate(prefab, hit.transform);
            dummyGameObject.transform.localPosition = new Vector3(-(pickScaleX - 1) * 0.5f, 1, (pickScaleZ - 1) * 0.5f);

            IsBool(index, true, pickScaleX, pickScaleZ);

            // 1 pick
            prefab = null;
        }
    }

    private void OnDrag()
    {
        if (pickObject == null) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue, wallLayerMask))
        {
            if (CheckBool(int.Parse(hit.transform.name), pickScaleX, pickScaleZ)) return;

            pickObject.SetParent(hit.transform);
            pickObject.localPosition = new Vector3(-(pickScaleX - 1) * 0.5f, 1, (pickScaleZ - 1) * 0.5f);
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
}
