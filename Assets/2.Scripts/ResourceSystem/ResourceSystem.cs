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

        InsertResource();
    }

    public void PayResource(int _cost1, int _cost2, int _cost3, int _cost4, int _cost5)
    {
        // 코스트가 오버 될 경우 return;
        resourceElements[0].resourceValue -= _cost1;
        resourceElements[5].resourceValue -= _cost2;
        resourceElements[6].resourceValue -= _cost3;
        resourceElements[7].resourceValue -= _cost4;
        resourceElements[8].resourceValue -= _cost5;

        InsertResource();
    }

    public void InsertResource()
    {
        for (int i = 0; i < resourceElements.Count; i++) //InsertResource
        {
            if(resourceElements[i].resourceText == null)
                continue;
            resourceElements[i].resourceText.text = resourceElements[i].resourceValue.ToString();
        }
    }
}