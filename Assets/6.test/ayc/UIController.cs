using UnityEngine;

public class UIController : Singleton<UIController>
{
    public GameObject[] pagePrefab;

    public int maxPage;

    [HideInInspector] public int pageNumber = 0;

    public void NextPage(int _int)
    {
        for(int i=0; i<maxPage; i++) {
            pagePrefab[i].gameObject.SetActive(false);
        }
        pageNumber = _int;
        pagePrefab[pageNumber].gameObject.SetActive(true);
    }

    public void ExitPage()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}