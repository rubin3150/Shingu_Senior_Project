using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustForFun : MonoBehaviour
{
    public MeshRenderer go;
    public bool iss;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            iss = true;
            go.enabled = true;
            StartCoroutine("On");
            Invoke(nameof(Off), 10f);
        }
    }

    IEnumerator On()
    {
        while(iss)
        {
            this.transform.Translate(Vector3.up * Time.deltaTime * 15f);
            yield return null;
        }
    }

    public void Off()
    {
        go.enabled = false;
        iss = false;
    }
}
