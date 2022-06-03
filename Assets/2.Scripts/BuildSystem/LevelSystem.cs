using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : Singleton<LevelSystem>
{
    public int level;
    public List<GameObject> levelObject = new List<GameObject>();
    private int lockNum = 9;
    //private int unlockNum = 10;

    private void Start() 
    {
        CameraRay.Instance.LockBlock(lockNum);
    }

    void Update()
    {
        if(!CameraRay.Instance.isEditing)
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                LevelChange(0);
            }

            if(Input.GetKeyDown(KeyCode.I))
            {
                LevelChange(1);
            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                LevelChange(2);
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                LevelChange(3);
            }
        }
    }

    public void LevelChange(int _level)
    {
        levelObject[_level].SetActive(false);
        level++;
        //CameraRay.Instance.UnLockBlock(unlockNum);
        //unlockNum++;
        CameraRay.Instance.ResetBuildingsPosition();
    }
}
