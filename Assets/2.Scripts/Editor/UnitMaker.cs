using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UnitMaker : EditorWindow
{
    public Unit unit;
    public Object presetObject;
    public GameObject presetGameObject;

    [MenuItem("Utility/UnitMaker")]
    private static void ShowWindow() 
    {
        var window = GetWindow<UnitMaker>();
        window.titleContent = new GUIContent("UnitMaker");
        window.Show();
    }

    public string unitName;
    public Sprite unitImage;
    public Sprite unitFieldImage;
    public float moonEnergy;
    public float hpStat;
    public float attackStat;
    public float speedStat;
    public float spawnCoolTime;
    public float attackRangeStat;
    public float attackDelayStat;
    public float pushRange;
    public int criRate;
    public int criDamage;

    [System.Obsolete]
    private void OnGUI() 
    {
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("******유닛 생성 툴******");
        EditorGUILayout.Space(15);
        
        GUILayout.Label("유닛 이름을 적고 엔터키(ReturnKey)를 두번 빠르게 눌러주세요", EditorStyles.label);
        unitName = EditorGUILayout.TextField(unitName);
        GUI.enabled = (unitName == string.Empty || unitName == null) ? false : true;
        
        GUILayout.Space (15);

        presetObject = EditorGUILayout.ObjectField("오브젝트 프리셋", presetObject, typeof(GameObject), true);
        presetObject = PresetObjFind("PresetObject");
        presetGameObject = (GameObject) presetObject;

        GUILayout.Space (20);

        unitImage = (Sprite) EditorGUILayout.ObjectField("유닛 이미지", unitImage, typeof(Sprite));
        unitFieldImage = (Sprite) EditorGUILayout.ObjectField("유닛 필드 이미지", unitFieldImage, typeof(Sprite));

        GUILayout.Space (10);
 
        moonEnergy = EditorGUILayout.FloatField("달빛 에너지", moonEnergy);

        hpStat = EditorGUILayout.FloatField("체력", hpStat);

        attackStat = EditorGUILayout.FloatField("공격력", attackStat);

        speedStat = EditorGUILayout.FloatField("이동속도", speedStat);

        spawnCoolTime = EditorGUILayout.FloatField("소환 쿨타임", spawnCoolTime);

        attackRangeStat = EditorGUILayout.FloatField("공격 사거리", attackRangeStat);

        attackDelayStat = EditorGUILayout.FloatField("공격 딜레이", attackDelayStat);

        pushRange = EditorGUILayout.FloatField("넉백 거리", pushRange);

        criRate = EditorGUILayout.IntField("치명타 확률", criRate);

        criDamage = EditorGUILayout.IntField("치명타 데미지", criDamage);

        //tag = EditorGUILayout.TextField("설명", tag);

#if UNITY_EDITOR
        Event e = Event.current;
        if (e.isKey)
        {
            //Debug.Log("Detected key code: " + e.keyCode);
            if(e.keyCode == KeyCode.Return)
            {
                //Debug.Log("Return");
                unitImage = SpriteFinder(unitName);
            }
        }

        if(GUILayout.Button("유닛 생성!"))
        {
            if(unitImage == null || unitFieldImage == null)
            {
                Debug.Log("유닛 이미지를 먼저 채워주세요. 유닛이미지가 비어있으면 생성되지 않습니다.");
                return;
            }
            //if (prefabPreset == null)
            //{
            //    Debug.Log("프리셋 오브젝트를 먼저 채워주세요. 프리셋 오브젝트가 비어있으면 생성되지 않습니다.");
            //    return;
            //}
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
            unit.criRate = criRate;
            unit.criDamage = criDamage;

            presetGameObject.GetComponent<Image>().sprite = unitFieldImage;

            if(!FileCheck(unitName))
            {
                CreateScriptableObject<Unit>(unitName);
                CreatePrefabAsset(unitName, presetGameObject);
                unit.unitPrefab = presetGameObject;
            }
        }
    }

    private GameObject PresetObjFind(string objName)
    {
        GameObject obj = GameObject.Find(objName);
        //if(obj == null)
        //{
        //    Debug.Log("해당 Scene이 아닌 Defence Scene에서 작업해주세요");
        //    return null;
        //}
        return obj;
    }
    private Sprite SpriteFinder(string FileName)
    {
        string[] a = AssetDatabase.FindAssets(FileName, new[] { "Assets/Resources" });
        if (a.Length == 0)
        {
            Debug.Log($"{FileName}과 같은 이름의 유닛이 존재하지 않습니다. 제대로 된 유닛 이름을 입력해주세요");
            unitFieldImage = null;
            return null;
        }
        Sprite _sprite = Resources.Load<Sprite>(FileName);
        Sprite __sprite = Resources.Load<Sprite>(FileName + "_Unit");
        unitFieldImage = __sprite;
        return _sprite;
    }

    private bool FileCheck(string FileName) 
    {
        string[] a = AssetDatabase.FindAssets(FileName , new[] {"Assets/10.Data/Units/"});

        if (a.Length > 0)
        {
            Debug.Log($"{FileName}과 같은 이름의 파일이 존재합니다. 파일명을 수정하세요");
            return true;
        } 
        return false;
    }


    private void CreateScriptableObject<T>(string name) where T : ScriptableObject 
    {
        var value = unit;//ScriptableObject.CreateInstance<T>();
        //AssetDatabase.CreateAsset(value, "Assets/10.Data/Units" + path + "/New" + typeof(T).ToString() + ".asset");
        AssetDatabase.CreateAsset(value, "Assets/10.Data/Units/" + name + ".asset");
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = value;
    }

    private void CreatePrefabAsset(string name, GameObject go)
    { 
        Object _object = PrefabUtility.InstantiatePrefab(go);
        GameObject _gameObject = PrefabUtility.SaveAsPrefabAsset(go, "Assets/10.Data/UnitPrefabs/" + name + "Unit" + ".prefab");
        presetGameObject = _gameObject;
    }
#endif
}