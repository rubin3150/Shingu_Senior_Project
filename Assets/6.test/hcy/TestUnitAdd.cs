using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUnitAdd : MonoBehaviour
{
    public GameObject[] Units;

    public SelectUnitBase theBase;

    [SerializeField] private SkillManager skillManager;

    private int _r;

    private int k = 1;

    [SerializeField] private StageManager stageManager;

    [SerializeField] private FadeIn _fadeIn;

    [SerializeField] private GameObject[] skillTxt;

    // Update is called once per frame
    void Update()
    {
        if (stageManager.inStage == false)
        {
            if (Data.Instance.isStage[0] == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Units[i] != null)
                    {
                        theBase.AddMaxUnit(Units[i].GetComponent<UnitPickUp>().unit);
                        Units[i] = null;
                    }
                }
            }
            else if (Data.Instance.isStage[1] == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Units[i] != null)
                    {
                        theBase.AddMaxUnit(Units[i].GetComponent<UnitPickUp>().unit);
                        Units[i] = null;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                for (int i = 0; i < Units.Length; i++)
                {
                    if (Units[i] != null)
                    {
                        theBase.AddMaxUnit(Units[i].GetComponent<UnitPickUp>().unit);
                        Units[i] = null;
                    }
                }
                // _r = Random.Range(0, Units.Length);
                //
                // if (Units[_r] != null)
                // {
                //     theBase.AddMaxUnit(Units[_r].GetComponent<UnitPickUp>().unit);
                //     Units[_r] = null;
                // }
                // else if (Units[_r] == null)
                // {
                //     _r = Random.Range(0, Units.Length);
                //     if (Units[_r] != null)
                //     {
                //         theBase.AddMaxUnit(Units[_r].GetComponent<UnitPickUp>().unit);
                //     }
                //
                //     Units[_r] = null;
                // }
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                if (skillManager.skillImage.Length > k)
                {
                    skillManager.isActive[k] = true;
                    _fadeIn.skillBorderImage[k].GetComponent<Image>().color =
                        new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

                    for (int i = 0; i < k * 2; i++)
                    {
                        skillTxt[i].transform.GetComponent<TextMeshProUGUI>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                    }
                    skillManager.SetColor();
                    k += 1;
                }
                
            }
        }
    }
}
