using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "New Building/building")]
public class BuildingData : ScriptableObject
{
    public StorePage storePage = StorePage.None;
    public BuildingType buildingType = BuildingType.None;
    public ResourceType BuildingResourceType = ResourceType.None;
    public string buildingName;
    public int cost1; // childlikeEnergy cost
    public int cost2; // board cost
    public int cost3; // iron cost
    public int cost4; // honey cost
    public int cost5; // stew cost
    public float buildTime;
    public int maxResource;
    public Sprite buildingImg;
    public string description;
}