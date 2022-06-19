using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public GameObject createPopUp;

    public Button[] titleBtn;
    /*
    0. 게임 시작 버튼 - Pop-up UI On
    1. Pop-up 취소 버튼
    */

    void Start()
    {
        Data.Instance.mainAudio.clip = Data.Instance.titleClip;
        Data.Instance.mainAudio.Play();

        titleBtn[0].onClick.AddListener(() => PopUpController(true));
        titleBtn[1].onClick.AddListener(() => PopUpController(false));
    }

    public void PopUpController(bool _bool)
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        createPopUp.SetActive(_bool);
    }
}