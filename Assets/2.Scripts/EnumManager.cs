using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    //Resource
    childlikeEnergy  = 0,
    log         = 1,
    flower    = 2,
    ore         = 3,
    mushroom      = 4,

    //ManufactureResource
    board       = 5,
    honey        = 6,
    iron        = 7,
    stew      = 8,

    None
}

public enum BuildingType
{
    music_box,
    windmil,
    candlestick,
    fountain,
    gazebo,
    Tower,
    wood_house,
    blacksmith_shop,
    bee,
    mushroom_house,

    None
}

public enum StorePage
{
    Manufacture,
    Enhance,

    None
}