using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "New Building/building")]
public class BuildingData : ScriptableObject
{
    public BuildingType buildingType = BuildingType.None;
    public ResourceType BuildingResourceType = ResourceType.None;
    public int cost;
    public float buildTime;
    public int maxResource;
    public Sprite buildingImg;
    public string description;
}