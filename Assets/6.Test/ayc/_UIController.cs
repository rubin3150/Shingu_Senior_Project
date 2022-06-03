using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _UIController : MonoBehaviour
{
    public GameObject popUP;
    
    public GameObject[] mainCampChild;
    public int max_MainCampChild;

    // temp
    public GameObject[] toShop;
    public int max_ToShop;
    // ______

    public void MainCamp_False()
    {
        for(int i=0; i<max_MainCampChild; i++) {
            mainCampChild[i].SetActive(false);
        }
    }

    public void ToShop()
    {
        MainCamp_False();
        for(int i=0; i<max_ToShop; i++) {
            toShop[i].SetActive(true);
        }
    }

    public void PopUp()
    {
        popUP.SetActive(true);
    }
}