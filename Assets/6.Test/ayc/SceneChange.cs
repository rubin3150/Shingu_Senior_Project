using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject stage;

    public void SceneChanger()
    {
        SceneManager.LoadScene("Defence 1");
        DontDestroyOnLoad(stage);
    }
}