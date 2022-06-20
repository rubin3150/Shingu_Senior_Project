using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMapUI : MonoBehaviour
{
    public Image blur;
    public GameObject[] popup;
    /*
    0. 상점
    1. 내 유닛
    2. 환경 설정
    3. 전투입장1
    4. 전투입장2
    5. 전투입장3
    6. 전투입장4
    */

    public Button[] worldMapBtn;
    /*
    0. 상점 아이콘
    1. 내 유닛 아이콘
    2. 환경 설정 아이콘
    3. 상점 - 닫기 버튼
    4. 내 유닛 - 닫기 버튼
    5. 전투입장1 - 닫기 버튼
    6. 전투입장2 - 닫기 버튼
    7. 전투입장3 - 닫기 버튼
    8. 전투입장4 - 닫기 버튼
    9. 전투입장1 - 입장 버튼
    10. 전투입장2 - 입장 버튼
    11. 전투입장3 - 입장 버튼
    12. 전투입장4 - 입장 버튼
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
        worldMapBtn[5].onClick.AddListener(() => PopUpOffController(3, true));
        worldMapBtn[6].onClick.AddListener(() => PopUpOffController(4, true));
        worldMapBtn[7].onClick.AddListener(() => PopUpOffController(5, true));
        worldMapBtn[8].onClick.AddListener(() => PopUpOffController(6, true));
        worldMapBtn[9].onClick.AddListener(loadDefence);
        worldMapBtn[10].onClick.AddListener(loadDefence);
        worldMapBtn[11].onClick.AddListener(loadDefence);
        worldMapBtn[12].onClick.AddListener(loadDefence);
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
    public void loadDefence()
    {
        SceneManager.LoadScene(1);
    }
}