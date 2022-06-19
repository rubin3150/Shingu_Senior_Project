using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineSwitch : MonoBehaviour
{
    public MeshRenderer mesh;
    void OnMouseEnter()
    {
        mesh.enabled = true;
    }

    void OnMouseDown()
    {
        mesh.enabled = false;
    }

    void OnMouseExit()
    {
        mesh.enabled = false;
    }
}
