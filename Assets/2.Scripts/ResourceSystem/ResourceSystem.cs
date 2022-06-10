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
    public List<ResourceDict> resourceText = new List<ResourceDict>();
    public static int moonEnergy, wood, stew, ore, honey, board, mushroom, iron, flower;

    private void Start()
    {
        //for (int i = 0; i < 9; i++)
        //{
        //    resourceText[i].resourceName = 
        //}
    }

    public void GetResource(ResourceType _resourceType, int _int)
    {

    }
    
    public void GetResource(ManufactureReosource _manufactureReosource, int _int)
    {

    }
}
