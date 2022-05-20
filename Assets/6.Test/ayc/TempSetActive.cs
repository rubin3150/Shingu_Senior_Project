using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempSetActive : MonoBehaviour
{
    public GameObject bag;
    
    public void ActiveTrue()
    {
        bag.SetActive(true);
    }
    
    public void ActiveFalse()
    {
        bag.SetActive(false);
    }
}
