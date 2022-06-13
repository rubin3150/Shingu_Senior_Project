using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    public Buildings buildingType = Buildings.None;
    public ResourceType resourceType = ResourceType.None;

    private int cost;
    private int resource;
    private float buildTime;
    private float createTime;
    private float maxResource;
    private string description;

    private Coroutine thisCoroutine;

    private void Start() 
    {
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
            case ResourceType.wood:
                thisCoroutine = StartCoroutine(Create(1));
                break;
            case ResourceType.stew:
                thisCoroutine = StartCoroutine(Create(2));
                break;
            case ResourceType.ore:

                break;
            case ResourceType.honey:

                break;
        }
    }

    IEnumerator Create(int _int)
    {
        while(true)
        {
            resource += _int;
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void Stop()
    {
        StopCoroutine(thisCoroutine);
    }
}