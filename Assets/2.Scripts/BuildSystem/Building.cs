using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    public Buildings buildingType = Buildings.None;
    public ResourceType resourceType = ResourceType.None;
    
    private int resource;

    private int cost;
    private float buildTime;
    private float createTime;
    private int maxResource;
    private string description;
    private bool isCreation = true;
    private bool isCollect;

    private Coroutine thisCoroutine;

    private void OnEnable() 
    {
        cost = buildingData.cost;
        buildTime = buildingData.buildTime;
        maxResource = buildingData.maxResource;
        description = buildingData.description;
        CreateReosource();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Stop();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CreateReosource();
        }
    }

    public void BuildingType()
    {
        switch (buildingType)
        {
            case Buildings.None:
                return;
            case Buildings.music_box:

            break;
            case Buildings.windmil:

            break;
            case Buildings.candlestick:

                break;
            case Buildings.Fountain:

                break;
            case Buildings.gazebo:

                break;
            case Buildings.Tower:

                break;
            case Buildings.wood_house:

                break;
            case Buildings.blacksmith_shop:

                break;
            case Buildings.bee:

                break;
            case Buildings.mushroom_house:

                break;
        }
    }
    
    public void CreateReosource()
    {
        switch (resourceType)
        {
            case ResourceType.None:
                return;
            case ResourceType.moonEnergy:
                thisCoroutine = StartCoroutine(Create(2, 1f));
                return;
            case ResourceType.log:
                resource = maxResource;
                return;
            case ResourceType.flower:
                resource = maxResource;
                return;
            case ResourceType.ore:
                resource = maxResource;
                return;
            case ResourceType.mushroom:
                resource = maxResource;
                return;
        }
    }

    IEnumerator Create(int _int, float _time)
    {
        while(isCreation)
        {
            resource += _int;
            yield return new WaitForSeconds(_time);
        }
    }

    public void Stop()
    {
        StopCoroutine(thisCoroutine);
    }
}