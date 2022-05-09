using UnityEngine;

public class UIController : Singleton<UIController>
{
    public GameObject[] pagePrefab;

    public int maxPageNumber;

    [HideInInspector] public int pageNumber = 0;

    public void OnClickBtn()
    {
        if(pageNumber < maxPageNumber) {
            pagePrefab[pageNumber].gameObject.SetActive(false);
            pageNumber += 1;
            pagePrefab[pageNumber].gameObject.SetActive(true);
        }
        else {
            return;
        }
    }
}