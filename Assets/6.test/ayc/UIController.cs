using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    public GameObject[] pagePrefab;

    public Animator animator;
    private int levelToLoad;

    public GameObject stage;
    
    private void Start()
    {
        // titleBtn[0].onClick.AddListener(() => PopUpController(true));
        // titleBtn[1].onClick.AddListener(TempSetting);
        // titleBtn[2].onClick.AddListener(() => PopUpController(false));
        DontDestroyOnLoad(stage);    
    }

    public void NextPage(int _int)
    {   
        for(int i=0; i<pagePrefab.Length; i++) {
            pagePrefab[i].gameObject.SetActive(false);
        }

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