using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreMusic : MonoBehaviour
{
    void Start()
    {
        Data.Instance.mainAudio.clip = Data.Instance.storeClip;
        Data.Instance.mainAudio.Play();
    }

    private void OnDisable()
    {
        Data.Instance.mainAudio.clip = Data.Instance.worldmapClip;
        Data.Instance.mainAudio.Play();
    }
}
