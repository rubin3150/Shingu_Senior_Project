using UnityEngine;
// Temp
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    public GameObject[] pagePrefab;

    public int maxPage;

    [HideInInspector] public int pageNumber = 0;

    private int backPageNumber;

    public void NextPage(int _int)
    {   
        offAllPage();
        backPageNumber = pageNumber;
        pageNumber = _int - 1;
        pagePrefab[pageNumber].gameObject.SetActive(true);

    }

    public void BackPage()
    {   
        offAllPage();
        pageNumber = backPageNumber;
        pagePrefab[backPageNumber].gameObject.SetActive(true);
    }

    public void ExitPage()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void offAllPage()
    {
        for(int i=0; i<maxPage; i++) {
            pagePrefab[i].gameObject.SetActive(false);
        }
    }

    // Temp
    public void DefenceSceneChange()
    {
        SceneManager.LoadScene(1);
    }
}