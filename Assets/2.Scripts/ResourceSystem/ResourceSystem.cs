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
    // public List<GameObject> logSpawner = new List<GameObject>();
    // public List<GameObject> flowerSpawner = new List<GameObject>();
    // public List<GameObject> oreSpawner = new List<GameObject>();
    // public List<GameObject> mushroomSpawner = new List<GameObject>();
    public List<ResourceDict> resourceElements = new List<ResourceDict>();

    private void Start()
    {
        for (int i = 0; i < resourceElements.Count; i++)
        {
            resourceElements[i].resourceName = ((ResourceType)i).ToString();
            resourceElements[i].resourceValue = 0;
        }
        GetResource(ResourceType.childlikeEnergy, 1000);
    }

    public void GetResource(ResourceType _resourceType, int _int)
    {
        resourceElements[(int)_resourceType].resourceValue += _int;
        
        for (int i = 0; i < resourceElements.Count; i++) //InsertResource
        {
            if(resourceElements[i].resourceText == null)
                continue;
            resourceElements[i].resourceText.text = resourceElements[i].resourceValue.ToString();
        }
    }
}