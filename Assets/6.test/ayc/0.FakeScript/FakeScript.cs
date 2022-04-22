using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FakeScript : MonoBehaviour
{
    public static bool firstLoading = true;

    void Update()
    {
        if(firstLoading) {
            LoadingBarController.LoadScene("2.Title");
        }
        else {
            SceneManager.LoadScene("1.Loading");
        }
    }
}