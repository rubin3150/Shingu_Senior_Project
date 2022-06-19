using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    public TextMeshProUGUI[] nickname;

    void Start()
    {
        for(int i=0; i<nickname.Length; i++)
        {
            nickname[i].text = Data.Instance.nicknameBasket;
        }
    }
}
