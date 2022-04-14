using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawn : MonoBehaviour
{
    public GameObject prefab;
    public void SpawnObj()
    {
        SnapTool.Instance.EscapeEditMode();
        Instantiate(prefab);
    }
}
