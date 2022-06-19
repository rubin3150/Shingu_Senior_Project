using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    public Image blur;
    public GameObject[] popup;
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
    4. 내 유닛 - 닫기 버튼
    */

    void Start()
    {
        Data.Instance.mainAudio.clip = Data.Instance.worldmapClip;
        Data.Instance.mainAudio.Play();

        worldMapBtn[0].onClick.AddListener(() => PopUpOnController(0, false));
        worldMapBtn[1].onClick.AddListener(() => PopUpOnController(1, false));
        worldMapBtn[2].onClick.AddListener(() => PopUpOnController(2, false));
        worldMapBtn[3].onClick.AddListener(() => PopUpOffController(0, true));
        worldMapBtn[4].onClick.AddListener(() => PopUpOffController(1, true));
    }

    public void PopUpOnController(int _int, bool _bool)
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        blur.enabled = true;

        CameraMove.Instance.isMove = _bool;
        popup[_int].SetActive(true);
    }
    public void PopUpOffController(int _int, bool _bool)
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        blur.enabled = false;

        CameraMove.Instance.isMove = _bool;
        popup[_int].SetActive(false);
    }
}