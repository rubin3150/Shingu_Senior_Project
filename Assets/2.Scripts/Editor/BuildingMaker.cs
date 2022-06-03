using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildingMaker : EditorWindow
{
    public BuildingData buildingData;

    [MenuItem("Utility/BuildingMaker")]

    private static void ShowWindow()
    {
        var window = GetWindow<BuildingMaker>();
        window.titleContent = new GUIContent("BuildingMaker");
        window.Show();
    }

    public Buildings buildings = Buildings.None;
    public ResourceType BuildingResourceType = ResourceType.None;
    public int cost;
    public float buildTime;
    public float productionTime;
    public float maxResource;
    public Sprite buildingImg;

    private void OnGUI()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("******유닛 생성 툴******");
        EditorGUILayout.Space(15);

        GUILayout.Label("빌딩 이름과 리소스 생성 타입을 선택해주세요", EditorStyles.label);
        
        buildings = Buildings.None;

        BuildingResourceType = ResourceType.None;

        GUILayout.Space(20);

        buildingImg = (Sprite)EditorGUILayout.ObjectField("유닛 이미지", buildingImg, typeof(Sprite));

        GUILayout.Space(10);

        cost = EditorGUILayout.IntField("Cost", cost);

        buildTime = EditorGUILayout.FloatField("체력", buildTime);

        productionTime = EditorGUILayout.FloatField("공격력", productionTime);

        maxResource = EditorGUILayout.FloatField("이동속도", maxResource);

        //tag = EditorGUILayout.TextField("설명", tag);

#if UNITY_EDITOR

        if (GUILayout.Button("빌딩 데이터 생성!"))
        {
            if (buildingImg == null)
            {
                Debug.Log("빌딩 이미지를 먼저 채워주세요. 빌딩이미지가 비어있으면 생성되지 않습니다.");
                return;
            }

            buildingData = new BuildingData();
            buildingData.buildingImg = buildingImg;
            buildingData.cost = cost;
            buildingData.buildTime = buildTime;
            buildingData.productionTime = productionTime;
            buildingData.maxResource = maxResource;

            //if (!FileCheck(buildings))
            //{
            CreateScriptableObject<BuildingData>("test");
            //    CreatePrefabAsset(unitName, presetGameObject);
            //    unit.unitPrefab = presetGameObject;
            //}
        }
    }

    private void CreateScriptableObject<T>(string name) where T : ScriptableObject
    {
        var value = buildingData;//ScriptableObject.CreateInstance<T>();
        //AssetDatabase.CreateAsset(value, "Assets/10.Data/Units" + path + "/New" + typeof(T).ToString() + ".asset");
        AssetDatabase.CreateAsset(value, "Assets/10.Data/Buildings/" + name + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = value;
    }

    private bool FileCheck(string FileName)
    {
        string[] a = AssetDatabase.FindAssets(FileName, new[] { "Assets/10.Data/Units/" });

        if (a.Length > 0)
        {
            Debug.Log($"{FileName}과 같은 이름의 파일이 존재합니다. 파일명을 수정하세요");
            return true;
        }
        return false;
    }
}
#endif