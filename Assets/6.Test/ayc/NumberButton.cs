using System.Collections;
using UnityEngine;

public class NumberButton : MonoBehaviour
{
    public GameObject[] numberChange;
    
    public void numberMethod(int btnNumber)
    {
        for(int i=0; i<6; i++) {
            numberChange[i].SetActive(false);
        }
        numberChange[btnNumber -1].SetActive(true);
    }
}
