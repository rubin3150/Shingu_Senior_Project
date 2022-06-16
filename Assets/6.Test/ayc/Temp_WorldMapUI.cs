using UnityEngine;
using UnityEngine.UI;

public class Temp_WorldMapUI : MonoBehaviour
{
    public GameObject popupUI;

    public Button[] titleBtn;
    /*
    0. 게임 시작 버튼 - Pop-up UI On
    1. 환경 설정 버튼 - TenoSetting 함수 실행
    2. Pop-up 취소 버튼
    */

    void Start()
    {
        titleBtn[0].onClick.AddListener(() => PopUpController(true));
        titleBtn[1].onClick.AddListener(TempSetting);
        titleBtn[2].onClick.AddListener(() => PopUpController(false));
    }

    public void PopUpController(bool _bool)
    {
        popupUI.SetActive(_bool);
    }
    
    public void TempSetting()
    {
        Debug.Log("설정창이 켜졌습니다.");
    }
}