using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBarController : MonoBehaviour
{
    static string nextScene;
    public Image LoadingBar_Center;
    const float THRESHOLD = 0f;
    public GameObject loadingBar;
    public GameObject toTitleText;

    public static void LoadScene(string sceneName) {
        nextScene = sceneName;
        SceneManager.LoadScene("1.Loading");
    }
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }
    IEnumerator LoadSceneProcess() {

        if(! FakeScript.firstLoading) {
            Destroy(loadingBar);
            toTitleText.SetActive(true);
            yield break;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;

        while(!op.isDone) {
            yield return null;

            if(op.progress < THRESHOLD) {
                LoadingBar_Center.fillAmount = op.progress;
            }
            else {
                timer += Time.unscaledDeltaTime;
                LoadingBar_Center.fillAmount = Mathf.Lerp(THRESHOLD, 1f, timer *0.2f);
                if(LoadingBar_Center.fillAmount >= 1f) {
                    toTitleText.SetActive(true);
                    Destroy(loadingBar);
                    if(Input.GetMouseButtonDown(0)) {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}
