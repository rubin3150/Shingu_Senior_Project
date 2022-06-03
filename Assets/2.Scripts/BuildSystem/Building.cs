using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Buildings
{
    music_box,
    windmil,
    candlestick,
    Fountain,
    gazebo,
    Tower,
    wood_house,
    blacksmith_shop,
    bee,
    mushroom_house,

    None = 99
}

public enum ResourceType
{
    moonEnergy,
    wood,
    stew,
    iron,
    honey,

    None = 99
}

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    public Buildings BuildingType = Buildings.None;
    public ResourceType BuildingResourceType = ResourceType.None;

    private int cost;
    private float buildTime;
    private float productionTime;
    private float maxResource;

    private void Start() 
    {

    }

    private void Update()
    {
        switch (BuildingType)
        {
            case Buildings.bee: 
                //Debug.Log("이 빌딩은 " + Buildings.bee + "입니다.");
            break;
            case Buildings.windmil:
                //Debug.Log("이 빌딩은 " + Buildings.windmil + "입니다.");
            break;
        }
    }
}
