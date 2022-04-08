using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtons : MonoBehaviour
{

    public void OnClickManu()
    {
        Debug.Log("메뉴 화면으로 이동합니다.");
    }

    public void OnClickBuild()
    {
        Debug.Log("건설 화면으로 이동합니다.");
    }

    public void OnClickSoldiers()
    {
        Debug.Log("병력 화면으로 이동합니다.");
    }
}