using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineSwitch : MonoBehaviour
{
    public MeshRenderer mesh;
    void OnMouseEnter()
    {
        if(!CameraRay.Instance.isEditing)
            mesh.enabled = true;
    }

    void OnMouseDown()
    {
        if(!CameraRay.Instance.isEditing)
            mesh.enabled = false;
    }

    void OnMouseExit()
    {
        mesh.enabled = false;
    }
}
