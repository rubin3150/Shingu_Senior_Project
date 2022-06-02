using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _UIController : MonoBehaviour
{
    public GameObject[] mainCampChild;
    public int max_MainCampChild;

    public int shopChild;

    public void MainCamp_False()
    {
        for(int i=0; i<max_MainCampChild; i++) {
            mainCampChild[i].SetActive(false);
        }
    }

    public void ToShop()
    {
        MainCamp_False();
        mainCampChild[shopChild].SetActive(true);
    }
}