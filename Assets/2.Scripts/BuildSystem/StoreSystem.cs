
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class StoreSystem : MonoBehaviour
{
    public Transform target;
    public GameObject child;
    public List<BuildingData> buildingData = new List<BuildingData>();

    private void OnEnable()
    {
        for (int i = 0; i < buildingData.Count; i++)
        {
            GameObject obj = Instantiate(child, target);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = buildingData[i].buildingImg;
            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = buildingData[i].buildingName;
            //obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = buildingData[i].cost1 + " " +
            //                                                                 buildingData[i].cost2 + " " +
            //                                                                 buildingData[i].cost3 + " " +
            //                                                                 buildingData[i].cost4 + " " +
            //                                                                 buildingData[i].cost5;
        }
    }
}
