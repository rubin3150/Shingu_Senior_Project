using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UnitMaker : EditorWindow 
{
    public Unit unit;

    [MenuItem("Utility/UnitMaker")]
    private static void ShowWindow() 
    {
        var window = GetWindow<UnitMaker>();
        window.titleContent = new GUIContent("UnitMaker");
        window.Show();
    }
    public string unitName;
    public Sprite unitImage;
    public float moonEnergy;
    public float hpStat;
    public float attackStat;
    public float speedStat;
    public float spawnCoolTime;
    public float attackRangeStat;
    public float attackDelayStat;
    public float pushRange;
    private void OnGUI() 
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("******유닛 생성 툴******");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.Label("유닛 이름을 적어주세요", EditorStyles.miniLabel);
       
        unitName = EditorGUILayout.TextField(unitName);
        GUI.enabled = (unitName == string.Empty || unitName == null) ? false : true;

        //GUILayout.TextArea("Text Area", EditorStyles.textArea);

        GUILayout.Space (20);

        //unitName = EditorGUILayout.TextField("유닛 이름", unitName);

        unitImage = (Sprite) EditorGUILayout.ObjectField("유닛 이미지", unitImage, typeof(Sprite));
 
        moonEnergy = EditorGUILayout.FloatField("달빛 에너지", moonEnergy);

        hpStat = EditorGUILayout.FloatField("체력", hpStat);

        attackStat = EditorGUILayout.FloatField("공격력", attackStat);

        speedStat = EditorGUILayout.FloatField("이동속도", speedStat);

        spawnCoolTime = EditorGUILayout.FloatField("소환 쿨타임", spawnCoolTime);

        attackRangeStat = EditorGUILayout.FloatField("공격 사거리", attackRangeStat);

        attackDelayStat = EditorGUILayout.FloatField("공격 딜레이", attackDelayStat);

        pushRange = EditorGUILayout.FloatField("넉백 거리", pushRange);

        //tag = EditorGUILayout.TextField("설명", tag);

#if UNITY_EDITOR
        if(GUILayout.Button("유닛 생성!"))
        {
            unit = new Unit();
            unit.unitName = unitName;
            unit.unitImage = unitImage;
            unit.moonEnergy = moonEnergy;
            unit.hpStat = hpStat;
            unit.attackStat = attackStat;
            unit.speedStat = speedStat;
            unit.spawnCoolTime = spawnCoolTime;
            unit.attackRangeStat = attackRangeStat;
            unit.attackDelayStat = attackDelayStat;
            unit.pushRange = pushRange;

            if(!FileCheck(unitName)){ CreateScriptableObject<Unit>(unitName);}
        }
    }

    bool FileCheck(string FileName) 
    {
        string[] a = AssetDatabase.FindAssets(FileName , new[] {"Assets/10.Data/Units/"});

        if (a.Length > 0)
        {
            Debug.Log($"{FileName}과 같은 이름의 파일이 존재합니다. 파일명을 수정하세요");
            return true;
        } 
        return false;
    }
    
    // GameObject FindObject(string ObjectName)
    // {
    //     string[] a = AssetDatabase.FindAssets(ObjectName , new[] {"Assets/10.Data/"});
    //     GameObject obj = ObjectName.

    //     if (a.Length == 0)
    //     {
    //         Debug.Log($"{ObjectName}를 찾을 수 없습니다.");
    //         return null;
    //     } 
    //     return ;
    // }
    
    private void CreateScriptableObject<T>(string name) where T : ScriptableObject 
    {
        var value = unit;//ScriptableObject.CreateInstance<T>();
        //AssetDatabase.CreateAsset(value, "Assets/10.Data/Units" + path + "/New" + typeof(T).ToString() + ".asset");
        AssetDatabase.CreateAsset(value, "Assets/10.Data/Units/" + name + ".asset");
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = value;
    }

    // private void CreatePrefabAsset(string name)
    // {
    //     var value = 
    //     AssetDatabase.CreateAsset(value, "Assets/10.Data/UnitPrefabs/" + name + ".asset");
    //     AssetDatabase.SaveAssets();
    // }
#endif
}
