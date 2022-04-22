using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public Canvas canvas;
    public GameObject[] pagePrefab;
    public GameObject initialPage;
    public GameObject currentPage;
    int initialPageNumber = 0;
    int pageIndex = 0;
    public int maxPageIndex;

    // void OnEnable()
    // {
    //     Instantiate(pagePrefab[pageIndex]);
    // }

    private void Start()
    {
        currentPage = Instantiate(initialPage, canvas.transform);
    }
    public void BtnClick()
    {

        if(pageIndex <= maxPageIndex) {
            Destroy(currentPage);
            pageIndex += 1;
        }
        else {
            return;
        }
    }

}
