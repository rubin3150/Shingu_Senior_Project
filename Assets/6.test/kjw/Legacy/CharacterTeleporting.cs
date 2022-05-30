using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTeleporting : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private bool isCharaterMove;
    private int wallLayerMask = 1 << 6;
    //private int unitLayerMask = 1 << 7;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Teleporting();

        LookMoveDirection();
    }

    public void Teleporting()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue))
        {
            if(hit.transform.tag == "Unit")
            {
                return;
            }
        }

        if (Physics.Raycast(ray, out hit, 50f, wallLayerMask))
        {
            this.transform.position = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
            isCharaterMove = true;
        }
    }

    private void LookMoveDirection()
    { 
        if (isCharaterMove) 
        {
            
        } 
    }
}
