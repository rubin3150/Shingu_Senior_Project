using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int destroyTime;
    void OnEnable()
    {
        Invoke("Self", destroyTime);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float t = 0;
        while (t < destroyTime)
        {
            t += Time.deltaTime;
            text.alpha = Mathf.Lerp(1, 0, t / destroyTime);
            yield return null;
        }
    }

    void Self()
    {
        gameObject.SetActive(false);
    }
}
