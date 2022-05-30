using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public NavMeshAgent agent;

    private Ray ray;
    private RaycastHit hit;
    private bool isCharaterMove;
    private int wallLayerMask = 1 << 6;
    //private int unitLayerMask = 1 << 7;


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            SetDestination();

        LookMoveDirection();
    }

    public void SetDestination()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue))
        {
            if(hit.transform.tag == "Unit")
            {
                return;
                //Debug.Log("unit ����");
            }
        }

        if (Physics.Raycast(ray, out hit, 50f, wallLayerMask))
        {
            agent.SetDestination(new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z));
            isCharaterMove = true;
        }
    }

    private void LookMoveDirection()
    { 
        if (isCharaterMove) 
        {
            if (agent.velocity.magnitude == 0.0f)
            {
                isCharaterMove = false;
                return; 
            } 
            Vector3 dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
            this.transform.forward = dir; 
        } 
    }
}
