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
    public StorePage storePage = StorePage.None;
    public BuildingType buildingType = BuildingType.None;
    public ResourceType buildingResourceType = ResourceType.None;
    public string buildingName;
    public int cost1;
    public int cost2;
    public int cost3;
    public int cost4;
    public int cost5;
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
        EditorGUILayout.Space(15);

        storePage = (StorePage)EditorGUILayout.EnumPopup("빌딩 종류", storePage);
        buildingType = (BuildingType)EditorGUILayout.EnumPopup("빌딩 이름", buildingType);
        buildingResourceType = (ResourceType)EditorGUILayout.EnumPopup("재료 생산 타입", buildingResourceType);

        GUILayout.Space(20);
        GUILayout.Label("빌딩의 한글 이름을 적어주세요", EditorStyles.label);
        buildingName = EditorGUILayout.TextField(buildingName);
        GUILayout.Space(20);

        buildingImg = (Sprite)EditorGUILayout.ObjectField("건물 이미지", buildingImg, typeof(Sprite));

        GUILayout.Space(10);

        cost1 = EditorGUILayout.IntField("동심에너지 비용", cost1);
        cost2 = EditorGUILayout.IntField("판자 비용", cost2);
        cost3 = EditorGUILayout.IntField("철 비용", cost3);
        cost4 = EditorGUILayout.IntField("꿀 비용", cost4);
        cost5 = EditorGUILayout.IntField("스튜 비용", cost5);

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
            buildingData.buildingName = buildingName;
            buildingData.buildingImg = buildingImg;
            buildingData.cost1 = cost1;
            buildingData.cost2 = cost2;
            buildingData.cost3 = cost3;
            buildingData.cost4 = cost4;
            buildingData.cost5 = cost5;
            buildingData.buildTime = buildTime;
            buildingData.maxResource = maxResource;
            buildingData.storePage = storePage;
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