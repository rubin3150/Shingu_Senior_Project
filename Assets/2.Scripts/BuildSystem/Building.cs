using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    public Buildings BuildingType = Buildings.None;
    public ResourceType BuildingResourceType = ResourceType.None;

    private int cost;
    private float buildTime;
    private float productionTime;
    private float maxResource;
    private string description;

    private void Start() 
    {

    }

    private void Update()
    {
        switch (BuildingType)
        {
            case Buildings.None:
                return;
            case Buildings.bee: 
                Debug.Log("이 빌딩은 " + Buildings.bee + "입니다.");
            break;
            case Buildings.windmil:
                Debug.Log("이 빌딩은 " + Buildings.windmil + "입니다.");
            break;
        }
    }
}
