using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStage : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;
    
    public void SetStageScript()
    {
        // 변수에 참이라는 값을 넣음 (전투 스테이지 진입)
        stageManager.inStage = true;
    }
}
