using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitAdd : MonoBehaviour
{
    public GameObject[] Units;

    public SelectUnitBase theBase;

    private int _r;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            _r = Random.Range(0, Units.Length);

            if (Units[_r] != null)
            {
                theBase.AddMaxUnit(Units[_r].GetComponent<UnitPickUp>().unit);
                Units[_r] = null;
            }
            else if (Units[_r] == null)
            {
                
                _r = Random.Range(0, Units.Length);
                if (Units[_r] != null)
                {
                    theBase.AddMaxUnit(Units[_r].GetComponent<UnitPickUp>().unit);
                }

                Units[_r] = null;
            }
        }
    }
}
