using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Data : Singleton<Data>
{
    public string nicknameBasket; //바구니
    public Text nicknamefield; //유저입력
    public TextMeshProUGUI nicknameshow; //보여줄거

    public AudioSource audio;
    public AudioSource sfx;

    // audio
    public AudioClip titleClip;
    public AudioClip worldmapClip;
    public AudioClip selectunitClip;
    public AudioClip defenseClip;
    public AudioClip storeClip;
    public AudioClip gameclearClip;
    public AudioClip gameoverClip;

    // sfx
    public AudioClip btnClip;
    public AudioClip scenechangeClip;

    public void SaveNicknameData()
    {
        nicknameBasket = nicknamefield.text;

        nicknameshow.text = nicknameBasket;
    }
}