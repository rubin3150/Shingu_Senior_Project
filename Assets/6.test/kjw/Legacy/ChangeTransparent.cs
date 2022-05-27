using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTransparent : MonoBehaviour
{
    // only works on "UnityChanToonShader/NoOutline/ToonColor_ShadingGradeMap_Transparent"
    public Renderer rd;
    public GameObject obj;
    public Material mat;
    public float matF;

    private void Start() 
    {
        obj = this.transform.gameObject;
        rd = obj.GetComponent<Renderer>();
        mat = obj.GetComponent<Renderer>().material;
        matF = obj.GetComponent<Renderer>().material.GetFloat("_Tweak_transparency");
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) //change Input require
        {
            rd.material.SetFloat("_Tweak_transparency", -0.5f);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) //change Input require
        {
            rd.material.SetFloat("_Tweak_transparency", 0f);
        }    
    }
}
