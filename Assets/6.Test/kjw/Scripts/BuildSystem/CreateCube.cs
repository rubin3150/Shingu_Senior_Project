using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CreateCube : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    bool isbool;
    GameObject game;
    private void OnEnable()
    {
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                if (isbool)
                    game = Instantiate(prefab1, transform);
                else
                    game = Instantiate(prefab2, transform);
                isbool = !isbool;
                game.transform.position = new Vector3(-i, 0, j);
                game.name = $"{i * 13 + j}";
            }
        }
    }
}
