using UnityEngine;
using UnityEngine.UI;

public class Temp_WorldMapUI : MonoBehaviour
{
    public GameObject[] popupUI;
    /*
    0. 상점
    1. 내 유닛
    2. 환경 설정    
    */

    public Button[] worldMapBtn;
    /*
    0. 상점 아이콘
    1. 내 유닛 아이콘
    2. 환경 설정 아이콘
    3. 상점 - 닫기 버튼
    4. 환경설정 - 닫기 버튼
    5. 환경설정 - 게임종료 버튼
    */

    void Start()
    {
        worldMapBtn[0].onClick.AddListener(() => PopUpOnController(0));
        worldMapBtn[1].onClick.AddListener(TempMyUnit);
        worldMapBtn[2].onClick.AddListener(() => PopUpOnController(1));
        worldMapBtn[3].onClick.AddListener(() => PopUpOffController(0));
        worldMapBtn[4].onClick.AddListener(() => PopUpOffController(1));
        worldMapBtn[5].onClick.AddListener(ExitPage);
    }

    public void PopUpOnController(int _int)
    {
        popupUI[_int].SetActive(true);
    }
    public void PopUpOffController(int _int)
    {
        popupUI[_int].SetActive(false);
    }

    public void TempMyUnit()
    {
        Debug.Log("내 유닛 창으로 이동했습니다.");
    }

    public void ExitPage()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}