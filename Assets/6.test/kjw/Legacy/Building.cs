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

public class Building : MonoBehaviour
{
    public Buildings thisBuilding = Buildings.None;

    private void Update()
    {
        switch (thisBuilding)
        {
            case Buildings.bee: 
                Debug.Log("이 빌딩은 " + Buildings.bee + "입니다.");
            break;
            case Buildings.windmil:
                Debug.Log("이 빌딩은 " + Buildings.windmil + "입니다.");
            break;
        }
        // if(thisBuilding == Buildings.bee)
        // {
        //     Debug.Log("이 빌딩은 " + Buildings.bee + "입니다.");
        // }
        // if(thisBuilding == Buildings.windmil)
        // {
        //     Debug.Log("이 빌딩은 " + Buildings.windmil + "입니다.");
        // }
    }
}
