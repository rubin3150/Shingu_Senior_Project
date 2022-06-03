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
        EditorGUILayout.LabelField("******���� ���� ��******");
        EditorGUILayout.Space(15);

        GUILayout.Label("���� �̸��� ���ҽ� ���� Ÿ���� �������ּ���", EditorStyles.label);
        
        buildings = Buildings.None;

        BuildingResourceType = ResourceType.None;

        GUILayout.Space(20);

        buildingImg = (Sprite)EditorGUILayout.ObjectField("���� �̹���", buildingImg, typeof(Sprite));

        GUILayout.Space(10);

        cost = EditorGUILayout.IntField("Cost", cost);

        buildTime = EditorGUILayout.FloatField("ü��", buildTime);

        productionTime = EditorGUILayout.FloatField("���ݷ�", productionTime);

        maxResource = EditorGUILayout.FloatField("�̵��ӵ�", maxResource);

        //tag = EditorGUILayout.TextField("����", tag);

#if UNITY_EDITOR

        if (GUILayout.Button("���� ������ ����!"))
        {
            if (buildingImg == null)
            {
                Debug.Log("���� �̹����� ���� ä���ּ���. �����̹����� ��������� �������� �ʽ��ϴ�.");
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
            Debug.Log($"{FileName}�� ���� �̸��� ������ �����մϴ�. ���ϸ��� �����ϼ���");
            return true;
        }
        return false;
    }
}
#endif