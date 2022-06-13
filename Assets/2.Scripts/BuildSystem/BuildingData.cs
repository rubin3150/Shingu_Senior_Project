using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "New Building/building")]
public class BuildingData : ScriptableObject
{
    public Buildings buildings = Buildings.None;
    public ResourceType BuildingResourceType = ResourceType.None;
    public int cost;
    public float buildTime;
    public float createTime;
    public float maxResource;
    public Sprite buildingImg;
    public string description;
}