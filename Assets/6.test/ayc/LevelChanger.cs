using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            FadeToLevel(1);
        }
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {

    }
}
