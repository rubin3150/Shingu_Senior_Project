using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    public TextMeshProUGUI[] playerName;

    public void Start()
    {
        for(int i=0; i<playerName.Length; i++)
        playerName[i].text = Data.Instance.nicknameBasket;
    }
}