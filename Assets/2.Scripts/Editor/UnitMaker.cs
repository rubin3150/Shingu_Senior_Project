using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UnitMaker : EditorWindow 
{
    [MenuItem("Utility/UnitMaker")]
    private static void ShowWindow() 
    {
        var window = GetWindow<UnitMaker>();
        window.titleContent = new GUIContent("UnitMaker");
        window.Show();
    }

    public string unitName;
    public Sprite unitImage;
    private void OnGUI() 
    { 
        
        // // 유닛의 이미지
        // public Sprite unitImage; 

        // // 유닛의 프리팹
        // public GameObject unitPrefab;

        // public float moonEnergy;

        // public float hpStat;

        // public float attackStat;

        // public float speedStat;

        // public float spawnCoolTime;

        // public float attackRangeStat;

        // public float attackDelayStat;

        // public float pushRange;


        GUILayout.Label("유닛 이름을 적어주세요", EditorStyles.miniLabel);
       
        unitName = EditorGUILayout.TextField(unitName);
        GUI.enabled = (unitName == string.Empty || unitName == null) ? false : true;

        //GUILayout.TextArea("Text Area", EditorStyles.textArea);

        GUILayout.Space (20);

        //unitImage = EditorGUILayout.

        if(GUILayout.Button("유닛 생성!"))
        {

        }
    }
}
