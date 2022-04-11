using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MoveUnit : MonoBehaviour
{
    private RectTransform trans; 
    private RaycastHit _hit;
    public LayerMask enemyLayer;
     
    private void Update()
    {
        {
            GetComponent<RectTransform>().localPosition += new Vector3(1, 0, 0);
        }
        

        if (Physics.Raycast(transform.position, Vector3.right, out _hit, 20, enemyLayer))
        {
            _hit.transform.GetComponent<Monster>().A();
            transform.GetComponent<RectTransform>().localPosition -= new Vector3(200, 0,0 );
        }
    }
}