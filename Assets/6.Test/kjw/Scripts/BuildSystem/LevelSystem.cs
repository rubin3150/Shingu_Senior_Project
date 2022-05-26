using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private int level;
    public List<GameObject> levelObject = new List<GameObject>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            LevelChange(1);
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            LevelChange(2);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            LevelChange(3);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            LevelChange(4);
        }
    }

    public void LevelChange(int _level)
    {
        level = _level;
        for (int i = 0; i < levelObject.Count; i++)
        {
            levelObject[i].SetActive(false);
        }
        levelObject[_level].SetActive(false);
    }
}
