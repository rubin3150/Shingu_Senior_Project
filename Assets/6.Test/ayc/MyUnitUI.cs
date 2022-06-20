using UnityEngine;
using UnityEngine.UI;

public class MyUnitUI : MonoBehaviour
{
    public GameObject[] matching;
    public Button[] myunitBtn;


    void Start()
    {
        Data.Instance.mainAudio.clip = Data.Instance.selectunitClip;
        Data.Instance.mainAudio.Play();
        
        myunitBtn[0].onClick.AddListener(() => UnitMatching(0));
        myunitBtn[1].onClick.AddListener(() => UnitMatching(1));
        myunitBtn[2].onClick.AddListener(() => UnitMatching(2));
        myunitBtn[3].onClick.AddListener(() => UnitMatching(3));
        myunitBtn[4].onClick.AddListener(() => UnitMatching(4));
        myunitBtn[5].onClick.AddListener(() => UnitMatching(5));
        myunitBtn[6].onClick.AddListener(() => UnitMatching(6));
    }

    private void OnDisable()
    {
        Data.Instance.mainAudio.clip = Data.Instance.worldmapClip;
        Data.Instance.mainAudio.Play();
    }
    public void UnitMatching(int _int)
    {
        for(int i=0; i<matching.Length; i++)
        {
            matching[i].SetActive(false);
        }
        matching[_int].SetActive(true);

        Data.Instance.sfx.clip = Data.Instance.btnClip;
        Data.Instance.sfx.Play();
    }
}