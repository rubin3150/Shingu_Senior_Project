using UnityEngine;

public class UIController : Singleton<UIController>
{
    public GameObject[] pagePrefab;

    [HideInInspector] public int pageNumber = 0;

    public void NextPage(int _int)
    {
        pagePrefab[pageNumber].gameObject.SetActive(false);
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