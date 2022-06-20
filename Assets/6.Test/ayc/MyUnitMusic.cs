using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitMusic : MonoBehaviour
{
    void Start()
    {
        Data.Instance.mainAudio.clip = Data.Instance.selectunitClip;
        Data.Instance.mainAudio.Play();
    }

    private void OnDisable()
    {
        Data.Instance.mainAudio.clip = Data.Instance.worldmapClip;
        Data.Instance.mainAudio.Play();
    }
}
