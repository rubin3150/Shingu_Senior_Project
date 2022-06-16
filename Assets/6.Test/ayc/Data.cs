using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Data : Singleton<Data>
{
    public string nicknameBasket; //바구니
    public TextMeshProUGUI nicknamefield; //유저입력
    public TextMeshProUGUI nicknameshow; //보여줄거

    public void SaveNicknameData()
    {
        nicknameBasket = nicknamefield.text;

        nicknameshow.text = nicknameBasket;
    }
}
