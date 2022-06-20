using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void SceneChanger()
    {
        SceneManager.LoadScene("Defence");
    }
}