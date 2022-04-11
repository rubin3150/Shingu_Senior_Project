using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Hp = 30;

    public SelectUnitBase _SelectUnitBase;
    private void Update()
    {
        if (_SelectUnitBase.inStage == true)
        {
            GetComponent<RectTransform>().localPosition -= new Vector3(1, 0, 0);
        }
    }

    public void A()
    {
        Hp -= 10;
        transform.GetComponent<RectTransform>().localPosition += new Vector3(200, 0,0 );

        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
