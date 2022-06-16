using UnityEngine;

public class UIController : Singleton<UIController>
{
    public GameObject[] pagePrefab;
    public int maxPage;

    public Animator animator;
    private int levelToLoad;

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

    public void ExitPage()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        NextPage(levelToLoad);
        animator.SetTrigger("FadeIn");
    }
}