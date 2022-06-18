using UnityEngine;
using UnityEngine.UI;

public class Temp_WorldMapUI : MonoBehaviour
{
    public GameObject[] popupUI;
    /*
    0. 상점
    1. 내 유닛
    2. 환경 설정
    temp-2. 재료 가공
    3. 재료 가공 Plus
    */

    public Button[] worldMapBtn;
    /*
    0. 상점 아이콘
    1. 내 유닛 아이콘
    2. 환경 설정 아이콘
    3. 상점 - 닫기 버튼
    4. 환경설정 - 닫기 버튼
    5. 환경설정 - 게임종료 버튼
    temp-6. Building 버튼
    7. 재료가공 - X 버튼
    8. 재료가공 - 가공하기 버튼
    */

    void Start()
    {
        Data.Instance.audio.clip = Data.Instance.worldmapClip;
        Data.Instance.audio.Play();

        worldMapBtn[0].onClick.AddListener(() => PopUpOnController(0));
        worldMapBtn[1].onClick.AddListener(TempMyUnit);
        worldMapBtn[2].onClick.AddListener(() => PopUpOnController(1));
        worldMapBtn[3].onClick.AddListener(() => PopUpOffController(0));
        worldMapBtn[4].onClick.AddListener(() => PopUpOffController(1));
        worldMapBtn[5].onClick.AddListener(ExitPage);
        worldMapBtn[6].onClick.AddListener(() => PopUpOnController(2));
        worldMapBtn[7].onClick.AddListener(TempOff);
        worldMapBtn[8].onClick.AddListener(() => PopUpOnController(3));
    }

    public void PopUpOnController(int _int)
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        popupUI[_int].SetActive(true);
    }
    public void PopUpOffController(int _int)
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        popupUI[_int].SetActive(false);
    }
    public void TempOff()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
        
        for(int i=2;i<=3;i++)
        {
            popupUI[i].SetActive(false);
        }
    }


    public void TempMyUnit()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        Debug.Log("내 유닛 창으로 이동했습니다.");
    }

    public void ExitPage()
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}