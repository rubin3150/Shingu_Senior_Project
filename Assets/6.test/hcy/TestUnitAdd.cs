using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitAdd : MonoBehaviour
{
    public GameObject[] Units;

    public SelectUnitBase theBase;

    [SerializeField] private Skill[] skills;

    private int _r;

    private int k = 1;

    [SerializeField] private StageManager stageManager;

    // Update is called once per frame
    void Update()
    {
        if (stageManager.inStage == false)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                _r = Random.Range(0, Units.Length);

                if (Units[_r] != null)
                {
                    theBase.AddMaxUnit(Units[_r].GetComponent<UnitPickUp>().unit);
                    Units[_r] = null;
                }
                else if (Units[_r] == null)
                {
                
                    _r = Random.Range(0, Units.Length);
                    if (Units[_r] != null)
                    {
                        theBase.AddMaxUnit(Units[_r].GetComponent<UnitPickUp>().unit);
                    }

                    Units[_r] = null;
                }
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                if (skills.Length > k)
                {
                    skills[k].isActive = true;
                    skills[k].SetColor();
                    k += 1;
                }
                
            }
        }
    }
}
