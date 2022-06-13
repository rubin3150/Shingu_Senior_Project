using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ResourceDict
{
    public string resourceName;
    public int resourceValue;
    public TextMeshProUGUI resourceText;
}

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<ResourceDict> resourceElements = new List<ResourceDict>();
    public static int moonEnergy, log, flower, ore, mushroom, board, honey, iron, stew;

    private void Start()
    {
        for (int i = 0; i < resourceElements.Count; i++)
        {
            resourceElements[i].resourceName = ((ResourceType)i).ToString();
            resourceElements[i].resourceValue = 0;
        }
        GetResource(ResourceType.moonEnergy, 1000);
    }

    public void GetResource(ResourceType _resourceType, int _int)
    {
        resourceElements[(int)_resourceType].resourceValue = _int;
    }

    public void InsertResource()
    {

    }
}