using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject[] page;
    
    public Image blur;
    public GameObject set;
    
    public Button[] btn;
    /*
    0. Set - esc
    1. Set - exit
    2. Create Pop-Up - Y Btn;
    3. 타이틀 SETTING
    4. 월드맵 set
    5~8. 입장버튼
    */
    
    public Animator animator;
    private int levelToLoad;
    
    void Start()
    {
        btn[0].onClick.AddListener(() => SetController(false));
        btn[1].onClick.AddListener(Exit);
        btn[2].onClick.AddListener(Data.Instance.SaveNicknameData);
        btn[2].onClick.AddListener(() => FadeToLevel(1));
        btn[3].onClick.AddListener(() => SetController(true));
        btn[4].onClick.AddListener(() => SetController(true));
        btn[5].onClick.AddListener(() => FadeToLevelSceneChange(1));
        btn[6].onClick.AddListener(() => FadeToLevelSceneChange(1));
        btn[7].onClick.AddListener(() => FadeToLevelSceneChange(1));
        btn[8].onClick.AddListener(() => FadeToLevelSceneChange(1));
    }

    // Fade
    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");

        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
    }

    public void OnFadeComplete(int _int)
    {
        _int = levelToLoad;
        
        for(int i=0; i<page.Length; i++)
        {
            page[i].gameObject.SetActive(false);
        }
        page[_int].gameObject.SetActive(true);
        
        animator.SetTrigger("FadeIn");
    }

    // _______________________________________________________

    public void FadeToLevelSceneChange(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOutSceneChange");

        Data.Instance.sfx.clip = Data.Instance.scenechangeClip;
        Data.Instance.sfx.Play();
    }

    public void OnFadeCompleteSceneChange()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // ________________________________________________________

    public void SetController(bool _bool)
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
        
        set.SetActive(_bool);
        blur.enabled = _bool;

        CameraMove.Instance.isMove = !(_bool);
    }

    public void Exit()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}