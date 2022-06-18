using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    public GameObject[] pagePrefab;
    public int maxPage;

    public Animator animator;
    private int levelToLoad;

    public GameObject stage;
    
    private void Start()
    {
        DontDestroyOnLoad(stage);    
    }

    private void offAllPage()
    {
        for(int i=0; i<maxPage; i++)
            pagePrefab[i].gameObject.SetActive(false);
    }

    public void NextPage(int _int)
    {   
        offAllPage();
        pagePrefab[_int -1].gameObject.SetActive(true);
    }

    
    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");

        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
    }

    public void FadeToLevelSceneChange(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOutSceneChange");

        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
    }

    public void OnFadeComplete()
    {
        NextPage(levelToLoad);
        animator.SetTrigger("FadeIn");
    }

    public void OnFadeCompleteSceneChange()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}