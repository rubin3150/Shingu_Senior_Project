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

    public BuildingType buildingType = BuildingType.None;
    public ResourceType buildingResourceType = ResourceType.None;
    public int cost;
    public float buildTime;
    public int maxResource;
    public Sprite buildingImg;
    public string description;

    [System.Obsolete]
    private void OnGUI()
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("******빌딩 생성 툴******");
        EditorGUILayout.Space(15);

        GUILayout.Label("빌딩 이름과 리소스 생성 타입을 선택해주세요", EditorStyles.label);

        buildingType = (BuildingType)EditorGUILayout.EnumPopup("빌딩 종류", buildingType);
        buildingResourceType = (ResourceType)EditorGUILayout.EnumPopup("재료 생산 타입", buildingResourceType);

        GUILayout.Space(20);

        buildingImg = (Sprite)EditorGUILayout.ObjectField("유닛 이미지", buildingImg, typeof(Sprite));

        GUILayout.Space(10);

        cost = EditorGUILayout.IntField("비용", cost);

        buildTime = EditorGUILayout.FloatField("건물 건설 시간", buildTime);

        maxResource = EditorGUILayout.IntField("최대 자원 개수", maxResource);

        description = EditorGUILayout.TextField("설명", description);

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
            buildingData.maxResource = maxResource;
            buildingData.buildingType = buildingType;
            buildingData.BuildingResourceType = buildingResourceType;
            buildingData.description = description;

            if (!FileCheck(buildingType.ToString()))
            {
                CreateScriptableObject<BuildingData>(buildingType.ToString());
            }
        }
    }

    private void CreateScriptableObject<T>(string name) where T : ScriptableObject
    {
        var value = buildingData;
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