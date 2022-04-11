using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/unit")]
public class Unit : ScriptableObject
{
    public string unitName; // 유닛의 이름
    public Sprite unitImage; // 유닛의 이미지

    public GameObject unitPrefab; // 유닛의 프리팹
    
    
}
