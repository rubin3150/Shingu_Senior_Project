using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public bool isSeleted;
    public Canvas canvas;
    public List<GameObject> UiItems = new List<GameObject>();

    public void Start()
    {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            UiItems.Add(canvas.transform.GetChild(i).gameObject);
        }
    }

    public void EditMode(bool _bool)
    {
        UiItems[0].transform.position = Input.mousePosition; // 수정 필요: 움직임이 이상함
        UiItems[0].SetActive(_bool);
        isSeleted = _bool;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Rotate()
    {
        this.transform.eulerAngles += new Vector3(0, 45f, 0);
    }
}
