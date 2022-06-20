
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// [ExecuteAlways]
public class StoreSystem : MonoBehaviour
{
    public Transform target;
    public GameObject child;
    public List<BuildingData> buildingData = new List<BuildingData>();
    public List<GameObject> buildingPages = new List<GameObject>();

    public int buildingDataNum;
    public int BuildingDataNum { set {buildingDataNum = value;} }

    public int pageNum;
    public int PageNum{set {pageNum = value;}}

    private void OnEnable()
    {
        for (int i = 0; i < buildingData.Count; i++)
        {
            // Store Page Creator
            // GameObject obj = Instantiate(child, target);
            // obj.transform.GetChild(0).GetComponent<Image>().sprite = buildingData[i].buildingImg;
            // obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = buildingData[i].buildingName;
            // obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = buildingData[i].cost1 + "               " +
            //                                                                 buildingData[i].cost2 + "                      " +
            //                                                                 buildingData[i].cost3 + "                    " +
            //                                                                 buildingData[i].cost4 + "                   " +
            //                                                                 buildingData[i].cost5;
        }
    }

    public void SortPage()
    {
        if(pageNum == 0)
        {
            for (int i = 0; i < buildingPages.Count; i++)
            {
                buildingPages[i].SetActive(true);
            }
        }

        if(pageNum == 1)
        {
            for (int i = 0; i < buildingPages.Count; i++)
            {
                if(buildingData[i].storePage == StorePage.Manufacture)
                    buildingPages[i].SetActive(true);
                else
                    buildingPages[i].SetActive(false);
            }
        }

        if(pageNum == 2)
        {
            for (int i = 0; i < buildingPages.Count; i++)
            {
                if(buildingData[i].storePage == StorePage.Enhance)
                    buildingPages[i].SetActive(true);
                else
                    buildingPages[i].SetActive(false);
            }
        }
    }

    public void buyBuilding()
    {
        ResourceSystem.Instance.PayResource(
            buildingData[buildingDataNum].cost1,
            buildingData[buildingDataNum].cost2,
            buildingData[buildingDataNum].cost3,
            buildingData[buildingDataNum].cost4,
            buildingData[buildingDataNum].cost5 );
    }
}
