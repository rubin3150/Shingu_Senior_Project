using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public Canvas canvas;

    public GameObject[] pagePrefab;
    public GameObject initialPage;
    public GameObject currentPage;
    
    private void Start()
    {
        currentPage = Instantiate(initialPage, canvas.transform);
    }
}