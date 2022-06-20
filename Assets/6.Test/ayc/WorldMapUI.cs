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

        worldMapBtn[0].onClick.AddListener(() => PopUpController(0, true));
        worldMapBtn[1].onClick.AddListener(() => PopUpController(1, true));
        worldMapBtn[2].onClick.AddListener(() => PopUpController(2, true));
        worldMapBtn[3].onClick.AddListener(() => PopUpController(0, false));
        worldMapBtn[4].onClick.AddListener(() => PopUpController(1, false));
        worldMapBtn[5].onClick.AddListener(() => PopUpController(3, false));
        worldMapBtn[6].onClick.AddListener(() => PopUpController(4, false));
        worldMapBtn[7].onClick.AddListener(() => PopUpController(5, false));
        worldMapBtn[8].onClick.AddListener(() => PopUpController(6, false));
        worldMapBtn[9].onClick.AddListener(loadDefence);
        worldMapBtn[10].onClick.AddListener(loadDefence);
        worldMapBtn[11].onClick.AddListener(loadDefence);
        worldMapBtn[12].onClick.AddListener(loadDefence);
    }

    // Pop-Up On = (_bool == true)
    public void PopUpController(int _int, bool _bool)
    {
        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();

        blur.enabled = _bool;

        CameraMove.Instance.isMove = !(_bool);
        popup[_int].SetActive(_bool);
    }

    public void loadDefence()
    {
        SceneManager.LoadScene(1);
    }
}