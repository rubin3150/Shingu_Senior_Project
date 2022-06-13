using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ResourceDict
{
    public string resourceName;
    public int resourceValue;
    public Text resourceText;
}

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<ResourceDict> resourceElements = new List<ResourceDict>();
    private string[] resourceString = { "moonEnergy", "wood", "stew", "ore", "honey", "board", "mushroom", "iron", "flower" };
    public static int moonEnergy, wood, stew, ore, honey, board, mushroom, iron, flower;

    private void Start()
    {
        for (int i = 0; i < resourceString.Length; i++)
        {
            resourceElements[i].resourceName = resourceString[i];
            resourceElements[i].resourceValue = 0;
        }
    }

    public void GetResource(ResourceType _resourceType, int _int)
    {
        resourceElements[(int)_resourceType].resourceValue = _int;
    }
}